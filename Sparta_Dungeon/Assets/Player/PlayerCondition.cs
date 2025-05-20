using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCondition : MonoBehaviour
{
    public UICondition uiCondition;

    Condition health { get { return uiCondition.health; } } // �� �����͸� ���� ������Ʈ

    // Update is called once per frame
    void Update()
    {
        if (health.curValue > 0f) // �׽�Ʈ �ڵ�
        {
            health.curValue -= Time.deltaTime;
        }

        if (health.curValue < 0f) // ü���� 0 �Ʒ���
        {
            health.curValue = 100f; // �׽�Ʈ
            //Die();
        }
    }

    public void Die()
    {
        Debug.Log("�׾�");
    }
}
