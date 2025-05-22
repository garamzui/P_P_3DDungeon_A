using System;
using System.Collections;
using System.Runtime.ExceptionServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCondition : MonoBehaviour
{
    public UICondition uiCondition;


    public Condition health
    {
        get { return uiCondition.health; }
    }

    public Condition stamina
    {
        get { return uiCondition.stamina; }
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

    public void GetInstantItem(Condition condition, float time)
    {
        StartCoroutine(InstantEffect(condition, time));
    }

    IEnumerator InstantEffect(Condition value, float time)
    {
        value.passiveValue *= 2;
        Debug.Log("증가");

        yield return new WaitForSeconds(time);
        value.passiveValue /= 2;
        Debug.Log("원상복구");
    }

    public void UseConsumableItem(int slotIdx)
    {
        if (UIQuickBoard.Instance.ChooseSlot(slotIdx).itemData != null)
        {
            if (UIQuickBoard.Instance.ChooseSlot(slotIdx).itemData.dropPrefab.CompareTag("C_hp"))
            {
                StartCoroutine(InstantEffect(PlayerManager.Instance.Player.condition.health,
                    UIQuickBoard.Instance.ChooseSlot(slotIdx).itemData.buffTime));
                UIQuickBoard.Instance.ChooseSlot(slotIdx).Clear();
            }

            else if (UIQuickBoard.Instance.ChooseSlot(slotIdx).itemData.dropPrefab.CompareTag("C_sta"))
            {
                StartCoroutine(InstantEffect(PlayerManager.Instance.Player.condition.stamina,
                    UIQuickBoard.Instance.ChooseSlot(slotIdx).itemData.buffTime));
                UIQuickBoard.Instance.ChooseSlot(slotIdx).Clear();
            }

            else if (UIQuickBoard.Instance.ChooseSlot(slotIdx).itemData.dropPrefab.CompareTag("C_speed"))
            {
                PlayerManager.Instance.player.controller.UseConsumableItem(PlayerManager.Instance.player.controller
                    .Haste());
                UIQuickBoard.Instance.ChooseSlot(slotIdx).Clear();
            }
        }
       
    }
}