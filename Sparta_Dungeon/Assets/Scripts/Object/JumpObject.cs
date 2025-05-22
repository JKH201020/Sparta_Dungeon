using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class JumpObject : MonoBehaviour
{
    public float power = 3f; // 기본 3
    Color newColor = Color.black; // 점프대 색상

    private void Start()
    {
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();

        meshRenderer.material.color = newColor; // 색상 변경
    }

    private void OnCollisionEnter(Collision other) // 점프대와 충돌 시 점프됨
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // _playerRigidbody에 플레이어의 rigidbody 컴포넌트를 가져와 저장
            // 플레이어의 rigidbody의 정보에 따라 _playerRigidbody에 반영
            Rigidbody _playerRigidbody = other.gameObject.GetComponent<Rigidbody>();

            // ForceMode.Impulse: Impulse는 함수가 호출되는 바로 그 순간 한 번만 힘을 적용
            _playerRigidbody.AddForce(transform.up * power, ForceMode.Impulse);
        }
    }
}