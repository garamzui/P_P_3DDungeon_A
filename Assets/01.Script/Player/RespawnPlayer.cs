using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RespawnPlayer : MonoBehaviour
{
    public GameObject playerPrefabs;
    private void Awake()
    {
        Instantiate(playerPrefabs);
        
    }
    private void Start()
    {
        Destroy(this.gameObject);
    }
}
