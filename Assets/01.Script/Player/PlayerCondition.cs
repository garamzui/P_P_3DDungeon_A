using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface IDamagable
{
    void TakePhysicalDamage(int damage);
}
    public class PlayerCondition : MonoBehaviour, IDamagable
{
    public UICondition uiCondition;

    Condition health { get {  return uiCondition.health; } }
    Condition stamina { get { return uiCondition.stamina; } }

        public void TakePhysicalDamage(int damage)
        {
            throw new System.NotImplementedException();
        }

        void Update()
    {
        stamina.Add(stamina.passiveValue * Time.deltaTime);
        health.Add(health.passiveValue * Time.deltaTime);
    }
}
