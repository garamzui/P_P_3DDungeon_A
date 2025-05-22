
using UnityEngine;

public class UIQuickBoard : MonoBehaviour
{
    public static UIQuickBoard Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
    }
    
    public ItemSlot[] slots;

   
    public Transform slotPanel;
    
    
    private PlayerController controller;
    private PlayerCondition condition;

    ItemData data;
    int selectedItemIndex = 0;

    int curEquipIndex;



    // Start is called before the first frame update
    void Start()
    {
        controller = PlayerManager.Instance.Player.controller;
        condition = PlayerManager.Instance.Player.condition;
        PlayerManager.Instance.Player.addItem += AddItem;
        slots = new ItemSlot[slotPanel.childCount];

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = slotPanel.GetChild(i).GetComponent<ItemSlot>();
            slots[i].index = i;
            slots[i].slotNumber.text = (i+1).ToString();
            slots[i].UQB = this;
        }
        
    }
    void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].itemData != null)
            {
                slots[i].Set();
            }
            else
            {
                slots[i].Clear();
            }
        }
    }
    
    
    void AddItem()
    {
        ItemData data = PlayerManager.Instance.Player.itemData;

       
        ItemSlot emptySlot = GetEmptySlot();
      

        if (emptySlot != null)
        {
            emptySlot.itemData = data;
           
            UpdateUI();
            PlayerManager.Instance.Player.itemData = null;
          
        }
        

        
        
        

    }

   public bool IsSlotFull()
    {
        int full = 0;
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].itemData != null)
            {
                full++;
            }
           
        }

        if (full == slots.Length)
        {
            return true;
        }

        return false;

    }

    ItemSlot GetEmptySlot()
    {
        for (int i = 0; i < slots.Length; i++)
        { 
            if (slots[i].itemData == null)
            {
                return slots[i];
            }
        }
        return null;
    }

    public ItemSlot ChooseSlot(int slotIdx)
    {
     
        
        return slots[slotIdx];


    }

}

