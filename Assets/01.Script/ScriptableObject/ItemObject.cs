
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public string GetInteractPrompt();
    public void OnInteract();
}

public class ItemObject : MonoBehaviour, IInteractable
{
    public ItemData data;
     
    

    public string GetInteractPrompt()
    {
        string str = $"{data.displayName}\n{data.description}";
        return str;
    }
    public void OnInteract()
    {
        
        PlayerManager.Instance.Player.itemData = data;
        PlayerManager.Instance.Player.addItem?.Invoke();
        Destroy(gameObject);
    }
// 밸류 못가져오고 있으므로 다시 보기 
    private void OnTriggerEnter(Collider other )
    {
        float value;
        if (other.gameObject.CompareTag("Player"))
        {
            if (CompareTag("HP"))
            {
                value = PlayerManager.Instance.Player.condition.health.passiveValue;
                PlayerManager.Instance.Player.condition.GetInstantItem(value);
            }
            else if (CompareTag("STA"))
            {
                value = PlayerManager.Instance.Player.condition.stamina.passiveValue;
                PlayerManager.Instance.Player.condition.GetInstantItem(value);
            }
            else if (CompareTag("JUMP"))
            {
                value = PlayerManager.Instance.PlayerController.JumpPower;
                PlayerManager.Instance.PlayerController.GetInstantItem(value);
            }
            else if (CompareTag("SPEED"))
            {
                value = PlayerManager.Instance.PlayerController.MoveSpeed;
                PlayerManager.Instance.PlayerController.GetInstantItem(value);
            }

           
            
            OnInteract();
        }
    }

}
