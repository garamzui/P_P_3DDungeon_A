using System;
using System.Collections;
using System.Globalization;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
#region MyRegion

/*public enum STATE
{
    IDLE,
    MOVE,
    JUMP,
    GROUND,
    FALL,
    RUN,
    Die

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

#endregion

public class PlayerController : MonoBehaviour
{
    [Header("움직임관련스탯")] [SerializeField] private float moveSpeed;

    [SerializeField] private float jumpPower;

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

    private PlayerInput playerInput;
    private InputAction runAction;

    private PlayerCondition pCondition;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<MyAnimation>();
        pCondition = GetComponent<PlayerCondition>();
        interaction = GetComponent<Interaction>();
        playerInput = GetComponent<PlayerInput>();
        runAction = playerInput.actions["OnRun"];
    }

    private void Start()
    {
        //커서 움직임 막아주는 코드
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isClimb)
        {
            Move();
        }
        else if (isClimb)
        {
            Climb();
        }

        if (rb.velocity.y != 0f && !IsGrounded())
        {
            anim.SetFall(true);
        }
        else
        {
            anim.SetFall(false);
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

    public void OnLook(InputAction.CallbackContext context)
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

    public void Climb()
    {
        Vector3 move = transform.up * curMovementInput.y + transform.right * curMovementInput.x;
        move *= moveSpeed;
        move.z = rb.velocity.z;
        rb.velocity = move;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && IsGrounded() && !isJump &&
            (pCondition.stamina.curValue >= -pCondition.stamina.consumeValue))
        {
            StartCoroutine(Jump());
        }
    }


    bool isJump = false;
    private bool wasRunningbeforeJump = false;
    public IEnumerator Jump()
    {
        isJump = true;


        anim.TriggerJump();
        rb.velocity = Vector3.zero;
        yield return new WaitForSeconds(0.3f);
        if (isRun)
        {
            /*moveSpeed -= 5f;*/
            Debug.Log("달리기 멈춤");
            isRun = false;

            anim.SetRun(isRun);
            wasRunningbeforeJump = true;
        }

        rb.AddForce(Vector2.up * jumpPower, ForceMode.Impulse);
            pCondition.StaminaForJump();
        


        yield return new WaitForSeconds(0.3f);
       
        isJump = false;
        if (wasRunningbeforeJump)
        {   yield return new WaitForSeconds(1.2f);
            moveSpeed -= 5f;
            if (runAction.phase == InputActionPhase.Performed)
            {
                moveSpeed += 5f;
                Debug.Log("달리기 시작");
                isRun = true;

                anim.SetRun(isRun);
              wasRunningbeforeJump = false;
            }
         
            
        }
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

    public void OnQuickSlot(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            switch (context.control.name)
            {
                case "1":
                    PlayerManager.Instance.Player.condition.UseConsumableItem(int.Parse(context.control.name) - 1);
                    break;
                case "2":
                    PlayerManager.Instance.Player.condition.UseConsumableItem(int.Parse(context.control.name) - 1);
                    break;
                case "3":
                    PlayerManager.Instance.Player.condition.UseConsumableItem(int.Parse(context.control.name) - 1);
                    break;
            }
        }
    }


    public void SuperJumpScaffold()
    {
        rb.AddForce(Vector2.up * 15f, ForceMode.Impulse);
    }


    public void GetInstantItem(IEnumerator coroutine)
    {
        StartCoroutine(coroutine);
    }

    public void UseConsumableItem(IEnumerator coroutine)
    {
        StartCoroutine(coroutine);
    }

    public IEnumerator Haste()
    {
        moveSpeed += 5f;
        yield return new WaitForSeconds(5);
        moveSpeed -= 5f;
    }

    public IEnumerator SuperJump()
    {
        jumpPower += 5f;
        yield return new WaitForSeconds(5);
        jumpPower -= 5f;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "JumpScar")
            rb.AddForce(Vector2.up * jumpPower * 2.5f, ForceMode.Impulse);
    }

    bool isAtk = false;

    public void OnAttackmotion(InputAction.CallbackContext context)
    {
        if (IsGrounded() && !isRun)
        {
            if (context.phase == InputActionPhase.Started && !isAtk)
            {
                StartCoroutine(Attack());
            }
        }
    }

    IEnumerator Attack()
    {
        isAtk = true;
        anim.TriggerAttack();
        Debug.Log("공격");
        yield return new WaitForSeconds(0.7f);
        isAtk = false;
    }

    public bool isTPS = false;
    Interaction interaction;

    public void OnChangeView(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && !isTPS && !isTop)
        {
            cameraContainer.transform.localPosition = new Vector3(0, 1.5f, -2.5f);
            isTPS = true;
            interaction.CheckDistanceChange(isTPS);
        }
        else if (context.phase == InputActionPhase.Started && isTPS && !isTop)
        {
            cameraContainer.transform.localPosition = new Vector3(0, 0.63f, 0.2f);
            isTPS = false;
            interaction.CheckDistanceChange(isTPS);
        }
    }

    bool isTop = false;

    public void OnTopView(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && isTPS)
        {
            cameraContainer.transform.localPosition = new Vector3(0, 5f, -2.5f);

            isTop = true;
        }
        else if (context.phase == InputActionPhase.Canceled && isTPS)
        {
            cameraContainer.transform.localPosition = new Vector3(0, 1.5f, -2.5f);
            isTop = false;
        }
    }

    bool isRun = false;

    public void OnRunning(InputAction.CallbackContext context)
    {
        if (IsGrounded())
        {
            if (context.phase == InputActionPhase.Started && !isRun && !isJump)
            {
                moveSpeed += 5f;
                Debug.Log("달리기 시작");
                isRun = true;
                anim.SetRun(isRun);
            }
            else if (context.phase == InputActionPhase.Canceled && isRun)
            {
                if (isJump)
                {
                    return;
                }

                moveSpeed -= 5f;
                Debug.Log("달리기 멈춤");
                isRun = false;
                anim.SetRun(isRun);
            }
        }
    }
    
    public void GoToReSpawn(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started )
        {
            RespawnPlayer.instance.Respawn();
        }
    }

    public GameObject manualBoard;
    public void ManualBoard(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started )
        {
            manualBoard.SetActive(true);
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            manualBoard.SetActive(false);
        }
    }

    private bool isClimb = false;
    public void OnClimb(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && isClimbAble)
        {
           isClimb = true;
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            isClimb = false;
        }
    }

    private bool isClimbAble = false;
    void OnCollisionEnter(Collision other)
    {
        if (this.CompareTag("ClimbCheck"))
        {
            if (other.collider.tag == "Climb")
            {
                Debug.Log("벽에 닿았다.");
                isClimbAble = true;
            }

        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (this.CompareTag("ClimbCheck"))
        {
            if (other.collider.tag == "Climb")
            {
                Debug.Log("벽에서 떨어졌다");
                isClimbAble = false;
            }

        }
    }
}