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
    public bool onRoad = false; // ���ǿ� �ִ��� ���� �Ǵ�
    private Coroutine jumpPowerCoroutine;
    
    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void Start()
    {
        lastYPosition = transform.position.y; // ���� y��ġ(�� ��)�� ������ y��ġ�� ����
        currentJP = jumpPower; // �����ص� �����Ŀ��� ���� ���������� ����
    }

    public void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Road")) // ���� ���� ��
        {
            lastYPosition = transform.position.y; // ���� ���� �� ��ġ ����
            onRoad = true; // ���� ���� ����� ��
        }
    }

    public void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground")) // ���� �������� ��
        {
            if (onRoad) // ���� ���� ���� �ִٸ�
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

                    gameManager.ResetPosition(); // �������� ���� ��ġ �ʱ�ȭ
                }
            }

            onRoad = false; // ������ ������ �� ������ �ƴϱ� ������ false�� ��ȯ
            lastYPosition = transform.position.y; // ���� y��ġ(�� ��)�� ������ y��ġ�� ����
        }
    }

    public void Heal(float amount) // ���������� ȸ��
    {
        health.Add(amount);
    }

    public void Die() // ������ ����
    {
        Debug.Log("�׾�");
    }

    public void JumpUp(float amount) // ������ ������� �����ϴ� �����»�� ȿ��
    {
        if (jumpPowerCoroutine != null) // ������ ������ ���� �ڷ�ƾ�� ���� ���̾��ٸ�
        {
            StopCoroutine(jumpPowerCoroutine); // ������ ���� ���̴� �ڷ�ƾ�� ��� ����
        }

        // JumpPowerCoroutine�̶�� �̸��� �ڷ�ƾ�� ����, jumpPowerCoroutine�� ����
        // JumpUp �Լ��� ȣ��� �� ������ ���� ���̴� �ڷ�ƾ�� ��Ȯ�� ã�� ������ų �� �ֽ�
        jumpPowerCoroutine = StartCoroutine(JumpPowerCoroutine(amount));
    }

    private IEnumerator JumpPowerCoroutine(float amount) // ������ ����
    {
        currentJP = jumpPower + amount; // ������ ����
        Debug.Log($"�������� {amount}��ŭ �����Ͽ� {currentJP}�� �Ǿ����ϴ�. ({Timer}�� ���� ����)");
        
        yield return new WaitForSeconds(Timer); // ������ �ð� ���� ���

        currentJP = jumpPower; // ȿ�� ���� �� �⺻ ���������� ����
        Debug.Log("������ ���� ȿ���� ����Ǿ� �⺻ ���������� ���ƿԽ��ϴ�: " + currentJP);
    }

    public float GetCurrentJumpPower()
    {
        return currentJP; // currentJP ������ ����� ���� ȣ���ڿ��� ������
    }
}