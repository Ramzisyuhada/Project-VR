using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class InputController : MonoBehaviour
{
    [SerializeField]
    private LineRenderer lineRenderer; // Komponen XRRayInteractor dari controller

    [SerializeField]
    private Camera camera;
    [SerializeField]
    private LayerMask layerMask;

    private Vector3 _lastpoint;

    public Vector3 GetSelectedMapPosisition()
    {
        Vector3 endOfRay = lineRenderer.positionCount > 0 ? lineRenderer.GetPosition(lineRenderer.positionCount - 1) : Vector3.zero;
        Debug.Log(endOfRay);
        RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(endOfRay);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask)) {
            _lastpoint = hit.point;
        }
        return _lastpoint;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
