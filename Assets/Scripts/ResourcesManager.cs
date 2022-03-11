using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourcesManager : MonoBehaviour
{
    public int Money;
    public Text Text;

    private void Start()
    {
        UpdateInfo();
    }

    public void UpdateInfo()
    {
        Text.text = Money.ToString("000");
    }

    public void SetMoney(int value)
    {
        Money += value;
        UpdateInfo();
    }
}
