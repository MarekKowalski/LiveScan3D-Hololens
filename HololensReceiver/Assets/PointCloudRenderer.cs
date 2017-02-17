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

    public void Render(float[] arrVertices, byte[] arrColors)
    {
        int nPoints, nChunks;
        if (arrVertices == null || arrColors == null)
        {
            nPoints = 0;
            nChunks = 0;
        }
        else
        {
            nPoints = arrVertices.Length / 3;
            nChunks = 1 + nPoints / maxChunkSize;
        }
        
        if (elems.Count < nChunks)
            AddElems(nChunks - elems.Count);
        if (elems.Count > nChunks)
            RemoveElems(elems.Count - nChunks);

        int offset = 0;
        for (int i = 0; i < nChunks; i++)
        {
            int nPointsToRender = System.Math.Min(maxChunkSize, nPoints - offset);

            ElemRenderer renderer = elems[i].GetComponent<ElemRenderer>();
            renderer.UpdateMesh(arrVertices, arrColors, nPointsToRender, offset);

            offset += nPointsToRender;
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
