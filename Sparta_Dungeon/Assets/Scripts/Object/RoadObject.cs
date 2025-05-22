using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class RoadObject : MonoBehaviour
{
    public GameObject RoadPrefab; // ��ȯ�� ������Ʈ �Ҵ�

    [Header("Spawn Range")] // ��ȯ ����
    //public float spawnYRange; // �� ���Ǻ��� �󸶳� ���� �ִ���

    private bool roadSpawn = false; // ���� ���� - ���� ���� ����

    private static GameObject lastSpawnedRoad;

    private void OnCollisionEnter(Collision other)
    {
        // �浹 �±װ� �÷��̾�� �ε彺���� false�� ��
        if (other.gameObject.CompareTag("Player") && !roadSpawn)
        {
            SpawnRandomRoad(); // ���� ��ȯ
            roadSpawn = true; // ���� ��ȯ �ߴٴ� �ǹ�

            Destroy(lastSpawnedRoad); // ���� ���� ����
            lastSpawnedRoad = this.gameObject; // ���� ������ ���� �������� ����
        }
    }

    void SpawnRandomRoad() // ���� ��ġ�� �ε� ��ȯ
    {
        // ���� ������ ������ �ݶ��̴� ������ ������
        Collider curRoadCollider = GetComponent<Collider>();

        Bounds bounds = curRoadCollider.bounds; // ���� ������ ���� ���(Bounds)�� ������

        // ���� ���� �Ÿ�
        float randomDistanceX = Random.Range(2f, 4f);
        float randomDistanceZ = Random.Range(2f, 4f);
        float randomDistanceY = Random.Range(0f, 1.2f); // ����

        // ���� ���� ���� ����
        float directionX = Random.Range(0, 2) == 0 ? 1f : -1f; // �� ��
        float directionY = Random.Range(0, 2) == 0 ? 1f : -1f; // �� �Ʒ�
        float directionZ = Random.Range(0, 2) == 0 ? 1f : -1f;

        // ���� ������ �߽ɿ������� spawnXZRange ���� ���� ��ġ ����
        float randomX = bounds.center.x + (randomDistanceX * directionX);
        float randomY = bounds.center.y + (randomDistanceY * directionY);
        float randomZ = bounds.center.z + (randomDistanceZ * directionZ);

        if(randomY < 0.5f)
        {
            randomY = 0.5f;
        }

        // ���ο� ������ Y ��ǥ ���: ���� ������ ��� + ���� ���� Y�� ��ġ
        //float positionY = bounds.max.y + spawnYRange;

        // ���� ���� ��ġ
        Vector3 spawnPosition = new Vector3(randomX, randomY, randomZ);
        // ���ο� ������ ����
        GameObject newRoad = Instantiate(RoadPrefab, spawnPosition, Quaternion.identity);

        Debug.Log($"���ο� ������ {spawnPosition}�� ����");
    }
}