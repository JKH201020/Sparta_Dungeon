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

    public int index; // 아이템 배열 인덱스
    public bool equipped; // 장착중인가?
    public int quantity; // 정보를 넣어줌

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
        quantityText.text = quantity > 0 ? quantity.ToString() : string.Empty; // 개수 표시

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
