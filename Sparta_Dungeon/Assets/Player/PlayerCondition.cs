using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCondition : MonoBehaviour
{
    public UICondition uiCondition;

    Condition health { get { return uiCondition.health; } } // 이 데이터를 통해 업데이트

    // Update is called once per frame
    void Update()
    {
        if (health.curValue > 0f) // 테스트 코드
        {
            health.curValue -= Time.deltaTime;
        }

        if (health.curValue < 0f) // 체력이 0 아래면
        {
            health.curValue = 100f; // 테스트
            //Die();
        }
    }

    public void Die()
    {
        Debug.Log("죽었");
    }
}
