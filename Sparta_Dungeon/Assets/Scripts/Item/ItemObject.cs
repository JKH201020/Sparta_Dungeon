using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Interaction.cs 같은 스크립트에서 어떤 종류의 객체든 통일된 방식으로 상호작용 로직을 처리
// IInteractable 타입으로 받으면 GetInteractPrompt()나 OnInteract()를 안전하게 호출
public interface IInteractable
{
    public string GetInteractPrompt(); // 상호작용 문구
    public void OnInteract(); // 상호작용 시 실행
}

public class ItemObject : MonoBehaviour, IInteractable
{
    public ItemData data;
    public UIInventory uiInven;

    public string GetInteractPrompt() // 아이템 바라볼 때
    {
        string str = $"{data.displayName}\n{data.description}"; // 아이템 소개
        return str;
    }

    public void OnInteract() // 상호작용 했을 때 (아이템 획득했을 때) 
    {
        // 플레이어에게 현재 획득한 아이템 데이터 전달
        CharacterManager.Instance.Player.itemData = data;
        // addItem에 필요한 기능을 구독시켜 놓음
        // 아이템 추가 델리게이트 실행 (UIInventory에서 구독 중)
        CharacterManager.Instance.Player.addItem?.Invoke();

        Destroy(gameObject); // 획득 후 화면에서 아이템 제거
    }
}