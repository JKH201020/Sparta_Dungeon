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
    public int selectedIndex = 0; // ���� ���õ� ������ �ε���

    private void Awake()
    {
        slotSetting();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (slots.Length > 0)
        {
            // ���� ���� �� 0�� ���� (ù ��° ����) ���� �� �ƿ����� Ȱ��ȭ
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

            CharacterManager.Instance.Player.addItem += AddItem; // CharacterManager�� AddItem �̺�Ʈ ����
        }

        if (slots != null)
        {
            // ������ ���� ĭ�� slotPanel ������Ʈ�� �ڽ��� ������ �ҷ���
            slots = new ItemSlot[slotPanel.childCount];

            for (int i = 0; i < slots.Length; i++)
            {
                Transform child = slotPanel.GetChild(i);
                if (child != null) // �ڽ��� �����ϸ�
                {
                    // �����ǳ��� �ڽĿ�����Ʈ ������ ���� ������Ʈ�� ������ ����
                    slots[i] = child.GetComponent<ItemSlot>();

                    if (slots[i] != null)
                    {
                        slots[i].index = i;
                        slots[i].inventory = this;
                        slots[i].SetOutlineActive(false); // ��� ������ �ƿ������� �⺻������ ��Ȱ��ȭ
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

    void AddItem()
    {
        ItemData data = CharacterManager.Instance.Player.itemData;
        ItemSlot targetSlot = slots[selectedIndex];

        if (targetSlot.item == null) // ���õ� ������ ����ִٸ�
        {
            targetSlot.item = data; // data ������ ������ ������ �Ҵ� (������ �ֱ�)
            targetSlot.quantity = 1; // ���� 1 �߰�
            UpdateUI();
            //  targetSlot.item�� data�� �Ѱ�� ������ ������ �ִ� ������ ������ �ʿ䰡 ������ - �׷��� null�� �ʱ�ȭ
            CharacterManager.Instance.Player.itemData = null; 
            SelectedSlot(selectedIndex); // ���� ������Ʈ
            return;
        }
        // ���õ� ���Կ� �̹� �������� �ְ�, ���� �����ϸ�, �ִ� ���� �̸��̶��
        else if (targetSlot.item == data && data.canStack && targetSlot.quantity < data.maxStackAmount)
        {
            targetSlot.quantity++;
            UpdateUI();
            CharacterManager.Instance.Player.itemData = null;
            SelectedSlot(selectedIndex);
            return;
        }

        // ��� �ִ� ���� ������
        ItemSlot emptySlot = GetEmptySlot();
        // �÷��̾ �ִ� ������ �����͸� ���
        CharacterManager.Instance.Player.itemData = null;

        if (emptySlot != null) // ����ִ� ������ �ִٸ�
        {
            emptySlot.item = data;
            emptySlot.quantity = 1;
            UpdateUI();
            CharacterManager.Instance.Player.itemData = null;
            SelectedSlot(selectedIndex);
            return;
        }

        //���ٸ� ������ ����
        ThrowItem(data);
        CharacterManager.Instance.Player.itemData = null;
        Debug.Log($"�κ��丮�� ���� ���� '{data.displayName}'��(��) ����");
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

    ItemSlot GetEmptySlot() // ������ ������ ��� ���� ���
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null) // �ƹ��͵� ���� ����
            {
                return slots[i];
            }
        }

        return null;
    }

    void ThrowItem(ItemData data)
    {
        // ���� �������� �� ������ �������� ������
        Instantiate(data.dropPrefab, dropPosition.position, Quaternion.Euler(Vector3.one * Random.value * 360));
    }

    public void SelectedSlot(int index)
    {
        // 0 <= selectedIndex < 2 && ���õ� ������ �����ϴ°�?
        if (selectedIndex >= 0 && selectedIndex < slots.Length && slots[selectedIndex] != null)
        {
            // ���õ� ������ �ƿ������� ��Ȱ��ȭ -> �� ������ ���� ������ �Ǿ����
            slots[selectedIndex].SetOutlineActive(false);
        }

        selectedIndex = index; // ���õ� �ε��� �ֽ�ȭ (���� ���� ��)

        if (selectedIndex >= 0 && selectedIndex < slots.Length && slots[selectedIndex] != null)
        {
            slots[selectedIndex].SetOutlineActive(true); // ���� �ε��� ������ �ƿ����� Ȱ��ȭ
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

        if (slots[selectedIndex].quantity <= 0) // ���õ� ������ ������ ������ 0�� ���ϸ�
        {
            selectedItem = null; // ���õ� �������� ����
            // ���õ� ���Կ� ����Ǿ� �ִ� �������� "����"�� ����
            slots[selectedIndex].item = null;
        }

        UpdateUI();
        SelectedSlot(selectedIndex);
    }
}