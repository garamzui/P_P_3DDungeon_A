using System;
using System.Collections;

using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCondition : MonoBehaviour
{
    public UICondition uiCondition;
   
    
    
   public  Condition health
    {
        get { return uiCondition.health; }
    }

    public Condition stamina
    {
        get { return uiCondition.stamina; }
    }

    private void Awake()
    {
        
    }

    void Update()
    {
        stamina.Add(stamina.passiveValue * Time.deltaTime);
        health.Add(health.passiveValue * Time.deltaTime);

        if (health.curValue <= 0)
        {
            OnDie();
        }
        
    }

    public void StaminaForJump()
    {
        stamina.Substrack(stamina.consumeValue);
    }
    
    
    private void OnDie()
    {
        Debug.Log("죽었다");
        StartCoroutine(Die());
    }

    private IEnumerator Die()
    {
        //애니메이션 넣기
        yield return new WaitForSeconds(1);
        
        yield return new WaitForSeconds(1);
        
    }

    public void Resurrection()
    {
        health.curValue = health.maxValue;
    }
}