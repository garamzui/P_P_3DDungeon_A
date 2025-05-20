
using System.Collections;
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
    [Header("�����Ӱ��ý���")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpPower;
    private Vector2 curMovementInput;
    public LayerMask groundLayerMask;



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
        //Ŀ�� ������ �����ִ� �ڵ�
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
        if (context.phase == InputActionPhase.Started&&IsGrounded())
        {
            StartCoroutine(Jump());
        }
    }

    public IEnumerator Jump()
    {
       
        anim.TriggerJump();
        yield return new WaitForSeconds(0.3f); 
          rb.AddForce(Vector2.up * jumpPower, ForceMode.Impulse);
    }
    //������Ʈ�� ���� ����, ��ġ�� ����ϴ� ����� �����ؾߵ�
    //3D������Ʈ�� ����üũ�Ҷ� ���� ���� ��� 4���� ���ڴٸ�ó�� ���̽��
    bool IsGrounded()
    {
        Ray[] rays = new Ray[4]
            {
                new Ray(transform.position+(transform.forward*0.2f)+(transform.up*0.01f), Vector3.down),
                new Ray(transform.position+(-transform.forward*0.2f)+(transform.up*0.01f), Vector3.down),
                new Ray(transform.position+(transform.right*0.2f)+(transform.up*0.01f), Vector3.down),
                new Ray(transform.position+(-transform.right*0.2f)+(transform.up*0.01f), Vector3.down)
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
        new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
        new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
        new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),
        new Ray(transform.position + (-transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down)
        };

        for (int i = 0; i < rays.Length; i++)
        {
            Gizmos.DrawLine(rays[i].origin, rays[i].origin + rays[i].direction * 0.1f);
        }
    }
}
