using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEditor;
using UnityEngine;

public class UIInventory : MonoBehaviour
{
    public ItemSlot[] slots; // ������ ���� �迭

    public Transform slotPanel; // ���� �ǳ� ������Ʈ
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

        // ������ ���� ĭ�� slotPanel ������Ʈ�� �ڽ��� ������ �ҷ���
        slots = new ItemSlot[slotPanel.childCount];

        for (int i = 0; i < slots.Length; i++)
        {
            // �����ǳ��� �ڽĿ�����Ʈ ������ ���� ������Ʈ�� ������ ����
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

    ItemSlot GetEmptySlot() // ������ ������ �̾� ���� ���
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
        // �� ������ �������� ������
        Instantiate(data.dropPrefab, dropPosition.position, Quaternion.Euler(Vector3.one * Random.value * 360));
    }

    void SelectItem(int index)
    {
        if (slots[index].item == null) return;
    }
}