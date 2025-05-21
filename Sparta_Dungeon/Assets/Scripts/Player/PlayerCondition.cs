using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCondition : MonoBehaviour
{
    public UICondition uiCondition;

    Condition health { get { return uiCondition.health; } } // 이 데이터를 통해 업데이트

    // Update is called once per frame
    void Update()
    {
        if (health.curValue < 0f) // 체력이 0 아래면
        {
            //Die();
        }
    }

    public void Heal(float amount) // 아이템으로 회복
    {
        health.Add(amount);
    }

    public void Die()
    {
        Debug.Log("죽었");
    }
}
