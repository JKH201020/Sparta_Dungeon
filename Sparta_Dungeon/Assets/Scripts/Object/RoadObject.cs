using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RoadObject : MonoBehaviour
{
    public GameObject RoadPrefab; // 소환할 오브젝트 할당

    [Header("Spawn Range")] // 소환 범위
    public float spawnXZRange = 5f; // 발판의 기준으로 얼마나 떨어진 거리에 소환될 것인지
    public float spawnYRange = 0.1f; // 전 발판보다 얼마나 위에 있는지
    private bool roadSpawn = false; // 로드 생성 - 무한 생성 방지

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
        // 충돌 태그가 플레이어고 로드스폰이 false일 때
        if (other.gameObject.CompareTag("Player") && !roadSpawn)
        {
            SpawnRandomRoad(); // 로드 소환
            roadSpawn = true; // 로드가 소환 했다는 의미
        }
    }

    void SpawnRandomRoad() // 랜덤 위치에 로드 소환
    {
        // 현재 생성된 로드의 콜라이더 정보를 가져옴
        Collider curRoadCollider = GetComponent<Collider>();
        if (curRoadCollider == null)
        {
            Debug.LogError("Current RoadObject does not have a Collider component!", this);
            return;
        }

        // 현재 발판의 월드 경계(Bounds)를 가져옴
        Bounds bounds = curRoadCollider.bounds;

        // 현재 발판의 중심에서부터 spawnXZRange 따라 랜덤 위치 선정
        float randomX = Random.Range(bounds.center.x - bounds.size.x * 0.5f * spawnXZRange,
                                     bounds.center.x + bounds.size.x * 0.5f * spawnXZRange);
        float randomZ = Random.Range(bounds.center.z - bounds.size.z * 0.5f * spawnXZRange,
                                     bounds.center.z + bounds.size.z * 0.5f * spawnXZRange);

        // 새로운 발판의 Y 좌표 계산: 현재 발판의 상단 + 다음 발판 Y축 위치
        float randomY = bounds.max.y + spawnYRange;

         // 최종 스폰 위치 생성
         Vector3 spawnPosition = new Vector3(randomX, randomY, randomZ);
        // 새로운 발판을 생성합니다.
        GameObject newRoad = Instantiate(RoadPrefab, spawnPosition, Quaternion.identity);

        Debug.Log($"새로운 발판이 {spawnPosition}에 생성되었습니다. (원본: {gameObject.name})");
    }
}