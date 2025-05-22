using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class RoadObject : MonoBehaviour
{
    public GameObject RoadPrefab; // 소환할 오브젝트 할당

    [Header("Spawn Range")] // 소환 범위
    //public float spawnYRange; // 전 발판보다 얼마나 위에 있는지

    private bool roadSpawn = false; // 발판 생성 - 무한 생성 방지

    private static GameObject lastSpawnedRoad;

    private void OnCollisionEnter(Collision other)
    {
        // 충돌 태그가 플레이어고 로드스폰이 false일 때
        if (other.gameObject.CompareTag("Player") && !roadSpawn)
        {
            SpawnRandomRoad(); // 발판 소환
            roadSpawn = true; // 발판 소환 했다는 의미

            Destroy(lastSpawnedRoad); // 이전 발판 제거
            lastSpawnedRoad = this.gameObject; // 현재 발판을 이전 발판으로 지정
        }
    }

    void SpawnRandomRoad() // 랜덤 위치에 로드 소환
    {
        // 현재 생성된 발판의 콜라이더 정보를 가져옴
        Collider curRoadCollider = GetComponent<Collider>();

        Bounds bounds = curRoadCollider.bounds; // 현재 발판의 월드 경계(Bounds)를 가져옴

        // 발판 간의 거리
        float randomDistanceX = Random.Range(2f, 4f);
        float randomDistanceZ = Random.Range(2f, 4f);
        float randomDistanceY = Random.Range(0f, 1.2f); // 높이

        // 발판 생성 방향 설정
        float directionX = Random.Range(0, 2) == 0 ? 1f : -1f; // 좌 우
        float directionY = Random.Range(0, 2) == 0 ? 1f : -1f; // 위 아래
        float directionZ = Random.Range(0, 2) == 0 ? 1f : -1f;

        // 현재 발판의 중심에서부터 spawnXZRange 따라 랜덤 위치 선정
        float randomX = bounds.center.x + (randomDistanceX * directionX);
        float randomY = bounds.center.y + (randomDistanceY * directionY);
        float randomZ = bounds.center.z + (randomDistanceZ * directionZ);

        if(randomY < 0.5f)
        {
            randomY = 0.5f;
        }

        // 새로운 발판의 Y 좌표 계산: 현재 발판의 상단 + 다음 발판 Y축 위치
        //float positionY = bounds.max.y + spawnYRange;

        // 최종 스폰 위치
        Vector3 spawnPosition = new Vector3(randomX, randomY, randomZ);
        // 새로운 발판을 생성
        GameObject newRoad = Instantiate(RoadPrefab, spawnPosition, Quaternion.identity);

        Debug.Log($"새로운 발판이 {spawnPosition}에 생성");
    }
}