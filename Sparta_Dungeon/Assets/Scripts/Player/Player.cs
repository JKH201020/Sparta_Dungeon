using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour // Player와 관련된 기능을 모아두는 곳
{
    public PlayerController controller; // 플레이서 움직임
    public PlayerCondition condition; // 플레이어 상태
    public Equipment equip; // 장비 관리
    
    // 현재 상호작용하는 아이템의 데이터를 넘겨주고
    public ItemData itemData; // 이 스크립트에 아이템 데이터 넘겨줌
    // addItem이라는 델리게이트에 뭔가 구독이 되어있으면 실행시켜 주게끔 세팅
    public Action addItem;

    public Transform dropPosition; // 아이템 드랍(버릴) 위치

    private void Awake()
    {
        // 싱글톤매니저에 Player를 참조할 수 있게 데이터를 넘긴다.
        CharacterManager.Instance.Player = this;

        controller = GetComponent<PlayerController>();
        condition = GetComponent<PlayerCondition>();
        equip = GetComponent<Equipment>();
    }
}
