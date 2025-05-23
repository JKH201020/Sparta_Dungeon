using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject initRoad; // 시작 발판
    public GameObject roadPrefab; // 발판 프리팹
    public GameObject playerObject;

    public Vector3 playerInitPosition = new Vector3(20, 1f, -20); // 플레이어 위치 초기화
    public Vector3 initRoadPosition = new Vector3(20, 3, 0); // 처음 발판이 생성될 위치

    private void Start()
    {
        SetupInitState();
    }

    void SetupInitState()
    {
        // Road태그가 달려있는 오브젝트는 이 배열에 저장
        GameObject[] currentRoads = GameObject.FindGameObjectsWithTag("Road");
        foreach (GameObject road in currentRoads)
        {
            if (road != initRoad) // 초기 발판은 제거하지 않음
            {
                Destroy(road);
            }
        }

        RoadObject.ResetStatic();

        initRoad.SetActive(true); // 첫 발판 활성화 (밟고 난 이후에는 비활성화)
        initRoad.tag = "Road"; // 첫 발판에 태그 달아줌

        // 초기 발판 다시 생성
        RoadObject initRoadScript = initRoad.GetComponent<RoadObject>();
        if (initRoadScript == null)
        {
            initRoadScript = initRoad.AddComponent<RoadObject>();
        }

        initRoadScript.RoadPrefab = roadPrefab; // 다음 발판을 생성할 때 사용할 원본 프리팹
        initRoadScript.ResetRoadState();

        // 모든 RoadObject 인스턴스에 공통으로 영향을 미치는 정적(static) 변수들을 초기화
        RoadObject.SetInitRoadReference(initRoad);
    }

    public void ResetPosition()
    {
        Debug.Log("플레이어가 발판에서 떨어져 게임 환경을 초기화합니다.");

        // Road태그가 달려있는 오브젝트는 이 배열에 저장
        GameObject[] currentRoads = GameObject.FindGameObjectsWithTag("Road");
        foreach (GameObject road in currentRoads)
        {
            if (road != initRoad)
            {
                Destroy(road);
            }
        }

        RoadObject.ResetStatic();

        initRoad.SetActive(true);

        RoadObject initRoadScript = initRoad.GetComponent<RoadObject>();

        if (initRoadScript == null)
        {
            initRoadScript = initRoad.AddComponent<RoadObject>();
            Debug.LogWarning("초기 발판에 RoadObject 스크립트가 없어서 다시 추가합니다.");
            initRoadScript.RoadPrefab = roadPrefab; // 원본 프리팹 다시 할당
        }

        initRoadScript.ResetRoadState();

        RoadObject.SetInitRoadReference(initRoad);
    }
}