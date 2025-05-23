using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Enumeration;
using UnityEngine;

public enum ItemType // 아이템 타입
{
    Consumable // 소모품
}

public enum ConsumableType // 소모품 타입
{
    Health, //체력 회복
    Jump // 점프
}

[Serializable]
public class ItemDataConsumable
{
    public ConsumableType type;
    public float value; // 수치
}

// project -> (마우스 우클릭)create를 누르면 맨처음에 NewItem이라고 생성된다.
[CreateAssetMenu(fileName = "Item", menuName = "New Item")]

public class ItemData : ScriptableObject
{
    [Header("Info")]
    public string displayName; // 아이템 이름
    public string description; // 아이템 설명
    public ItemType type;
    public Sprite icon;
    public GameObject dropPrefab;

    [Header("Stacking")]
    public bool canStack; // 중복으로 가질 수 있는가
    public int maxStackAmount; // 최대 소지 수

    [Header("Consumable")]
    public ItemDataConsumable[] consumables; // 소지중인 아이템 배열
}