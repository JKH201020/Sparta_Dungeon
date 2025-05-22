using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class JumpObject : MonoBehaviour
{
    public float power = 3f; // �⺻ 3
    Color newColor = Color.black; // ������ ����

    private void Start()
    {
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();

        meshRenderer.material.color = newColor; // ���� ����
    }

    private void OnCollisionEnter(Collision other) // ������� �浹 �� ������
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // _playerRigidbody�� �÷��̾��� rigidbody ������Ʈ�� ������ ����
            // �÷��̾��� rigidbody�� ������ ���� _playerRigidbody�� �ݿ�
            Rigidbody _playerRigidbody = other.gameObject.GetComponent<Rigidbody>();

            // ForceMode.Impulse: Impulse�� �Լ��� ȣ��Ǵ� �ٷ� �� ���� �� ���� ���� ����
            _playerRigidbody.AddForce(transform.up * power, ForceMode.Impulse);
        }
    }
}