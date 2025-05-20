using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour // Player와 관련된 기능을 모아두는 곳
{
    [SerializeField] PlayerController controller;

    private void Awake()
    {
        // 싱글톤매니저에 Player를 참조할 수 있게 데이터를 넘긴다.
        CharacterManager.Instance.player = this;
        controller = GetComponent<PlayerController>();   
    }
}
