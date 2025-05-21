using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class PlayerCondition : MonoBehaviour
{
    public UICondition uiCondition;

    Condition health { get {  return uiCondition.health; } }
    Condition stamina { get { return uiCondition.stamina; } }

        

        void Update()
    {
        stamina.Add(stamina.passiveValue * Time.deltaTime);
        health.Add(health.passiveValue * Time.deltaTime);
    }
}
