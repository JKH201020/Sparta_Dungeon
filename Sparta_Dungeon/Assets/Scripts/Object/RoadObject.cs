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
    //public float spawnXZRange; // ������ �������� �󸶳� ������ �Ÿ��� ��ȯ�� ������
    public float spawnYRange; // �� ���Ǻ��� �󸶳� ���� �ִ���

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

        // ���� ���� ���� ����
        float directionX = Random.Range(0, 2) == 0 ? 1f : -1f;
        float directionZ = Random.Range(0, 2) == 0 ? 1f : -1f;

        // ���� ������ �߽ɿ������� spawnXZRange ���� ���� ��ġ ����
        float randomX = bounds.center.x + (randomDistanceX * directionX);
        float randomZ = bounds.center.z + (randomDistanceZ * directionZ);
        // ���ο� ������ Y ��ǥ ���: ���� ������ ��� + ���� ���� Y�� ��ġ
        float positionY = bounds.max.y + spawnYRange;

        // ���� ���� ��ġ ����
        Vector3 spawnPosition = new Vector3(randomX, positionY, randomZ);
        // ���ο� ������ ����
        GameObject newRoad = Instantiate(RoadPrefab, spawnPosition, Quaternion.identity);

        Debug.Log($"���ο� ������ {spawnPosition}�� ����");
    }
}