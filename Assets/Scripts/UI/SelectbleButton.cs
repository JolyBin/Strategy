using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class SelectbleButton : MonoBehaviour
{
    public GameObject SelectblePrefab;
    public Text PriceText;

    private void Start()
    {
        PriceText.text = SelectblePrefab.GetComponent<SelectableObject>().Price.ToString();
    }
    public abstract void TryBuy();
}
