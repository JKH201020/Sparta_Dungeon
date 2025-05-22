using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class JumpObject : MonoBehaviour
{
    float power = 3f;
    //private void OnTriggerEnter(Collider other)
    //{
    //    Debug.Log("������ ����");
    //    AddForce(other);
    //}

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("������ ����");
            AddForce(other);
        }
    }

    void AddForce(Collider other) // ������ ������
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody _playerRigidbody = other.GetComponent<Rigidbody>();

            _playerRigidbody.AddForce(transform.up * power, ForceMode.Impulse);
        }
    }
}
