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
    public float spawnXZRange; // ������ �������� �󸶳� ������ �Ÿ��� ��ȯ�� ������
    public float spawnYRange; // �� ���Ǻ��� �󸶳� ���� �ִ���
    private bool roadSpawn = false; // �ε� ���� - ���� ���� ����

    private static GameObject lastSpawnedRoad;

    private void OnCollisionEnter(Collision other)
    {
        // �浹 �±װ� �÷��̾�� �ε彺���� false�� ��
        if (other.gameObject.CompareTag("Player") && !roadSpawn)
        {
            SpawnRandomRoad(); // �ε� ��ȯ
            roadSpawn = true; // �ε尡 ��ȯ �ߴٴ� �ǹ�
            Destroy(lastSpawnedRoad); // ���� ���� ����
            lastSpawnedRoad = this.gameObject; // ���� ������ ���� �������� ����
        }
    }

    void SpawnRandomRoad() // ���� ��ġ�� �ε� ��ȯ
    {
        // ���� ������ �ε��� �ݶ��̴� ������ ������
        Collider curRoadCollider = GetComponent<Collider>();
        // ���� ������ ���� ���(Bounds)�� ������
        Bounds bounds = curRoadCollider.bounds;

        // ���� ������ �߽ɿ������� spawnXZRange ���� ���� ��ġ ����
        float randomX = Random.Range(bounds.center.x - bounds.size.x * 0.5f * spawnXZRange,
                                     bounds.center.x + bounds.size.x * 0.5f * spawnXZRange);
        //float randomY = Random.Range(bounds.center.y - bounds.size.y * 0.5f * spawnYRange,
        //                             bounds.center.y + bounds.size.y * 0.5f * spawnYRange);
        float randomZ = Random.Range(bounds.center.z - bounds.size.z * 0.5f * spawnXZRange,
                                     bounds.center.z + bounds.size.z * 0.5f * spawnXZRange);
        // ���ο� ������ Y ��ǥ ���: ���� ������ ��� + ���� ���� Y�� ��ġ
        float positionY = bounds.max.y + spawnYRange;

        if (randomX < 3) // �ּҰ� ĿƮ���� ����
        {
            randomX = 3;
        }

        if (randomZ < 3) // �ּҰ� ĿƮ���� ����
        {
            randomZ = 3;
        }

        // ���� ���� ��ġ ����
        Vector3 spawnPosition = new Vector3(randomX, positionY, randomZ);
        // ���ο� ������ �����մϴ�.
        GameObject newRoad = Instantiate(RoadPrefab, spawnPosition, Quaternion.identity);

        Debug.Log($"���ο� ������ {spawnPosition}�� �����Ǿ����ϴ�. (����: {gameObject.name})");
    }
}