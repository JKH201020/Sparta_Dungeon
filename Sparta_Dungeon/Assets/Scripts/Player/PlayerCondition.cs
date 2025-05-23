using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCondition : MonoBehaviour
{
    public UICondition uiCondition;
    GameManager gameManager;

    Condition health { get { return uiCondition.health; } } // �� �����͸� ���� ������Ʈ

    [Header("Jump Power")]
    public float Timer = 30f; // ���� �ð�
    public float jumpPower; // �⺻ ���� �Ŀ�
    public float currentJP; // ���� ���� �Ŀ�

    float lastYPosition; // �������� ���� Y�� ��ǥ
    public bool onRoad = false;
    private Coroutine jumpPowerCoroutine;
    
    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        if (gameManager == null)
        {
            Debug.LogError("���� GameManager�� �����ϴ�! GameManager ��ũ��Ʈ�� �����ϴ� ������Ʈ�� �߰����ּ���.");
        }
    }

    private void Start()
    {
        lastYPosition = transform.position.y;
        currentJP = jumpPower;
    }

    public void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            if (onRoad) // ����������
            {
                // ������ ����
                float fallDistance = lastYPosition - transform.position.y;

                if (fallDistance >= 2) // ���� 2 �̻��� ���� ���̰� �� ��
                {
                    health.curValue -= Mathf.Abs(fallDistance) * 5; // �������� ü�� ����
                    Debug.Log($"�÷��̾ {fallDistance:F2} ���̿��� ���������ϴ�!");
                }

                if (gameManager != null)
                {

                    gameManager.ResetPosition();
                }
            }

            onRoad = false;
            lastYPosition = transform.position.y;
        }
    }

    public void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Road"))
        {
            // ���� ���� ��ġ ����
            lastYPosition = transform.position.y;
            onRoad = true;
        }
    }

    public void ResetOnRoadState(float currentY) /////////
    {
        onRoad = false;
        lastYPosition = currentY;
    }

    public void Heal(float amount) // ���������� ȸ��
    {
        health.Add(amount);
    }

    public void Die()
    {
        Debug.Log("�׾�");
    }

    public void JumpUp(float amount)
    {
        if (jumpPowerCoroutine != null)
        {
            StopCoroutine(jumpPowerCoroutine);
        }

        jumpPowerCoroutine = StartCoroutine(JumpPowerCoroutine(amount));
    }

    private IEnumerator JumpPowerCoroutine(float amount) // ������ ����
    {
        // ������ ����
        currentJP = jumpPower + amount;
        Debug.Log($"�������� {amount}��ŭ �����Ͽ� {currentJP}�� �Ǿ����ϴ�. ({Timer}�� ���� ����)");

        // ������ �ð� ���� ���
        yield return new WaitForSeconds(Timer);

        // ȿ�� ���� �� �⺻ ���������� ����
        currentJP = jumpPower;
        Debug.Log("������ ���� ȿ���� ����Ǿ� �⺻ ���������� ���ƿԽ��ϴ�: " + currentJP);
    }

    public float GetCurrentJumpPower()
    {
        return currentJP;
    }
}