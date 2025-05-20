
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
public class CharacterState
{
    public bool canMove;
    public bool canJump;
    public void CState(STATE state)
    {
        switch (state)
        {
            case STATE.IDLE:
                return;
            case STATE.MOVE:
                return;
            case STATE.GROUND:
                return;
            case STATE.RUN:
                return;
            case STATE.FALL:
                return;
            case STATE.JUMP:
                return;
        }

    }
}
public class PlayerController : MonoBehaviour
{
    [Header("움직임관련스탯")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpPower;
    private Vector2 curMovementInput;

    private Rigidbody rb;
    private MyAnimation anim;
    private Vector2 inputDirection;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<MyAnimation>();
    }
    private void Start()
    {
        //커서 움직임 막아주는 코드
        Cursor.lockState = CursorLockMode.Locked;

    }
    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
    }

    //InputAction
    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            curMovementInput = context.ReadValue<Vector2>();
            anim.SetMove(true);
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            curMovementInput = Vector2.zero;
            anim.SetMove(false);
        }

    }
    public void Move()
    {
        Vector3 move = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;
        move *= moveSpeed;
        move.y = rb.velocity.y;
        rb.velocity = move;
    }


    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            rb.AddForce(Vector2.up * jumpPower, ForceMode.Impulse);
            anim.TriggerJump();
        }
    }
}
