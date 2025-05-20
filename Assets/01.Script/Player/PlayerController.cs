using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public enum STATE
{
    IDLE,
    MOVE,
    JUMP,
    GROUND,
    FALL,
    RUN

}
public class  CharacterState
{
    
}
public class PlayerController : MonoBehaviour
{
    [Header("움직임관련스탯")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpPower;
    

   private Rigidbody rb;
    private MyAnimation anim; 
   private Vector2 inputDirection;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnMove(InputAction.CallbackContext context)
    {
        inputDirection = context.ReadValue<Vector2>();



    }
}
