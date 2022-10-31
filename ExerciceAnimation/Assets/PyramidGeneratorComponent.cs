using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class PyramidGeneratorComponent : MonoBehaviour
{
    [SerializeField] private float longueurBase = 1;
    [SerializeField] private float hauteur = 1;
    private static Vector3[] baseVerticesFace = new Vector3[]
    {
        // Face Triangulaire
        new Vector3(0,1,0),
        new Vector3(0.5f,0,-0.5f),
        new Vector3(-0.5f,0,-0.5f),
    };
    private static Vector3[] baseBaseVertices = new Vector3[]
    {
        // Base
        new Vector3(0.5f,0,-0.5f),
        new Vector3(-0.5f,0,-0.5f),
        new Vector3(-0.5f,0,0.5f)
    };
    private static Matrix4x4[] rotationsBases = new[]
    {
        Matrix4x4.Rotate(Quaternion.identity),
        Matrix4x4.Rotate(Quaternion.Euler(0,180,0))
    };
    private static Matrix4x4[] rotationsFaces = new []
    {
        Matrix4x4.Rotate(Quaternion.identity),
        Matrix4x4.Rotate(Quaternion.Euler(0,90,0)),   
        Matrix4x4.Rotate(Quaternion.Euler(0,180,0)),   
        Matrix4x4.Rotate(Quaternion.Euler(0,270,0)),

    };

    private static int[] baseTris = new int[]
    {
        0,1,2
    };
    private static int[] baseBaseTris = new int[]
    {
        14,13,12,17,16,15
    };
    private static Vector2[] baseUvFace = new Vector2[]
    {
        new Vector2(0.5f,1),
        new Vector2(1,0.5f),
        new Vector2(0,0.5f)
    };
    private static Vector2[] baseUvBase = new Vector2[]
    {
        new Vector2(1,0.5f),
        new Vector2(0,0.5f),
        new Vector2(0,0),
        new Vector2(0,0),
        new Vector2(1,0),
        new Vector2(1,0.5f),
    };
    private void Awake()
    {
        Mesh m = GetComponent<MeshFilter>().mesh;
        m.vertices = GenerateVertices();
        m.triangles = GenerateTris();
        m.uv = GenerateUvs();
        m.RecalculateNormals();
    }
    private Vector3[] GenerateVertices()
    {
        Vector3[] vertices = new Vector3[18];
        
        int compteur = 0;
        //Génère les vertex des faces triangulaires
        for(int t = 0; t < 4; t++)
        {
            for (int k = 0; k < 3; k++)
            {
                vertices[compteur] = rotationsFaces[t].MultiplyPoint(baseVerticesFace[k]);
                compteur++;
            }
        }
        // Génère les vertex des bases
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                vertices[compteur] = rotationsBases[i].MultiplyPoint(baseBaseVertices[j]);
                compteur++;
            }
        }
        
        return vertices;
    }
    
    private int[] GenerateTris()
    {
        int[] tris = new int[24];

        
        int currentTriIndex = 0;
        
        // Pour chaque face de la pyramide...
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < baseTris.Length; j++)
            {
                tris[currentTriIndex++] = baseTris[j] + (3 * i);
            }
        }
        // Pour chaque triangle de la base...
        for (int i = 0; i < 6; i++)
        {
            tris[currentTriIndex] = baseBaseTris[i];
            currentTriIndex++;

        }
        
        return tris;   
    }

    private Vector2[] GenerateUvs()
    {
        Vector2[] uvs = new Vector2[18];
       
        int compteur = 0;
        // UV des faces...
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                uvs[compteur] = baseUvFace[j];
                compteur++;
            }
        }

        for (int i = 0;i<6; i++)
        {
            uvs[compteur] = baseUvBase[i];
            compteur++;
        }
        
        return uvs;
    }
}
