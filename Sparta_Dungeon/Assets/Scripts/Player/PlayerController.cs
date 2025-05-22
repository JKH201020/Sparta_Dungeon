using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    private Vector2 curMovementInput; // 현재 입력 값
    public LayerMask groundLayerMask; // 레이어 정보

    [Header("Look")]
    public Transform cameraContainer;
    public float minXLook; // 최소 시야각
    public float maxXLook; // 최대 시야각
    private float camCurXRot;
    public float lookSensitivity; // 카메라 민감도

    private Vector2 mouseDelta; // 마우스 변화값

    [HideInInspector]
    public bool canLook = true;

    private Rigidbody rigidbody;
    public UIInventory uiInven;
    public PlayerCondition condition;

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

    public void OnMove(InputAction.CallbackContext context) // 이동버튼 누를 때(wasd)
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

    public void OnJump(InputAction.CallbackContext context) // 점프(스페이스)
    {
        if (context.phase == InputActionPhase.Started && IsGrounded())
        {
            rigidbody.AddForce(Vector2.up * condition.GetCurrentJumpPower(), ForceMode.Impulse);
        }
    }

    public void OnUse(InputAction.CallbackContext context) // 아이템 사용(마우스 좌클릭)
    {
        if (context.phase == InputActionPhase.Started)
        {
            if (uiInven != null && uiInven.selectedItem != null) // 인벤토리와 선택한 아이템이 존재할 때
            {
                for (int i = 0; i < uiInven.selectedItem.consumables.Length; i++)
                {
                    switch (uiInven.selectedItem.consumables[i].type) // 소모 아이템 타입에 따른 조건문
                    {
                        case ConsumableType.Jump:
                            // 점프력 증가
                            condition.JumpUp(uiInven.selectedItem.consumables[i].value);
                            break;
                        case ConsumableType.Health:
                            // 체력 회복
                            condition.Heal(uiInven.selectedItem.consumables[i].value);
                            break;
                    }
                }

                uiInven.RemoveSelectedItem(); // 사용 시 아이템 제거
            }
        }
    }

    public void OnSelectItem(InputAction.CallbackContext context) // 인벤토리 슬롯 스위치(탭)
    {
        if (context.phase == InputActionPhase.Started)
        {
            // (uiInventory.selectedItemIndex + 1)이 uiInven.slots.Length와 같아질 때 다시 0으로 초기화 시킴
            int Index = (uiInven.selectedIndex + 1) % uiInven.slots.Length;

            uiInven.SelectedSlot(Index);
            Debug.Log("현재 선택된 슬롯 인덱스: " + uiInven.selectedIndex);
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
            // 땅과의 거리가 0.1f이하일 경우
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