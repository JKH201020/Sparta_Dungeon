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

    [Header("Jump Power")]
    public float Timer = 60f; // 지속 시간
    public float jumpPower; // 기본 점프 파워
    public float currentJP; // 현재 점프 파워
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

    public void JumpUp(float amount)
    {
        if (jumpPowerCoroutine != null)
        {
            StopCoroutine(jumpPowerCoroutine);
        }

        jumpPowerCoroutine = StartCoroutine(JumpPowerCoroutine(amount));
    }

    private IEnumerator JumpPowerCoroutine(float amount) // 점프력 증가
    {
        // 점프력 증가
        currentJP = jumpPower + amount;
        Debug.Log($"점프력이 {amount}만큼 증가하여 {currentJP}가 되었습니다. ({Timer}초 동안 지속)");

        // 지정된 시간 동안 대기
        yield return new WaitForSeconds(Timer);

        // 효과 종료 후 기본 점프력으로 복귀
        currentJP = jumpPower;
        Debug.Log("점프력 증가 효과가 종료되어 기본 점프력으로 돌아왔습니다: " + currentJP);
    }

    public float GetCurrentJumpPower()
    {
        return currentJP;
    }
}