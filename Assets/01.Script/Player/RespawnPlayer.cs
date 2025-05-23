using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RespawnPlayer : MonoBehaviour
{
   public static RespawnPlayer instance{ get; private set; }
     
    
    public GameObject player;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            
        }
        else
        {
            Destroy(gameObject); // 중복 방지
        }

    }

    public void Respawn()
    {
        player.transform.position = transform.position;
        PlayerManager.Instance.player.condition.Resurrection();
    }

}
