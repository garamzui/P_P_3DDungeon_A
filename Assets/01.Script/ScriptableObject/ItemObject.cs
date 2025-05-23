using System;
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


    public float moveSpeed;
    public float MaxX;
    public float MinX;
    Rigidbody rb;
    private Transform tf;

    private void FixedUpdate()
    {
        MoveScaffold();
    }

    private void Start()
    {
        FindObjectOfType<PlayerCondition>();
        FindObjectOfType<PlayerController>();
        rb = GetComponent<Rigidbody>();
        tf = GetComponent<Transform>();
    }

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
    private void OnTriggerEnter(Collider other)
    {
        Condition Condition;
        float Time;
        IEnumerator coroutine;
        if (other.gameObject.CompareTag("Player"))
        {
            if (CompareTag("HP"))
            {
                Condition = PlayerManager.Instance.Player.condition.health;
                Time = data.buffTime;
                PlayerManager.Instance.Player.condition.GetInstantItem(Condition, Time);
            }
            else if (CompareTag("STA"))
            {
                Condition = PlayerManager.Instance.Player.condition.stamina;
                Time = data.buffTime;
                PlayerManager.Instance.Player.condition.GetInstantItem(Condition, Time);
            }
            else if (CompareTag("JUMP"))
            {
                coroutine = PlayerManager.Instance.Player.controller.SuperJump();
                PlayerManager.Instance.Player.controller.GetInstantItem(coroutine);
            }
            else if (CompareTag("SPEED"))
            {
                coroutine = PlayerManager.Instance.Player.controller.Haste();
                PlayerManager.Instance.Player.controller.GetInstantItem(coroutine);
            }

            Destroy(gameObject);
        }
    }


    private void MoveScaffold()
    {
        if (this.CompareTag("MoveScaffold"))
        {
            Vector3 move;

            if (tf.localPosition.x < MinX)
            {
                move = rb.position + Vector3.right * moveSpeed * Time.fixedDeltaTime;
                rb.MovePosition(move);
            }
            else if (tf.localPosition.x > MaxX)
            {
                move = rb.position + Vector3.left * moveSpeed * Time.fixedDeltaTime;
                rb.MovePosition(move);
            }
        }
    }

   
}