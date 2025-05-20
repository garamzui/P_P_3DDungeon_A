using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyAnimation : MonoBehaviour
{
    public Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    void SetMove(bool isMove)
    {

        animator.SetBool("IsMove", isMove);
    }

    void TriggerJump()
    {
        animator.SetTrigger("IsJump");
    }

}
