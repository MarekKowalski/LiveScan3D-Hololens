using UnityEngine;
using System.Collections;
using System.Linq;

public class ElemRenderer : MonoBehaviour
{
    Mesh mesh;

    private void Awake()
    {
    }

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void UpdateMesh(float[] arrVertices, byte[] arrColors, int[] arrTriangles, int VerticesStart, int nVerticesToRender, int TrianglesStart, int nTrianglesToRender)
    {

        int nPoints = nVerticesToRender;
        int nTriangles = nTrianglesToRender * 3;

        Vector3[] points = new Vector3[nPoints];
        int[] indices = new int[nPoints];
        Color[] colors = new Color[nPoints];
        int[] triangles = new int[nTriangles];
        for (int i = 0; i < nPoints; i++)
        {
            int ptIdx = 3 * (VerticesStart + i);
            points[i] = new Vector3(arrVertices[ptIdx + 0], arrVertices[ptIdx + 1], -arrVertices[ptIdx + 2]);
            indices[i] = i;
            colors[i] = new Color((float)arrColors[ptIdx + 0] / 256.0f, (float)arrColors[ptIdx + 1] / 256.0f, (float)arrColors[ptIdx + 2] / 256.0f, 1.0f);
        }

        for (int i = 0; i < nTriangles; i++) {
           triangles[i] = arrTriangles[TrianglesStart + i];
        }

        if (mesh != null)
            Destroy(mesh);
        mesh = new Mesh();
        mesh.vertices = points;
        mesh.colors = colors;
        mesh.triangles = triangles;
        GetComponent<MeshFilter>().mesh = mesh;

    }
}
