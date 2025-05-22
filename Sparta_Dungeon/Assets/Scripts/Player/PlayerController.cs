using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    private Vector2 curMovementInput; // ���� �Է� ��
    public LayerMask groundLayerMask; // ���̾� ����

    [Header("Look")]
    public Transform cameraContainer;
    public float minXLook; // �ּ� �þ߰�
    public float maxXLook; // �ִ� �þ߰�
    private float camCurXRot;
    public float lookSensitivity; // ī�޶� �ΰ���

    private Vector2 mouseDelta; // ���콺 ��ȭ��

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
        Cursor.lockState = CursorLockMode.Locked; // ���콺 Ŀ�� �� ���̰� ���� ȭ�鿡�� ���
    }

    private void FixedUpdate() // ���� ����
    {
        Move();
    }

    private void LateUpdate() // ī�޶� ���� -> ��� ������ ������ ī�޶� ������
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

    public void OnMove(InputAction.CallbackContext context) // �̵���ư ���� ��(wasd)
    {
        // wasd ������ ��
        if (context.phase == InputActionPhase.Performed)
        {
            curMovementInput = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            curMovementInput = Vector2.zero;
        }
    }

    public void OnJump(InputAction.CallbackContext context) // ����(�����̽�)
    {
        if (context.phase == InputActionPhase.Started && IsGrounded())
        {
            rigidbody.AddForce(Vector2.up * condition.GetCurrentJumpPower(), ForceMode.Impulse);
        }
    }

    public void OnUse(InputAction.CallbackContext context) // ������ ���(���콺 ��Ŭ��)
    {
        if (context.phase == InputActionPhase.Started)
        {
            if (uiInven != null && uiInven.selectedItem != null) // �κ��丮�� ������ �������� ������ ��
            {
                for (int i = 0; i < uiInven.selectedItem.consumables.Length; i++)
                {
                    switch (uiInven.selectedItem.consumables[i].type) // �Ҹ� ������ Ÿ�Կ� ���� ���ǹ�
                    {
                        case ConsumableType.Jump:
                            // ������ ����
                            condition.JumpUp(uiInven.selectedItem.consumables[i].value);
                            break;
                        case ConsumableType.Health:
                            // ü�� ȸ��
                            condition.Heal(uiInven.selectedItem.consumables[i].value);
                            break;
                    }
                }

                uiInven.RemoveSelectedItem(); // ��� �� ������ ����
            }
        }
    }

    public void OnSelectItem(InputAction.CallbackContext context) // �κ��丮 ���� ����ġ(��)
    {
        if (context.phase == InputActionPhase.Started)
        {
            // (uiInventory.selectedItemIndex + 1)�� uiInven.slots.Length�� ������ �� �ٽ� 0���� �ʱ�ȭ ��Ŵ
            int Index = (uiInven.selectedIndex + 1) % uiInven.slots.Length;

            uiInven.SelectedSlot(Index);
            Debug.Log("���� ���õ� ���� �ε���: " + uiInven.selectedIndex);
        }
    }

    void CameraLook()
    {
        // ���콺 �������� ��ȭ��(mouseDelta)�� y(�� �Ʒ�)���� �ΰ����� ���Ѵ�.
        // ī�޶� �� �Ʒ��� ȸ���Ϸ��� rotation�� x ���� �־��ش�.
        camCurXRot += mouseDelta.y * lookSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);

        // ī�޶� �¿�� ȸ���Ϸ��� rotation�� y ���� �־��ش�. -> ȸ���� Y�� �������� ȸ���Ѵ�.
        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
    }

    private void Move()
    {
        // ���� �Է��� y ���� z ��(forward, �յ�)�� ���Ѵ�. -> ������ 3D���� �������� 2D�� �����̱� ������
        // ���� �Է��� x ���� x ��(right, �¿�)�� ���Ѵ�.
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;
        dir *= moveSpeed; // ���⿡ �ӷ��� �����ش�.
        dir.y = rigidbody.velocity.y; // y���� velocity(��ȭ��)�� y ���� �־��ش�.

        rigidbody.velocity = dir; // ����� �ӵ��� velocity(��ȭ��)�� �־��ش�.
    }

    bool IsGrounded()
    {
        // �÷��̾ ���� ��� �ִ��� �����ϱ� ���� Ray - �������� ����
        // �÷��̾�(transform)�� �������� �յ��¿� 0.2�� ����߷��� 0.01 ���� ��¦ ���� �ø���.
        Ray[] rays = new Ray[4]
        {
           new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
           new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
           new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),
           new Ray(transform.position + (-transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down)
        };

        for (int i = 0; i < rays.Length; i++)
        {
            // ������ �Ÿ��� 0.1f������ ���
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