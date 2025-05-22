using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public ItemData item;
    public Image icon;
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI slotNumber;

    public UIQuickBoard UQB;
    /*public UIInventory inventory;*/

    public int index;
    public bool equipped;
    public int quantity;
    public void Set()
    { 
        icon.gameObject.SetActive(true);
        icon.sprite = item.icon;
       itemNameText.text = item.displayName;
    }

    public void Clear()
    {
        item = null;
        icon.gameObject.SetActive(false);
        itemNameText.text = string.Empty;
    }
}
