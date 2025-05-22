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

   public void SetMove(bool isMove)
    {

        animator.SetBool("IsMove", isMove);
    }

    public void TriggerJump()
    {
        animator.SetTrigger("IsJump");

    }
    public void SetFall(bool isFall)
    {   
        
        animator.SetBool("IsFall",isFall);
    }

    public void TriggerAttack()
    {
        string trigger = "Atk"+Random.Range(1,4).ToString();
        animator.SetTrigger(trigger);
    }

public void SetRun(bool isRun)
{
    animator.SetBool("IsRun", isRun);
}

}
