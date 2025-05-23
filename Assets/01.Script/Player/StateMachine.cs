/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
    void OnEnter();
    void OnExit();
    void OnUpdate();
}

public class StateMachine 
{ 
    private IState currentState;

    public void ChangeState(IState newState)
    {
        if (currentState == newState) return;

        currentState?.OnExit();
        currentState = newState;
        currentState.OnEnter();
    }

    public void Update()
    {
        currentState?.OnUpdate();
    }
}

public class IdleState : IState
{
    private PlayerController player;

    public IdleState(PlayerController player)
    {
        this.player = player;
    }

    public void OnEnter()
    {
        player.anim.Play("Idle");
    }

    public void OnUpdate()
    {
        if (player.IsMoveInput())
        {
            player.ChangeState(player.moveState);
        }
        else if (player.IsJumpPressed())
        {
            player.ChangeState(player.jumpState);
        }
    }

    public void OnExit()
    {
        // 나갈 때 해야 할 일
    }
}*/