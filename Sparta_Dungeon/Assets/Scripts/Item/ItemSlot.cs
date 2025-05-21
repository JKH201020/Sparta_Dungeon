using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public ItemData item;

    public Image icon;
    public TextMeshProUGUI quantityText;
    private Outline outline;

    public UIInventory inventory;

    public int index; // ������ �迭 �ε���
    public bool equipped; // �������ΰ�?
    public int quantity; // ������ �־���

    private void Awake()
    {
        outline = GetComponent<Outline>();
    }

    private void OnEnable()
    {
        outline.enabled = equipped;
    }

    public void Set()
    {
        icon.gameObject.SetActive(true);
        icon.sprite = item.icon;
        quantityText.text = quantity > 0 ? quantity.ToString() : string.Empty; // ���� ǥ��

        if (outline != null)
        {
            outline.enabled = equipped;
        }
    }

    public void Clear()
    {
        icon.gameObject.SetActive(false);
        item = null;
        quantityText.text = string.Empty;
    }
}
