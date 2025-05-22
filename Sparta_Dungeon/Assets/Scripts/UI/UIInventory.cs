using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInventory : MonoBehaviour
{
    public ItemSlot[] slots; // ������ ���� �迭

    public Transform slotPanel; // ���� �ǳ� ������Ʈ
    public Transform dropPosition;

    private PlayerController controller;
    private PlayerCondition condition;

    public ItemData selectedItem;
    public int selectedIndex = 0; // ���� ���õ� ������ �ε��� (�ƿ����� ǥ�ÿ�)

    //int curEquipIndex; // ���� ������ �������� ���� �ε��� (Equip �������� ���)

    private void Awake()
    {
        if (CharacterManager.Instance != null && CharacterManager.Instance.Player != null)
        {
            controller = CharacterManager.Instance.Player.controller;
            condition = CharacterManager.Instance.Player.condition;
            dropPosition = CharacterManager.Instance.Player.dropPosition;

            // CharacterManager�� AddItem �̺�Ʈ ����
            CharacterManager.Instance.Player.addItem += AddItem;
        }

        if (slots != null)
        {
            // ������ ���� ĭ�� slotPanel ������Ʈ�� �ڽ��� ������ �ҷ���
            slots = new ItemSlot[slotPanel.childCount];

            for (int i = 0; i < slots.Length; i++)
            {
                Transform child = slotPanel.GetChild(i);
                if (child != null)
                {
                    // �����ǳ��� �ڽĿ�����Ʈ ������ ���� ������Ʈ�� ������ ����
                    slots[i] = child.GetComponent<ItemSlot>();

                    if (slots[i] != null)
                    {
                        slots[i].index = i;
                        slots[i].inventory = this;

                        // ��� ������ �ƿ������� �⺻������ ��Ȱ��ȭ
                        slots[i].SetOutlineActive(false);
                    }
                    else
                    {
                        slots[i] = null; // null�� ���� �ʵ��� ��������� null �Ҵ�
                    }
                }
            }
        }
        else
        {
            slots = new ItemSlot[0]; // NullReferenceException ������ ���� �� �迭�� �ʱ�ȭ
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (slots.Length > 0)
        {
            // ���� ���� �� 0�� ���� (ù ��° ����) ���� �� �ƿ����� Ȱ��ȭ
            SelectedSlot(0);
        }
    }

    void AddItem()
    {
        ItemData data = CharacterManager.Instance.Player.itemData;

        // �ߺ� �������� - Ȥ�ó� �ؼ� ������
        if (data.canStack)
        {
            ItemSlot slot = GetItemStack(data);
            // ����ִ� ���� ������
            if (slot != null)
            {
                slot.quantity++;
                UpdateUI();
                CharacterManager.Instance.Player.itemData = null;
                return;
            }
        }

        // ��� �ִ� ���� ������
        ItemSlot emptySlot = GetEmptySlot();

        // �÷��̾ �ִ� ������ �����͸� ���
        CharacterManager.Instance.Player.itemData = null;

        if (emptySlot != null)
        {
            emptySlot.item = data;
            emptySlot.quantity = 1;
            UpdateUI();
            CharacterManager.Instance.Player.itemData = null;
            return;
        }

        //���ٸ� ������ ����
        ThrowItem(data);
        CharacterManager.Instance.Player.itemData = null;
    }

    void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item != null)
            {
                // �ش� ���Կ� �� ���� ��
                slots[i].Set(); // ������ ����
            }
            else
            {
                // �ش� ���� ���
                slots[i].Clear();
            }
        }
    }

    ItemSlot GetItemStack(ItemData data) // ������ ���� �߰�
    {
        for (int i = 0; i < slots.Length; i++)
        {
            // �ִ� ������ ���ٸ� ������ ������ �߰�
            if (slots[i].item == data && slots[i].quantity < data.maxStackAmount)
            {
                return slots[i];
            }
        }

        return null;
    }

    ItemSlot GetEmptySlot() // ������ ������ ��� ���� ���
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

    void ThrowItem(ItemData data) // �ϴ� �ڵ常 �ۼ�
    {
        // ���� �������� �� ������ �������� ������
        Instantiate(data.dropPrefab, dropPosition.position, Quaternion.Euler(Vector3.one * Random.value * 360));
    }

    public void SelectedSlot(int index)
    {
        // ������ ������ �ƿ������� ��Ȱ��ȭ
        if (selectedIndex >= 0 && selectedIndex < slots.Length && slots[selectedIndex] != null)
        {
            slots[selectedIndex].SetOutlineActive(false);
        }

        selectedIndex = index;

        if (selectedIndex >= 0 && selectedIndex < slots.Length && slots[selectedIndex] != null)
        {
            slots[selectedIndex].SetOutlineActive(true);
            selectedItem = slots[selectedIndex].item; // ���õ� ������ ������ ������Ʈ
        }
        else
        {
            selectedItem = null; // ��ȿ���� ���� �ε����� ��� ���õ� ������ �ʱ�ȭ
        }
    }

    public void RemoveSelectedItem() // ������ �����ų� ������� ��
    {
        slots[selectedIndex].quantity--; // ������ ���� ����

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
            // ���� ������ �������� �ִٸ� ����
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