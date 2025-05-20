using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour // Player�� ���õ� ����� ��Ƶδ� ��
{
    public PlayerController controller;
    public PlayerCondition condition;

    private void Awake()
    {
        // �̱���Ŵ����� Player�� ������ �� �ְ� �����͸� �ѱ��.
        CharacterManager.Instance.Player = this;
        controller = GetComponent<PlayerController>();
        condition = GetComponent<PlayerCondition>();
    }
}
