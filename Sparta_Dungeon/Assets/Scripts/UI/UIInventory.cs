using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInventory : MonoBehaviour
{
    public ItemSlot[] slots; // 아이템 슬롯 배열

    public Transform slotPanel; // 슬롯 판넬 오브젝트
    public Transform dropPosition;

    private PlayerController controller;
    private PlayerCondition condition;

    public ItemData selectedItem;
    public int selectedIndex = 0; // 현재 선택된 슬롯의 인덱스 (아웃라인 표시용)

    //int curEquipIndex; // 현재 장착된 아이템의 슬롯 인덱스 (Equip 로직에서 사용)

    private void Awake()
    {
        if (CharacterManager.Instance != null && CharacterManager.Instance.Player != null)
        {
            controller = CharacterManager.Instance.Player.controller;
            condition = CharacterManager.Instance.Player.condition;
            dropPosition = CharacterManager.Instance.Player.dropPosition;

            // CharacterManager의 AddItem 이벤트 구독
            CharacterManager.Instance.Player.addItem += AddItem;
        }

        if (slots != null)
        {
            // 아이템 슬롯 칸을 slotPanel 오브젝트의 자식의 개수를 불러옴
            slots = new ItemSlot[slotPanel.childCount];

            for (int i = 0; i < slots.Length; i++)
            {
                Transform child = slotPanel.GetChild(i);
                if (child != null)
                {
                    // 슬롯판넬의 자식오브젝트 아이템 슬롯 컴포넌트를 가져와 저장
                    slots[i] = child.GetComponent<ItemSlot>();

                    if (slots[i] != null)
                    {
                        slots[i].index = i;
                        slots[i].inventory = this;

                        // 모든 슬롯의 아웃라인을 기본적으로 비활성화
                        slots[i].SetOutlineActive(false);
                    }
                    else
                    {
                        slots[i] = null; // null이 들어가지 않도록 명시적으로 null 할당
                    }
                }
            }
        }
        else
        {
            slots = new ItemSlot[0]; // NullReferenceException 방지를 위해 빈 배열로 초기화
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (slots.Length > 0)
        {
            // 게임 시작 시 0번 슬롯 (첫 번째 슬롯) 선택 및 아웃라인 활성화
            SelectedSlot(0);
        }
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

    ItemSlot GetEmptySlot() // 아이템 슬롯이 비어 있을 경우
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

    void ThrowItem(ItemData data) // 일단 코드만 작성
    {
        // 버린 아이템이 내 주위에 랜덤으로 떨어짐
        Instantiate(data.dropPrefab, dropPosition.position, Quaternion.Euler(Vector3.one * Random.value * 360));
    }

    public void SelectedSlot(int index)
    {
        // 이전에 슬롯의 아웃라인을 비활성화
        if (selectedIndex >= 0 && selectedIndex < slots.Length && slots[selectedIndex] != null)
        {
            slots[selectedIndex].SetOutlineActive(false);
        }

        selectedIndex = index;

        if (selectedIndex >= 0 && selectedIndex < slots.Length && slots[selectedIndex] != null)
        {
            slots[selectedIndex].SetOutlineActive(true);
            selectedItem = slots[selectedIndex].item; // 선택된 아이템 정보도 업데이트
        }
        else
        {
            selectedItem = null; // 유효하지 않은 인덱스인 경우 선택된 아이템 초기화
        }
    }

    public void RemoveSelectedItem() // 아이템 버리거나 사용했을 때
    {
        slots[selectedIndex].quantity--; // 아이템 수량 감소

        if (slots[selectedIndex].quantity <= 0)
        {
            selectedItem = null;
            slots[selectedIndex].item = null;
            //selectedIndex = -1;
        }

        UpdateUI();
    }

    public void OnEquip()
    {
        if (selectedItem == null || selectedItem.equipPrefab == null)
        {
            // 현재 장착된 아이템이 있다면 해제
            if (CharacterManager.Instance != null && CharacterManager.Instance.Player != null)
            {
                CharacterManager.Instance.Player.equip.UnEquip();
            }

            return;
        }

        //slots[selectedIndex].equipped = true;
        //curEquipIndex = selectedIndex;

        if (CharacterManager.Instance != null && CharacterManager.Instance.Player != null && CharacterManager.Instance.Player.equip != null)
        {
            CharacterManager.Instance.Player.equip.EquipNew(selectedItem);
        }

        UpdateUI();
    }
}