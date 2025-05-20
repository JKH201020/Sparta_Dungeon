using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour // Player�� ���õ� ����� ��Ƶδ� ��
{
    [SerializeField] PlayerController controller;

    private void Awake()
    {
        // �̱���Ŵ����� Player�� ������ �� �ְ� �����͸� �ѱ��.
        CharacterManager.Instance.player = this;
        controller = GetComponent<PlayerController>();   
    }
}
