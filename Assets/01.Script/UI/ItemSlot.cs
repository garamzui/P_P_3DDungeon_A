
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public ItemData itemData;
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
        icon.sprite = itemData.icon;
       itemNameText.text = itemData.displayName;
    }

    public void Clear()
    {
        itemData = null;
        icon.gameObject.SetActive(false);
        itemNameText.text = string.Empty;
    }
}
