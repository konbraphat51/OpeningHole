//https://www.youtube.com/watch?v=z1r7VjgufJ8

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutHoleSq : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void DeleteSquare(int index1, int index2)
    {
        Destroy(this.gameObject.GetComponent<MeshCollider>());
        Mesh mesh = transform.GetComponent<MeshFilter>().mesh;
        int[] oldTriangles = mesh.triangles;
        int[] newTriangles = new int[mesh.triangles.Length - 3];

        int i = 0;
        int j = 0;
        while(j < mesh.triangles.Length)
        {
            if (j != index1*3 && j != index2 * 3)
            {
                newTriangles[i++] = oldTriangles[j++];
                newTriangles[i++] = oldTriangles[j++];
                newTriangles[i++] = oldTriangles[j++];
            }
            else
            {
                j += 3;
            }
        }
        transform.GetComponent<MeshFilter>().mesh.triangles = newTriangles;
        this.gameObject.AddComponent<MeshCollider>();
    }

    int FindVertex(Vector3 v)
    {
        Vector3[] vertices = transform.GetComponent<MeshFilter>().mesh.vertices;
        for(int i = 0; i < vertices.Length; i++)
        {
            if(vertices[i] == v)
            {
                return i;
            }
        }
        return -1;
    }
    
    int FindTriangle(Vector3 v1, Vector3 v2, int notTriIndex)
    {
        int[] triangles = transform.GetComponent<MeshFilter>().mesh.triangles;
        Vector3[] vertices = transform.GetComponent<MeshFilter>().mesh.vertices;
        int i = 0;
        int j = 0;
        int found = 0;
        while(j < triangles.Length)
        {
            if (j/3 != notTriIndex)
            {
                if ((vertices[triangles[j]] == v1) && (vertices[triangles[j + 1]] == v2 || vertices[triangles[j + 2]] == v2))
                    return j / 3;
                else if ((vertices[triangles[j]] == v2) && (vertices[triangles[j + 1]] == v1 || vertices[triangles[j + 2]] == v1))
                    return j / 3;
                else if ((vertices[triangles[j + 1]] == v2) && (vertices[triangles[j]] == v1 || vertices[triangles[j + 2]] == v1))
                    return j / 3;
                else if ((vertices[triangles[j + 1]] == v1) && (vertices[triangles[j]] == v2 || vertices[triangles[j + 2]] == v2))
                    return j / 3;
            }
            j += 3;
        }

        return -1;
    }

    void DeleteTri(int index)
    {
        Destroy(this.gameObject.GetComponent<MeshCollider>());
        Mesh mesh = transform.GetComponent<MeshFilter>().mesh;
        int[] oldTriangles = mesh.triangles;
        int[] newTriangles = new int[mesh.triangles.Length - 1];

        int i = 0;
        int j = 0;
        while (j < mesh.triangles.Length)
        {
            if(j != index * 3)
            {
                newTriangles[i++] = oldTriangles[j++];
                newTriangles[i++] = oldTriangles[j++];
                newTriangles[i++] = oldTriangles[j++];
            }
            else
            {
                j += 3;
            }
        }
        transform.GetComponent<MeshFilter>().mesh.triangles = newTriangles;
        this.gameObject.AddComponent<MeshCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out hit, 1000.0f))
            {
                int hitTri = hit.triangleIndex;

                int[] triangles = transform.GetComponent<MeshFilter>().mesh.triangles;
                Vector3[] vertices = transform.GetComponent<MeshFilter>().mesh.vertices;
                Vector3 p0 = vertices[triangles[hitTri * 3 + 0]];
                Vector3 p1 = vertices[triangles[hitTri * 3 + 1]];
                Vector3 p2 = vertices[triangles[hitTri * 3 + 2]];

                float edge1 = Vector3.Distance(p0, p1);
                float edge2 = Vector3.Distance(p0, p2);
                float edge3 = Vector3.Distance(p1, p2);

                Vector3 shared1;
                Vector3 shared2;

                if(edge1 > edge2 && edge1 > edge3)
                {
                    shared1 = p0;
                    shared2 = p1;
                }
                else if(edge2 > edge1 && edge2 > edge3)
                {
                    shared1 = p0;
                    shared2 = p2;
                }
                else
                {
                    shared1 = p1;
                    shared2 = p2;
                }

                int v1 = FindVertex(shared1);
                int v2 = FindVertex(shared2);

                DeleteSquare(hitTri, FindTriangle(vertices[v1], vertices[v2], hitTri));
            }
        }
    }
}
