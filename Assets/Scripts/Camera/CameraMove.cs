using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Camera RaycastCamera;

    Vector3 _startPoint;
    Vector3 _cameraStartPosition;
    Plane _plane;
    void Start()
    {
        _plane = new Plane(Vector3.up, Vector3.zero);
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = RaycastCamera.ScreenPointToRay(Input.mousePosition);

        float distance;
        _plane.Raycast(ray, out distance);
        Vector3 point = ray.GetPoint(distance);

        if(Input.GetMouseButtonDown(2))
        {
            _startPoint = point;
            _cameraStartPosition = transform.position;
        }

        if(Input.GetMouseButton(2))
        {
            Vector3 offset = point - _startPoint;
            transform.position = _cameraStartPosition - offset;
        }

        transform.Translate(0, 0, Input.mouseScrollDelta.y);
        RaycastCamera.transform.Translate(0, 0, Input.mouseScrollDelta.y);
    }
}
