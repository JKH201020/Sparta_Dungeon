//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using Random = UnityEngine.Random;

//public class RoadObject : MonoBehaviour
//{
//    public GameObject RoadPrefab;

//    [Header("Spawn Range")]
//    public int minX = -5; // X�� �ּ� ����
//    public int maxX = 5; // X�� �ִ� ����
//    public int minZ = -5; // Z�� �ּ� ����
//    public int maxZ = 5; // Z�� �ִ� ����

//    // Start is called before the first frame update
//    void Start()
//    {
//        for (int i = 0; i < 1; i++)
//        {
//            SpawnRandomRoad();
//        }
//    }

//    private void Update()
//    {
        
//    }

//    void SpawnRandomRoad()
//    {
//        int randomX = Random.Range(minX, maxX);
//        int randomZ = Random.Range(minZ, maxZ);

//        Vector3 randomPosition = new Vector3(randomX, 0, randomZ);

//        Instantiate(RoadPrefab, randomPosition, Quaternion.identity);
//        Debug.Log($"Cube spawned at: ({randomX:F2}, {randomZ:F2})");
//    }
//}
