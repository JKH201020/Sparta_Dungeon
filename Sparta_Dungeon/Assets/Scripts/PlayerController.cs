using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float moveSpeed;
    private Vector2 curMovementInput; // ���� �Է� ��
    [SerializeField] float jumpPower;
    [SerializeField] LayerMask groundLayerMask; // ���̾� ����

    [Header("Look")]
    [SerializeField] Transform cameraContainer;
    [SerializeField] float minXLook; // �ּ� �þ߰�
    [SerializeField] float maxXLook; // �ִ� �þ߰�
    private float camCurXRot;
    [SerializeField] float lookSensitivity; // ī�޶� �ΰ���

    private Vector2 mouseDelta; // ���콺 ��ȭ��

    [HideInInspector]
    public bool canLook = true;

    private Rigidbody rigidbody;

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

    public void OnMove(InputAction.CallbackContext context) // �̵���ư ���� ��
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

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && IsGrounded())
        {
            rigidbody.AddForce(Vector2.up * jumpPower, ForceMode.Impulse);
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