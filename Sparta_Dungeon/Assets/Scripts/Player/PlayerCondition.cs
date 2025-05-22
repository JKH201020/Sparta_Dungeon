using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCondition : MonoBehaviour
{
    public UICondition uiCondition;

    Condition health { get { return uiCondition.health; } } // �� �����͸� ���� ������Ʈ

    [Header("Jump Power")]
    public float Timer = 60f; // ���� �ð�
    public float jumpPower; // �⺻ ���� �Ŀ�
    public float currentJP; // ���� ���� �Ŀ�
    private Coroutine jumpPowerCoroutine;

    private void Awake()
    {
        currentJP = jumpPower;
    }

    // Update is called once per frame
    void Update()
    {
        if (health.curValue >0f) 
        {
            health.curValue -= Time.deltaTime * 2;
        }
        if (health.curValue < 0f) // ü���� 0 �Ʒ���
        {
            //Die();
        }
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