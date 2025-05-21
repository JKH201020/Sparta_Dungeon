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

    // Update is called once per frame
    void Update()
    {
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
}
