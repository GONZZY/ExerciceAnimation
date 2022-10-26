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
    private static Vector3[] baseVertices = new Vector3[]
    {
        // Base
        new Vector3(0,1,0),
        new Vector3(0.5f,0,-0.5f),
        new Vector3(-0.5f,0,-0.5f),
    };
    private static Matrix4x4[] rotations = new []
    {
        Matrix4x4.Rotate(Quaternion.identity),   
        
        Matrix4x4.Rotate(Quaternion.Euler(0,90,0)),   
        Matrix4x4.Rotate(Quaternion.Euler(0,180,0)),   
        Matrix4x4.Rotate(Quaternion.Euler(0,270,0)),

    };

    private static int[] baseTris = new int[]
    {
        0,1,2,2,3,0
    };
    private void Awake()
    {
        Mesh m = GetComponent<MeshFilter>().mesh;
        m.vertices = GenerateVertices();
        m.triangles = GenerateTris();
        m.RecalculateNormals();
    }
    private Vector3[] GenerateVertices()
    {
        // Générer les sommets
        // Comment?
        // Créer les 4 premiers sommets et additionner la
        // rotation de ces sommets autour de l'origine du cube
        Vector3[] vertices = new Vector3[16];

        int currentVertex = 0;
        // Pour chaque rotation je veux créer un nouveau sommet 4 fois avec cette rotation
        // Pour chaque rotation...
        for (int i = 0; i < rotations.Length; ++i)
        {
            // Pour chaque base vertex...
            for (int j = 0; j < baseVertices.Length; ++j)
            {
                vertices[currentVertex++] = rotations[i].MultiplyPoint(baseVertices[j]);
                //++currentVertex;
            }
        }

        return vertices;
    }

    private int[] GenerateTris()
    {
        int[] tris = new int[18];

        int currentTriIndex = 0;
        
        // Pour chaque face de la pyramide...
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < baseTris.Length; j++)
            {
                tris[currentTriIndex++] = baseTris[j] + (4 * i);
            }
        }

        return tris;   
    }
}

    
    


