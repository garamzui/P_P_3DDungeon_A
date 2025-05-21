using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Condition : MonoBehaviour
{
    public float curValue;
    public float startValue;
    public float maxValue;
    public float passiveValue;
    public float consumeValue;
    public Image uiBar;

    private void Start()
    {
        curValue = startValue;
    }

    private void Update()
    {
        uiBar.fillAmount = GetPercentage();
        
    }

    public float GetPercentage()
    { 
        return curValue/maxValue;
    }


    public void Add(float value)
    {
        curValue = Mathf.Min(curValue + value, maxValue);
    }

    public void Substrack(float value)
    { 
        curValue = Mathf.Max(curValue+ value,0);
    }
    public void Die()
    {
        Debug.Log("죽었다");
    }

}
