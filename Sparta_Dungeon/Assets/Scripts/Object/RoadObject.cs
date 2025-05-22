using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RoadObject : MonoBehaviour
{
    public GameObject RoadPrefab; // ��ȯ�� ������Ʈ �Ҵ�

    [Header("Spawn Range")] // ��ȯ ����
    public float spawnXZRange = 5f; // ������ �������� �󸶳� ������ �Ÿ��� ��ȯ�� ������
    public float spawnYRange = 0.1f; // �� ���Ǻ��� �󸶳� ���� �ִ���
    private bool roadSpawn = false; // �ε� ���� - ���� ���� ����

    //// Start is called before the first frame update
    //void Start()
    //{
    //    for (int i = 0; i < 1; i++)
    //    {
    //        SpawnRandomRoad();
    //    }
    //}

    private void OnCollisionEnter(Collision other)
    {
        // �浹 �±װ� �÷��̾�� �ε彺���� false�� ��
        if (other.gameObject.CompareTag("Player") && !roadSpawn)
        {
            SpawnRandomRoad(); // �ε� ��ȯ
            roadSpawn = true; // �ε尡 ��ȯ �ߴٴ� �ǹ�
        }
    }

    void SpawnRandomRoad() // ���� ��ġ�� �ε� ��ȯ
    {
        // ���� ������ �ε��� �ݶ��̴� ������ ������
        Collider curRoadCollider = GetComponent<Collider>();
        if (curRoadCollider == null)
        {
            Debug.LogError("Current RoadObject does not have a Collider component!", this);
            return;
        }

        // ���� ������ ���� ���(Bounds)�� ������
        Bounds bounds = curRoadCollider.bounds;

        // ���� ������ �߽ɿ������� spawnXZRange ���� ���� ��ġ ����
        float randomX = Random.Range(bounds.center.x - bounds.size.x * 0.5f * spawnXZRange,
                                     bounds.center.x + bounds.size.x * 0.5f * spawnXZRange);
        float randomZ = Random.Range(bounds.center.z - bounds.size.z * 0.5f * spawnXZRange,
                                     bounds.center.z + bounds.size.z * 0.5f * spawnXZRange);

        // ���ο� ������ Y ��ǥ ���: ���� ������ ��� + ���� ���� Y�� ��ġ
        float randomY = bounds.max.y + spawnYRange;

         // ���� ���� ��ġ ����
         Vector3 spawnPosition = new Vector3(randomX, randomY, randomZ);
        // ���ο� ������ �����մϴ�.
        GameObject newRoad = Instantiate(RoadPrefab, spawnPosition, Quaternion.identity);

        Debug.Log($"���ο� ������ {spawnPosition}�� �����Ǿ����ϴ�. (����: {gameObject.name})");
    }
}