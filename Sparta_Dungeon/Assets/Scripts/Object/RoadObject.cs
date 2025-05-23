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

    private bool isRoadSpawn = false; // 발판 생성 - 무한 생성 방지

    //static 키워드는 해당 변수가 클래스 자체에 속하며, 그 클래스의 모든 인스턴스(객체)가 공유하는 하나의 값이라는 것을 의미
    //RoadObject 스크립트가 여러 발판에 붙어 있을 때,
    //static 변수를 사용하지 않았다면 각 발판은 자신만의 lastSpawnedRoad 변수를 가짐
    private static GameObject lastSpawnedRoad; // 마지막에 소환된 발판
    private static GameObject initRoadReference; // 시작 발판

    // 이 static 변수를 초기화하는 static 메서드를 추가
    public static void ResetStatic() // 재시작 초기화
    {
        lastSpawnedRoad = null; 
        initRoadReference = null;
    }

    public static void SetInitRoadReference(GameObject initRoad) // 매개 변수로 첫 발판을 받아옴
    {
        initRoadReference = initRoad; // 매개변수를 재시작 후 첫 발판으로 저장 
        lastSpawnedRoad = initRoad; // 매개변수를 마지막에 소환했던 발판으로 저장
    }

    public void ResetRoadState() // 재시작 후 발판 생성 상태
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
                    lastSpawnedRoad.SetActive(false); // 이전 발판 비활성화
                }
                else
                {
                    Destroy(lastSpawnedRoad); // 이전 발판 제거
                } 
            }

            lastSpawnedRoad = this.gameObject; // 현재 발판을 이전 발판으로 지정

            SpawnRandomRoad(); // 발판 소환
            isRoadSpawn = true; // 발판 소환 했다는 의미  
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

        // 발판 중심으로 부터 랜덤 거리와 방향을 곱한 값으로 좌표 지정
        float randomX = bounds.center.x + (randomDistanceX * directionX);
        float randomY = bounds.center.y + (randomDistanceY * directionY);
        float randomZ = bounds.center.z + (randomDistanceZ * directionZ);

        if (randomY < 0.5f)
        {
            randomY = 0.5f;
        }

        // 최종 스폰 위치
        Vector3 spawnPosition = new Vector3(randomX, randomY, randomZ);

        // 새로운 발판을 생성 / Instantiate: 오브젝트 복사하는 함수
        // 새로운 위치에 회전값이 (0, 0, 0)인 RoadPrefab를 복사한다는 의미
        GameObject newRoad = Instantiate(RoadPrefab, spawnPosition, Quaternion.identity);
        newRoad.tag = "Road"; // // 생성된 새로운 발판에도 "Road" 태그를 부여

        // 생성된 발판에 RoadObject 스크립트를 추가하고 newRoadScript라는 이름으로 사용할 준비함
        RoadObject newRoadScript = newRoad.AddComponent<RoadObject>();
        // 새로 생성된 RoadObject 스크립트에게, 나중에 필요할 때 사용할 '길 프리팹 원본'이 무엇인지 알려줌
        newRoadScript.RoadPrefab = RoadPrefab;
        newRoadScript.ResetRoadState(); // 새로 만들어진 길(발판)의 상태를 초기화

        Debug.Log($"새로운 발판이 {spawnPosition}에 생성");
    }
}