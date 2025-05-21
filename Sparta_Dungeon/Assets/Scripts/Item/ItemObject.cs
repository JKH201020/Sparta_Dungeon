using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public string GetInteractPrompt();
    public void OnInteract();
}

public class ItemObject : MonoBehaviour, IInteractable
{
    public ItemData data;
    public UIInventory uiInven;

    public string GetInteractPrompt()
    {
        string str = $"{data.displayName}\n{data.description}";
        return str;
    }

    public void OnInteract()
    {
        // 내가 가지고 있는 아이템의 데이터를 넣어줌
        CharacterManager.Instance.Player.itemData = data;
        // addItem에 필요한 기능을 구독시켜 놓음
        CharacterManager.Instance.Player.addItem?.Invoke();
        uiInven.SelectItem(uiInven.selectedItemIndex);
        uiInven.OnEquip();

        Destroy(gameObject);
    }
}
