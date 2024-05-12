using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PlacementController : MonoBehaviour
{
    [SerializeField]
    private InputController rayInteractor; // Komponen XRRayInteractor dari controller
    [SerializeField]
    private GameObject _Indicator;
    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
        // Get the position of the controller
        Vector3 endOfRay = rayInteractor.GetSelectedMapPosisition();
        // Set the position of the indicator to the controller's position
        _Indicator.transform.position = endOfRay;

    }
}
