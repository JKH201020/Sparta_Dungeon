using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject initRoad; // ���� ����
    public GameObject roadPrefab; // ���� ������
    public GameObject playerObject;

    public Vector3 playerInitPosition = new Vector3(20, 1f, -20); // �÷��̾� ��ġ �ʱ�ȭ
    public Vector3 initRoadPosition = new Vector3(20, 3, 0); // ó�� ������ ������ ��ġ

    private void Start()
    {
        SetupInitState();
    }

    void SetupInitState()
    {
        // Road�±װ� �޷��ִ� ������Ʈ�� �� �迭�� ����
        GameObject[] currentRoads = GameObject.FindGameObjectsWithTag("Road");
        foreach (GameObject road in currentRoads)
        {
            if (road != initRoad) // �ʱ� ������ �������� ����
            {
                Destroy(road);
            }
        }

        RoadObject.ResetStatic();

        initRoad.SetActive(true); // ù ���� Ȱ��ȭ (��� �� ���Ŀ��� ��Ȱ��ȭ)
        initRoad.tag = "Road"; // ù ���ǿ� �±� �޾���

        // �ʱ� ���� �ٽ� ����
        RoadObject initRoadScript = initRoad.GetComponent<RoadObject>();
        if (initRoadScript == null)
        {
            initRoadScript = initRoad.AddComponent<RoadObject>();
        }

        initRoadScript.RoadPrefab = roadPrefab; // ���� ������ ������ �� ����� ���� ������
        initRoadScript.ResetRoadState();

        // ��� RoadObject �ν��Ͻ��� �������� ������ ��ġ�� ����(static) �������� �ʱ�ȭ
        RoadObject.SetInitRoadReference(initRoad);
    }

    public void ResetPosition()
    {
        Debug.Log("�÷��̾ ���ǿ��� ������ ���� ȯ���� �ʱ�ȭ�մϴ�.");

        // Road�±װ� �޷��ִ� ������Ʈ�� �� �迭�� ����
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
            Debug.LogWarning("�ʱ� ���ǿ� RoadObject ��ũ��Ʈ�� ��� �ٽ� �߰��մϴ�.");
            initRoadScript.RoadPrefab = roadPrefab; // ���� ������ �ٽ� �Ҵ�
        }

        initRoadScript.ResetRoadState();

        RoadObject.SetInitRoadReference(initRoad);
    }
}