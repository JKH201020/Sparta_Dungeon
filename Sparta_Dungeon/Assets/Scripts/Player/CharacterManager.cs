using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    private static CharacterManager _instance;
    public static CharacterManager Instance // 싱글톤 생성
    {
        get
        {
            if (_instance == null)
            {
                // 인스턴스가 없으면 CharacterManager 스크립트가 있는 CharacterManager오브젝트 생성
                _instance = new GameObject("CharacterManager").AddComponent<CharacterManager>();
            }

            return _instance;
        }
    }

    public Player _player; // 플레이어 컴포넌트 참조 저장
    public Player Player
    {
        get { return _player; }
        set { _player = value; }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject); // 씬이 바뀌어도 파괴되지 않음
        }
        else
        {
            if (_instance != this)
            {
                Destroy(gameObject); // 중복 인스턴스 파괴
            }
        }
    }
}