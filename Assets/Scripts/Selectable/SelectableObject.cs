using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableObject : MonoBehaviour
{

    public GameObject[] SelectionIndicators;
    public int Price = 5;

    protected virtual void Start()
    {
        Unselect();
    }
    public virtual void OnHover()
    {
        transform.localScale = Vector3.one * 1.1f;
    }

    public virtual void OnUnhover()
    {
        transform.localScale = Vector3.one;
    }

    public virtual void Select()
    {
        foreach (GameObject selectionIndicator in SelectionIndicators)
        {
            selectionIndicator.SetActive(true);
        }
        
    }

    public virtual void Unselect()
    {
        foreach (GameObject selectionIndicator in SelectionIndicators)
        {
            selectionIndicator.SetActive(false);
        }
    }

    public virtual void WhenClickOnGround(Vector3 point)
    {

    }
}
