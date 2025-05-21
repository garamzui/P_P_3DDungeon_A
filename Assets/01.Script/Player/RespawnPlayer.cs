using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RespawnPlayer : MonoBehaviour
{
    public GameObject player;
    private void Awake()
    {
     player.transform.position = transform.position;
        
    }
    void OnEnable()
    {
        Debug.Log("오브젝트가 SetActive(true)로 활성화될 때 호출됨!");
    }
    
    private void Start()
    {
        //PlayerManager.Instance.Player.condition.Resurrection();
        
    }
}
