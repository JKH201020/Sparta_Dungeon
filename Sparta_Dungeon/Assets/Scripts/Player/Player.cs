using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour // Player�� ���õ� ����� ��Ƶδ� ��
{
    public PlayerController controller; // �÷��̼� ������
    public PlayerCondition condition; // �÷��̾� ����
    public Equipment equip; // ��� ����
    
    // ���� ��ȣ�ۿ��ϴ� �������� �����͸� �Ѱ��ְ�
    public ItemData itemData; // �� ��ũ��Ʈ�� ������ ������ �Ѱ���
    // addItem�̶�� ��������Ʈ�� ���� ������ �Ǿ������� ������� �ְԲ� ����
    public Action addItem;

    public Transform dropPosition; // ������ ���(����) ��ġ

    private void Awake()
    {
        // �̱���Ŵ����� Player�� ������ �� �ְ� �����͸� �ѱ��.
        CharacterManager.Instance.Player = this;

        controller = GetComponent<PlayerController>();
        condition = GetComponent<PlayerCondition>();
        equip = GetComponent<Equipment>();
    }
}
