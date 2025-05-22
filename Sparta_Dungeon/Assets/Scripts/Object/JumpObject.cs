using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class JumpObject : MonoBehaviour
{
    float power = 3f;
    //private void OnTriggerEnter(Collider other)
    //{
    //    Debug.Log("점프대 진입");
    //    AddForce(other);
    //}

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("점프대 진입");
            AddForce(other);
        }
    }

    void AddForce(Collider other) // 점프대 점프력
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody _playerRigidbody = other.GetComponent<Rigidbody>();

            _playerRigidbody.AddForce(transform.up * power, ForceMode.Impulse);
        }
    }
}
