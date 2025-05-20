using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float moveSpeed;
    private Vector2 curMovementInput; // 현재 입력 값
    [SerializeField] float jumpPower;
    [SerializeField] LayerMask groundLayerMask; // 레이어 정보

    [Header("Look")]
    [SerializeField] Transform cameraContainer;
    [SerializeField] float minXLook; // 최소 시야각
    [SerializeField] float maxXLook; // 최대 시야각
    private float camCurXRot;
    [SerializeField] float lookSensitivity; // 카메라 민감도

    private Vector2 mouseDelta; // 마우스 변화값

    [HideInInspector]
    public bool canLook = true;

    private Rigidbody rigidbody;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // 마우스 커서 안 보이게 게임 화면에서 잠금
    }

    private void FixedUpdate() // 물리 연산
    {
        Move();
    }

    private void LateUpdate() // 카메라 연산 -> 모든 연산이 끝나고 카메라 움직임
    {
        if (canLook)
        {
            CameraLook();
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    public void OnMove(InputAction.CallbackContext context) // 이동버튼 누를 때
    {
        // wasd 눌렀을 때
        if (context.phase == InputActionPhase.Performed)
        {
            curMovementInput = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            curMovementInput = Vector2.zero;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && IsGrounded())
        {
            rigidbody.AddForce(Vector2.up * jumpPower, ForceMode.Impulse);
        }
    }

    void CameraLook()
    {
        // 마우스 움직임의 변화량(mouseDelta)중 y(위 아래)값에 민감도를 곱한다.
        // 카메라가 위 아래로 회전하려면 rotation의 x 값에 넣어준다.
        camCurXRot += mouseDelta.y * lookSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);

        // 카메라가 좌우로 회전하려면 rotation의 y 값에 넣어준다. -> 회전은 Y축 기준으로 회전한다.
        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
    }

    private void Move()
    {
        // 현재 입력의 y 값은 z 축(forward, 앞뒤)에 곱한다. -> 실제는 3D지만 움직임은 2D로 움직이기 때문에
        // 현재 입력의 x 값은 x 축(right, 좌우)에 곱한다.
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;
        dir *= moveSpeed; // 방향에 속력을 곱해준다.
        dir.y = rigidbody.velocity.y; // y값은 velocity(변화량)의 y 값을 넣어준다.

        rigidbody.velocity = dir; // 연산된 속도를 velocity(변화량)에 넣어준다.
    }

    bool IsGrounded()
    {
        // 플레이어가 땅을 밟고 있는지 감지하기 위한 Ray - 무한점프 방지
        // 플레이어(transform)을 기준으로 앞뒤좌우 0.2씩 떨어뜨려서 0.01 정도 살짝 위로 올린다.
        Ray[] rays = new Ray[4]
        {
           new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
           new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
           new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),
           new Ray(transform.position + (-transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down)
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

    public void ToggleCursor(bool toggle)
    {
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;
        canLook = !toggle;
    }
}