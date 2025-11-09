using System;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    [SerializeField] private Toggle shop;
    [SerializeField] private GameObject inventory;
    private void Awake() {
        shop.onValueChanged.AddListener(OnShopToggleChanged);
    }

    private void OnShopToggleChanged(bool active)
    {
        if (active)
        {
            inventory.SetActive(true);
        }
        else
        {
            inventory.SetActive(false);
        }
    }
}
