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
    public bool onRoad = false; // 발판에 있는지 여부 판단
    private Coroutine jumpPowerCoroutine;
    
    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void Start()
    {
        lastYPosition = transform.position.y; // 현재 y위치(땅 위)를 마지막 y위치로 저장
        currentJP = jumpPower; // 설정해둔 점프파워를 현재 점프력으로 저장
    }

    public void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Road")) // 발판 위일 때
        {
            lastYPosition = transform.position.y; // 발판 위의 내 위치 저장
            onRoad = true; // 현재 발판 위라는 뜻
        }
    }

    public void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground")) // 땅에 떨어졌을 때
        {
            if (onRoad) // 만약 발판 위에 있다면
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

                    gameManager.ResetPosition(); // 떨어지면 발판 위치 초기화
                }
            }

            onRoad = false; // 땅으로 떨어진 후 발판이 아니기 때문에 false로 전환
            lastYPosition = transform.position.y; // 현재 y위치(땅 위)를 마지막 y위치로 저장
        }
    }

    public void Heal(float amount) // 아이템으로 회복
    {
        health.Add(amount);
    }

    public void Die() // 구현은 못함
    {
        Debug.Log("죽었");
    }

    public void JumpUp(float amount) // 아이템 사용으로 증가하는 점프력상승 효과
    {
        if (jumpPowerCoroutine != null) // 이전에 점프력 증가 코루틴이 실행 중이었다면
        {
            StopCoroutine(jumpPowerCoroutine); // 기존에 실행 중이던 코루틴을 즉시 중지
        }

        // JumpPowerCoroutine이라는 이름의 코루틴을 실행, jumpPowerCoroutine에 저장
        // JumpUp 함수가 호출될 때 이전에 실행 중이던 코루틴을 정확히 찾아 중지시킬 수 있습
        jumpPowerCoroutine = StartCoroutine(JumpPowerCoroutine(amount));
    }

    private IEnumerator JumpPowerCoroutine(float amount) // 점프력 증가
    {
        currentJP = jumpPower + amount; // 점프력 증가
        Debug.Log($"점프력이 {amount}만큼 증가하여 {currentJP}가 되었습니다. ({Timer}초 동안 지속)");
        
        yield return new WaitForSeconds(Timer); // 지정된 시간 동안 대기

        currentJP = jumpPower; // 효과 종료 후 기본 점프력으로 복귀
        Debug.Log("점프력 증가 효과가 종료되어 기본 점프력으로 돌아왔습니다: " + currentJP);
    }

    public float GetCurrentJumpPower()
    {
        return currentJP; // currentJP 변수에 저장된 값을 호출자에게 돌려줌
    }
}