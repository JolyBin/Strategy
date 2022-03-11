using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Healthbar : MonoBehaviour
{
    public Transform ScaleTransform;

    Transform _cameraTransform;
    void Start()
    {
        _cameraTransform = Camera.main.transform;
    }

    void LateUpdate()
    {
        transform.rotation = _cameraTransform.rotation;
    }

    public void SetHealth(int health, int maxHealth)
    {
        float xScale = Mathf.Clamp01((float)health / maxHealth);
        ScaleTransform.localScale = new Vector3(xScale, 1f, 1f);
    }
}
