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

    private bool isRoadSpawn = false; // ���� ���� - ���� ���� ����

    //static Ű����� �ش� ������ Ŭ���� ��ü�� ���ϸ�, �� Ŭ������ ��� �ν��Ͻ�(��ü)�� �����ϴ� �ϳ��� ���̶�� ���� �ǹ�
    //RoadObject ��ũ��Ʈ�� ���� ���ǿ� �پ� ���� ��,
    //static ������ ������� �ʾҴٸ� �� ������ �ڽŸ��� lastSpawnedRoad ������ ����
    private static GameObject lastSpawnedRoad; // �������� ��ȯ�� ����
    private static GameObject initRoadReference; // ���� ����

    // �� static ������ �ʱ�ȭ�ϴ� static �޼��带 �߰�
    public static void ResetStatic() // ����� �ʱ�ȭ
    {
        lastSpawnedRoad = null; 
        initRoadReference = null;
    }

    public static void SetInitRoadReference(GameObject initRoad) // �Ű� ������ ù ������ �޾ƿ�
    {
        initRoadReference = initRoad; // �Ű������� ����� �� ù �������� ���� 
        lastSpawnedRoad = initRoad; // �Ű������� �������� ��ȯ�ߴ� �������� ����
    }

    public void ResetRoadState() // ����� �� ���� ���� ����
    {
        isRoadSpawn = false;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player") && !isRoadSpawn)
        {
            if (lastSpawnedRoad != null && lastSpawnedRoad != this.gameObject)
            {
                if(lastSpawnedRoad == initRoadReference)
                {
                    lastSpawnedRoad.SetActive(false); // ���� ���� ��Ȱ��ȭ
                }
                else
                {
                    Destroy(lastSpawnedRoad); // ���� ���� ����
                } 
            }

            lastSpawnedRoad = this.gameObject; // ���� ������ ���� �������� ����

            SpawnRandomRoad(); // ���� ��ȯ
            isRoadSpawn = true; // ���� ��ȯ �ߴٴ� �ǹ�  
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

        // ���� �߽����� ���� ���� �Ÿ��� ������ ���� ������ ��ǥ ����
        float randomX = bounds.center.x + (randomDistanceX * directionX);
        float randomY = bounds.center.y + (randomDistanceY * directionY);
        float randomZ = bounds.center.z + (randomDistanceZ * directionZ);

        if (randomY < 0.5f)
        {
            randomY = 0.5f;
        }

        // ���� ���� ��ġ
        Vector3 spawnPosition = new Vector3(randomX, randomY, randomZ);

        // ���ο� ������ ���� / Instantiate: ������Ʈ �����ϴ� �Լ�
        // ���ο� ��ġ�� ȸ������ (0, 0, 0)�� RoadPrefab�� �����Ѵٴ� �ǹ�
        GameObject newRoad = Instantiate(RoadPrefab, spawnPosition, Quaternion.identity);
        newRoad.tag = "Road"; // // ������ ���ο� ���ǿ��� "Road" �±׸� �ο�

        // ������ ���ǿ� RoadObject ��ũ��Ʈ�� �߰��ϰ� newRoadScript��� �̸����� ����� �غ���
        RoadObject newRoadScript = newRoad.AddComponent<RoadObject>();
        // ���� ������ RoadObject ��ũ��Ʈ����, ���߿� �ʿ��� �� ����� '�� ������ ����'�� �������� �˷���
        newRoadScript.RoadPrefab = RoadPrefab;
        newRoadScript.ResetRoadState(); // ���� ������� ��(����)�� ���¸� �ʱ�ȭ

        Debug.Log($"���ο� ������ {spawnPosition}�� ����");
    }
}