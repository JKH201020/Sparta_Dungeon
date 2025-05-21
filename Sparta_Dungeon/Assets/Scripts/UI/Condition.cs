using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Condition : MonoBehaviour
{
    public float curValue; // ����ġ
    public float maxValue; // �ִ�ġ
    public float startValue; // ����ġ

    //public float passiveValue; 
    public Image uiBar;

    // Start is called before the first frame update
    void Start()
    {
        // ������ �� ���� ü���� ���� ü������ ���� - ü�� ����
        curValue = startValue;
    }

    // Update is called once per frame
    void Update()
    {
        // Image������Ʈ�� fillAmount�� GetPercentage�� ����
        uiBar.fillAmount = GetPercentage();
    }

    public void Add(float amount) // ������ ȿ���� ����
    {
        // curValue + amount �� maxValue �� �� ���� ���� ���� ������ ����
        curValue = Mathf.Min(curValue + amount, maxValue);
    }

    public void Subtract(float amount) // ������ ȿ���� ����
    {
        // curValue - amount �� 0 �� �� ū ���� ���� ������ ����
        curValue = Mathf.Max(curValue - amount, 0.0f);
    }

    public float GetPercentage()
    {
        // ���� ü���� �������� ǥ��
        return curValue / maxValue;
    }
}