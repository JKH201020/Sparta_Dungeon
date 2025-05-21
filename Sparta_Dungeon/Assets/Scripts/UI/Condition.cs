using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Condition : MonoBehaviour
{
    public float curValue; // 현재치
    public float maxValue; // 최대치
    public float startValue; // 시작치

    //public float passiveValue; 
    public Image uiBar;

    // Start is called before the first frame update
    void Start()
    {
        // 시작할 때 현재 체력을 시작 체력으로 지정 - 체력 기준
        curValue = startValue;
    }

    // Update is called once per frame
    void Update()
    {
        // Image컴포넌트의 fillAmount와 GetPercentage를 연결
        uiBar.fillAmount = GetPercentage();
    }

    public void Add(float amount) // 아이템 효과로 증가
    {
        // curValue + amount 와 maxValue 중 더 작은 값을 현재 값으로 설정
        curValue = Mathf.Min(curValue + amount, maxValue);
    }

    public void Subtract(float amount) // 아이템 효과로 감소
    {
        // curValue - amount 와 0 중 더 큰 값을 현재 값으로 설정
        curValue = Mathf.Max(curValue - amount, 0.0f);
    }

    public float GetPercentage()
    {
        // 현재 체력을 게이지로 표현
        return curValue / maxValue;
    }
}