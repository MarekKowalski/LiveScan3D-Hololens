using UnityEngine;
using System.Collections.Generic;

public class PointCloudRenderer : MonoBehaviour
{
    public int maxChunkSize = 65535;
    public float pointSize = 0.005f;
    public GameObject pointCloudElem;
    public Material pointCloudMaterial;

    List<GameObject> elems;

    void Start()
    {
        elems = new List<GameObject>();
        UpdatePointSize();
    }

    void Update()
    {
        if (transform.hasChanged)
        {
            UpdatePointSize();
            transform.hasChanged = false;
        }
    }

    void UpdatePointSize()
    {
        pointCloudMaterial.SetFloat("_PointSize", pointSize * transform.localScale.x);
    }

    public void Render(float[] arrVertices, byte[] arrColors, int[] arrTriangles, int[] chunksVertices, int[] chunksTriangles)
    {

        
        int nChunks = chunksVertices.length;

        int VerticesRead = 0;
        int TrianglesRead = 0;

        if (elems.Count < nChunks)
            AddElems(nChunks - elems.Count);
        if (elems.Count > nChunks)
            RemoveElems(elems.Count - nChunks);
        for (int i = 0; i < nChunks; i++)
        {

            ElemRenderer renderer = elems[i].GetComponent<ElemRenderer>();
            renderer.UpdateMesh(arrVertices, arrColors, arrTriangles, VerticesRead, chunksVertices[i], TrianglesRead*3, chunksTriangles[i]);
                  VerticesRead += chunksVertices[i];
                  TrianglesRead += chunksTriangles[i];
        }
    }

    void AddElems(int nElems)
    {
        for (int i = 0; i < nElems; i++)
        {
            GameObject newElem = GameObject.Instantiate(pointCloudElem);
            newElem.transform.parent = transform;
            newElem.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
            newElem.transform.localRotation = Quaternion.identity;
            newElem.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

            elems.Add(newElem);
        }            
    }

    void RemoveElems(int nElems)
    {
        for (int i = 0; i < nElems; i++)
        {
            Destroy(elems[0]);
            elems.Remove(elems[0]);
        }
    }
}
