using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Enumeration;
using UnityEngine;

public enum ItemType // ������ Ÿ��
{
    Consumable // �Ҹ�ǰ
}

public enum ConsumableType // �Ҹ�ǰ Ÿ��
{
    Health, //ü�� ȸ��
    Stat // ���� ����
}

[Serializable]
public class ItemDataConsumable
{
    public ConsumableType type;
    public float value; // ��ġ
}

// project -> (���콺 ��Ŭ��)create�� ������ ��ó���� NewItem�̶�� �����ȴ�.
[CreateAssetMenu(fileName = "Item", menuName = "New Item")]

public class ItemData : ScriptableObject
{
    [Header("Info")]
    public string displayName; // ������ �̸�
    public string description; // ������ ����
    public ItemType type;
    public Sprite icon;
    public GameObject dropPrefab;

    [Header("Stacking")]
    public bool canStack;
    public int maxStackAmount;

    [Header("Consumable")]
    public ItemDataConsumable[] consumables;

    [Header("Item")]
    public GameObject equipPrefab;
}