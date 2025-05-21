using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

/*public enum STATE
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
}*/
public class PlayerController : MonoBehaviour
{
    [Header("움직임관련스탯")] 
    [SerializeField] private float moveSpeed;
    public float MoveSpeed {get { return moveSpeed; }}
    [SerializeField] private float jumpPower;
    public float JumpPower {get {return jumpPower;}}
    private Vector2 curMovementInput;
    public LayerMask groundLayerMask;


    [Header("Look")] public Transform cameraContainer;
    public float minXLook;
    public float maxXLook;
    private float camCurXRot;


    public float LookSensitivity;
    private Vector2 mouseDelta;

    public Transform body;

    private Rigidbody rb;
    private MyAnimation anim;
    private Vector2 inputDirection;


    private PlayerCondition pCondition;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<MyAnimation>();
        pCondition = GetComponent<PlayerCondition>();
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
        if (rb.velocity.y != 0f)
        {
            anim.SetFall(!IsGrounded());
        }
        else
        {
            return;
        }
    }

    private void LateUpdate()
    {
        CameraLook();
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

    void CameraLook()
    {
        camCurXRot += mouseDelta.y * LookSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);

        transform.eulerAngles += new Vector3(0, mouseDelta.x * LookSensitivity, 0);

        body.eulerAngles += new Vector3(-mouseDelta.y * LookSensitivity, 0, 0);
    }

    public void OnLoook(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
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
        if (context.phase == InputActionPhase.Started && IsGrounded() && !stopJump && (pCondition.stamina.curValue >= -pCondition.stamina.consumeValue)) 
        {
            StartCoroutine(Jump());
        }
    }

    bool stopJump = false;

    public IEnumerator Jump()
    {
        stopJump = true;
        anim.TriggerJump();
        rb.velocity = Vector3.zero;
        yield return new WaitForSeconds(0.3f);
        pCondition.StaminaForJump();
        anim.SetFall(true);
        rb.AddForce(Vector2.up * jumpPower, ForceMode.Impulse);
        yield return new WaitForSeconds(0.3f);

        stopJump = false;
    }

    //오브젝트의 저런 방향, 위치를 계산하는 방법을 연습해야됨
    //3D오브젝트의 점프체크할때 많이 쓰는 방법 4개의 의자다리처럼 레이쏘기
    bool IsGrounded()
    {
        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.05f), Vector3.down),
            new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.05f), Vector3.down),
            new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.05f), Vector3.down),
            new Ray(transform.position + (-transform.right * 0.2f) + (transform.up * 0.05f), Vector3.down)
        };
        for (int i = 0; i < rays.Length; i++)
        {
            if (Physics.Raycast(rays[i], 0.1f, groundLayerMask))
            {
                return true;
            }
        }

        return false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.05f), Vector3.down),
            new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.05f), Vector3.down),
            new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.05f), Vector3.down),
            new Ray(transform.position + (-transform.right * 0.2f) + (transform.up * 0.05f), Vector3.down)
        };

        for (int i = 0; i < rays.Length; i++)
        {
            Gizmos.DrawLine(rays[i].origin, rays[i].origin + rays[i].direction * 0.1f);
        }
    }

    public void HasteMoveSpeed()
    {
        StartCoroutine(Haste());
    }

    public IEnumerator Haste()
    {
        moveSpeed += 5f;
        yield return new WaitForSeconds(5);
        moveSpeed -= 5f;
    }

    public void SuperJump()
    {
        rb.AddForce(Vector2.up * 15f, ForceMode.Impulse);
    }
    
    
    public void GetInstantItem(float value)
    {
        StartCoroutine(InstantEffect(value));
    }

    IEnumerator InstantEffect(float value)
    {
        float defaultvalue = value;
        value *= 2;
        yield return new WaitForSeconds(10);
        value = defaultvalue;
    }
   
}