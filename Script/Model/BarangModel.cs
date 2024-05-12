using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarangModel 
{
    public GameObject GameObject { get; private set; }
    public Vector3 OriginalPosition { get; private set; }
    public Material OriginalMaterial { get; private set; }

    public BarangModel(GameObject obj)
    {
        GameObject = obj;
        OriginalPosition = obj.transform.position;
        OriginalMaterial = obj.GetComponent<MeshRenderer>().material;
    }
}
