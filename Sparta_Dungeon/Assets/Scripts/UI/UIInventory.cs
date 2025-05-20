using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEditor;
using UnityEngine;

public class UIInventory : MonoBehaviour
{
    public ItemSlot[] slots; // 아이템 슬롯 배열

    public Transform slotPanel; // 슬롯 판넬 오브젝트
    public Transform dropPosition;

    private PlayerController controller;
    private PlayerCondition condition;

    ItemData selectedItem;
    int selectedItemIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        controller = CharacterManager.Instance.Player.controller;
        condition = CharacterManager.Instance.Player.condition;
        dropPosition = CharacterManager.Instance.Player.dropPosition;

        CharacterManager.Instance.Player.addItem += AddItem;

        // 아이템 슬롯 칸을 slotPanel 오브젝트의 자식의 개수를 불러옴
        slots = new ItemSlot[slotPanel.childCount];

        for (int i = 0; i < slots.Length; i++)
        {
            // 슬롯판넬의 자식오브젝트 아이템 슬롯 컴포넌트를 가져와 저장
            slots[i] = slotPanel.GetChild(i).GetComponent<ItemSlot>();
            slots[i].index = i;
            slots[i].inventory = this;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void AddItem()
    {
        ItemData data = CharacterManager.Instance.Player.itemData;

        // 중복 가능한지 - 혹시나 해서 만들음
        if (data.canStack)
        {
            ItemSlot slot = GetItemStack(data);
            // 비어있는 슬롯 가져옴
            if (slot != null)
            {
                slot.quantity++;
                UpdateUI();
                CharacterManager.Instance.Player.itemData = null;
                return;
            }
        }

        // 비어 있는 슬롯 가져옴
        ItemSlot emptySlot = GetEmptySlot();

        // 플레이어에 있는 아이템 데이터를 비움
        CharacterManager.Instance.Player.itemData = null;

        if (emptySlot != null)
        {
            emptySlot.item = data;
            emptySlot.quantity = 1;
            UpdateUI();
            CharacterManager.Instance.Player.itemData = null;
            return;
        }

        //없다면 아이템 버림
        ThrowItem(data);
        CharacterManager.Instance.Player.itemData = null;
    }

    void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item != null)
            {
                // 해당 슬롯에 템 있을 때
                slots[i].Set(); // 아이템 세팅
            }
            else
            {
                // 해당 슬롯 비움
                slots[i].Clear();
            }
        }
    }

    ItemSlot GetItemStack(ItemData data) // 아이템 개수 추가
    {
        for (int i = 0; i < slots.Length; i++)
        {
            // 최대 수보다 적다면 아이템 개수에 추가
            if (slots[i].item == data && slots[i].quantity < data.maxStackAmount)
            {
                return slots[i];
            }
        }

        return null;
    }

    ItemSlot GetEmptySlot() // 아이템 슬롯이 이어 있을 경우
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                return slots[i];
            }
        }

        return null;
    }

    void ThrowItem(ItemData data)
    {
        // 내 주위에 랜덤으로 떨어짐
        Instantiate(data.dropPrefab, dropPosition.position, Quaternion.Euler(Vector3.one * Random.value * 360));
    }

    void SelectItem(int index)
    {
        if (slots[index].item == null) return;
    }
}