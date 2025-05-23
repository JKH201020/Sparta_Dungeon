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

    Condition health { get { return uiCondition.health; } } // 이 데이터를 통해 업데이트

    [Header("Jump Power")]
    public float Timer = 30f; // 지속 시간
    public float jumpPower; // 기본 점프 파워
    public float currentJP; // 현재 점프 파워

    float lastYPosition; // 마지막에 밟은 Y축 좌표
    public bool onRoad = false;
    private Coroutine jumpPowerCoroutine;
    
    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        if (gameManager == null)
        {
            Debug.LogError("씬에 GameManager가 없습니다! GameManager 스크립트를 포함하는 오브젝트를 추가해주세요.");
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
            if (onRoad) // 떨어졌으면
            {
                // 떨어진 높이
                float fallDistance = lastYPosition - transform.position.y;

                if (fallDistance >= 2) // 땅과 2 이상의 높이 차이가 날 때
                {
                    health.curValue -= Mathf.Abs(fallDistance) * 5; // 떨어지면 체력 감소
                    Debug.Log($"플레이어가 {fallDistance:F2} 높이에서 떨어졌습니다!");
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
            // 발판 위의 위치 저장
            lastYPosition = transform.position.y;
            onRoad = true;
        }
    }

    public void ResetOnRoadState(float currentY) /////////
    {
        onRoad = false;
        lastYPosition = currentY;
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