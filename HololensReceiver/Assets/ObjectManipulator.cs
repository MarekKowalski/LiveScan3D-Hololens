using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManipulator : MonoBehaviour
{
    public GameObject ManipulatedObject;
    public float ManipulationSensitivity = 0.25f;
    public float RotationSensitivity = 10.0f;
    public float ScaleSensitivity = 0.01f;

	void Start ()
    {
        ActionManager.Instance.ResetEvent += ResetTransform;
	}

	void Update ()
    {
        PerformManipulation();
        PerformRotation();
        PerformZoom();
    }

    void PerformManipulation()
    {
        if (ManipulatedObject == null)
            return;
        if (GestureManager.Instance.IsManipulating)
            ManipulatedObject.transform.position += ManipulationSensitivity * GestureManager.Instance.ManipulationPosition;
    }

    void PerformRotation()
    {
        if (ManipulatedObject == null)
            return;

        if (GestureManager.Instance.IsNavigating &&
            ActionManager.Instance.CurrentAction == ActionManager.ActionType.Rotation)
        {
            float rotationFactor = RotationSensitivity * GestureManager.Instance.NavigationPosition.x;
            ManipulatedObject.transform.Rotate(0, rotationFactor, 0);
        }
    }

    void PerformZoom()
    {
        if (ManipulatedObject == null)
            return;
        if (GestureManager.Instance.IsNavigating && 
            ActionManager.Instance.CurrentAction == ActionManager.ActionType.Zoom)
        {
            float scaleFactor = 1 + ScaleSensitivity * GestureManager.Instance.NavigationPosition.x;
            ManipulatedObject.transform.localScale = ManipulatedObject.transform.localScale * scaleFactor;
        }

    }

    void ResetTransform()
    {
        if (ManipulatedObject == null)
            return;

        ManipulatedObject.transform.position = Vector3.zero;
        ManipulatedObject.transform.rotation = Quaternion.identity;
        ManipulatedObject.transform.localScale = Vector3.one;
    }
}
