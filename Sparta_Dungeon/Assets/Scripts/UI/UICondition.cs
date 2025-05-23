using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICondition : MonoBehaviour
{
    // 값들을 계속 업데이트 시킬 거임
    public Condition health; // 체력 UI를 관리할 Condition 컴포넌트 참조
    public Condition jump; // 점프력 UI를 관리할 Condition 컴포넌트 참조

    // Start is called before the first frame update
    void Start()
    {
        // PlayerCondition 스크립트에게 이 UICondition 스크립트의 참조를 넘겨줌
        CharacterManager.Instance.Player.condition.uiCondition = this;
    }
}