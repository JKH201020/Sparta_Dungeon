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
        // ���� ������ �ִ� �������� �����͸� �־���
        CharacterManager.Instance.Player.itemData = data;
        // addItem�� �ʿ��� ����� �������� ����
        CharacterManager.Instance.Player.addItem?.Invoke();
        uiInven.SelectItem(uiInven.selectedItemIndex);
        uiInven.OnEquip();

        Destroy(gameObject);
    }
}
