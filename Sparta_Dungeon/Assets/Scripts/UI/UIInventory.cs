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
    public int selectedIndex = 0; // 현재 선택된 슬롯의 인덱스

    private void Awake()
    {
        slotSetting();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (slots.Length > 0)
        {
            // 게임 시작 시 0번 슬롯 (첫 번째 슬롯) 선택 및 아웃라인 활성화
            SelectedSlot(selectedIndex);
        }
    }

    void slotSetting()
    {
        if (CharacterManager.Instance != null && CharacterManager.Instance.Player != null)
        {
            controller = CharacterManager.Instance.Player.controller;
            condition = CharacterManager.Instance.Player.condition;
            dropPosition = CharacterManager.Instance.Player.dropPosition;

            CharacterManager.Instance.Player.addItem += AddItem; // CharacterManager의 AddItem 이벤트 구독
        }

        if (slots != null)
        {
            // 아이템 슬롯 칸을 slotPanel 오브젝트의 자식의 개수를 불러옴
            slots = new ItemSlot[slotPanel.childCount];

            for (int i = 0; i < slots.Length; i++)
            {
                Transform child = slotPanel.GetChild(i);
                if (child != null) // 자식이 존재하면
                {
                    // 슬롯판넬의 자식오브젝트 아이템 슬롯 컴포넌트를 가져와 저장
                    slots[i] = child.GetComponent<ItemSlot>();

                    if (slots[i] != null)
                    {
                        slots[i].index = i;
                        slots[i].inventory = this;
                        slots[i].SetOutlineActive(false); // 모든 슬롯의 아웃라인을 기본적으로 비활성화
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

    void AddItem()
    {
        ItemData data = CharacterManager.Instance.Player.itemData;
        ItemSlot targetSlot = slots[selectedIndex];

        if (targetSlot.item == null) // 선택된 슬롯이 비어있다면
        {
            targetSlot.item = data; // data 아이템 데이터 정보를 할당 (아이템 넣기)
            targetSlot.quantity = 1; // 개수 1 추가
            UpdateUI();
            //  targetSlot.item에 data를 넘겼기 때문에 가지고 있던 아이템 정보는 필요가 없어짐 - 그래서 null로 초기화
            CharacterManager.Instance.Player.itemData = null; 
            SelectedSlot(selectedIndex); // 슬롯 업데이트
            return;
        }
        // 선택된 슬롯에 이미 아이템이 있고, 스택 가능하며, 최대 수량 미만이라면
        else if (targetSlot.item == data && data.canStack && targetSlot.quantity < data.maxStackAmount)
        {
            targetSlot.quantity++;
            UpdateUI();
            CharacterManager.Instance.Player.itemData = null;
            SelectedSlot(selectedIndex);
            return;
        }

        // 비어 있는 슬롯 가져옴
        ItemSlot emptySlot = GetEmptySlot();
        // 플레이어에 있는 아이템 데이터를 비움
        CharacterManager.Instance.Player.itemData = null;

        if (emptySlot != null) // 비어있는 슬롯이 있다면
        {
            emptySlot.item = data;
            emptySlot.quantity = 1;
            UpdateUI();
            CharacterManager.Instance.Player.itemData = null;
            SelectedSlot(selectedIndex);
            return;
        }

        //없다면 아이템 버림
        ThrowItem(data);
        CharacterManager.Instance.Player.itemData = null;
        Debug.Log($"인벤토리가 가득 차서 '{data.displayName}'을(를) 버림");
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

    ItemSlot GetEmptySlot() // 아이템 슬롯이 비어 있을 경우
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null) // 아무것도 없는 슬롯
            {
                return slots[i];
            }
        }

        return null;
    }

    void ThrowItem(ItemData data)
    {
        // 버린 아이템이 내 주위에 랜덤으로 떨어짐
        Instantiate(data.dropPrefab, dropPosition.position, Quaternion.Euler(Vector3.one * Random.value * 360));
    }

    public void SelectedSlot(int index)
    {
        // 0 <= selectedIndex < 2 && 선택된 슬롯이 존재하는가?
        if (selectedIndex >= 0 && selectedIndex < slots.Length && slots[selectedIndex] != null)
        {
            // 선택된 슬롯의 아웃라인을 비활성화 -> 이 슬롯은 이전 슬롯이 되어버림
            slots[selectedIndex].SetOutlineActive(false);
        }

        selectedIndex = index; // 선택된 인덱스 최신화 (슬롯 변경 후)

        if (selectedIndex >= 0 && selectedIndex < slots.Length && slots[selectedIndex] != null)
        {
            slots[selectedIndex].SetOutlineActive(true); // 현재 인덱스 슬롯의 아웃라인 활성화
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

        if (slots[selectedIndex].quantity <= 0) // 선택된 슬롯의 아이템 개수가 0개 이하면
        {
            selectedItem = null; // 선택된 아이템이 없음
            // 선택된 슬롯에 저장되어 있던 아이템의 "정보"를 지움
            slots[selectedIndex].item = null;
        }

        UpdateUI();
        SelectedSlot(selectedIndex);
    }
}