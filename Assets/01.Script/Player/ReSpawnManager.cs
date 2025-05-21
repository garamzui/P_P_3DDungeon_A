using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReSpawnManager : MonoBehaviour
{
    private static ReSpawnManager instance;
    public static ReSpawnManager Instance{get{return instance;}}

    public GameObject reSpawn;
    private void Awake()
    {
        if (instance == null)
            {
            instance = this;    
            }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ReSpawn()
    {
        Instantiate(reSpawn);
    }

}
