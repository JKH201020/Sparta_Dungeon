using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICondition : MonoBehaviour
{
    // ������ ��� ������Ʈ ��ų ����
    public Condition health; // ü�� UI�� ������ Condition ������Ʈ ����
    public Condition jump; // ������ UI�� ������ Condition ������Ʈ ����

    // Start is called before the first frame update
    void Start()
    {
        // PlayerCondition ��ũ��Ʈ���� �� UICondition ��ũ��Ʈ�� ������ �Ѱ���
        CharacterManager.Instance.Player.condition.uiCondition = this;
    }
}