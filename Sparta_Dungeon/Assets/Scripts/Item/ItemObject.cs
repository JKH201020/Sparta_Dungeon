using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Interaction.cs ���� ��ũ��Ʈ���� � ������ ��ü�� ���ϵ� ������� ��ȣ�ۿ� ������ ó��
// IInteractable Ÿ������ ������ GetInteractPrompt()�� OnInteract()�� �����ϰ� ȣ��
public interface IInteractable
{
    public string GetInteractPrompt(); // ��ȣ�ۿ� ����
    public void OnInteract(); // ��ȣ�ۿ� �� ����
}

public class ItemObject : MonoBehaviour, IInteractable
{
    public ItemData data;
    public UIInventory uiInven;

    public string GetInteractPrompt() // ������ �ٶ� ��
    {
        string str = $"{data.displayName}\n{data.description}"; // ������ �Ұ�
        return str;
    }

    public void OnInteract() // ��ȣ�ۿ� ���� �� (������ ȹ������ ��) 
    {
        // �÷��̾�� ���� ȹ���� ������ ������ ����
        CharacterManager.Instance.Player.itemData = data;
        // addItem�� �ʿ��� ����� �������� ����
        // ������ �߰� ��������Ʈ ���� (UIInventory���� ���� ��)
        CharacterManager.Instance.Player.addItem?.Invoke();

        Destroy(gameObject); // ȹ�� �� ȭ�鿡�� ������ ����
    }
}