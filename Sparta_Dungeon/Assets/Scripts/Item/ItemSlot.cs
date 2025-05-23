using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public ItemData item;

    public Image icon;
    public TextMeshProUGUI quantityText; // 아이템 개수
    private Outline outline;

    public UIInventory inventory;

    public int index; // 아이템 배열 인덱스
    public int quantity; // 정보를 넣어줌

    private void Awake()
    {
        outline = GetComponent<Outline>();
    }

    public void Set() // 아이템 획득시 아이템 슬롯에 추가
    {
        icon.gameObject.SetActive(true);
        icon.sprite = item.icon;
        quantityText.text = quantity > 0 ? quantity.ToString() : string.Empty; // 개수 표시
    }

    public void Clear() // 아이템이 슬롯에서 아예 없을 때
    {
        icon.gameObject.SetActive(false);
        item = null;
        quantityText.text = string.Empty;

        if (outline != null)
        {
            outline.enabled = false;
        }
    }

    public void SetOutlineActive(bool isActive) // 아웃라인 활성화를 선택하는 메서드
    {
        if (outline != null)
        {
            outline.enabled = isActive;
        }
    }
}