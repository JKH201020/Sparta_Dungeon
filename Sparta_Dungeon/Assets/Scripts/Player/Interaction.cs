using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interaction : MonoBehaviour
{
    public float checkRate = 0.05f; // 0.05초 간격으로 Ray를 체크함
    private float lastCheckTime; // 언제 마지막으로 체크 했는지
    public float maxCheckDistance; //최대 감지 거리
    public LayerMask layerMask;

    // 캐싱
    // 데이터나 리소스를 임시 저장해두었다가 다음에 다시 필요할 때 더 빠르고 효율적으로 접근하는 방식
    // 마치 자주 사용하는 물건을 손이 닿기 쉬운 곳에 두는 것과 비슷
    // 전에 사용했던 결과를 캐시에 저장해 다음에 사용할 때 바로 캐시 값을 가져와 사용하여 속도가 빠름

    // 캐싱하는 자료가 이 두 변수에 담겨져 있음
    public GameObject curInteractGameObject; // 플레이어가 현재 바라보고 있는 게임 오브젝트 자체를 저장
    private IInteractable curInteractable; // curInteractGameObject에 붙어있는 IInteractable 인터페이스 컴포넌트를 저장

    public TextMeshProUGUI promptText;
    private Camera camera;

    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        // 무한 실행을 방지하고자 조건문을 달아놓음
        if (Time.time - lastCheckTime > checkRate)
        {
            lastCheckTime = Time.time;

            // 카메라 화면의 높이와 너비의 중간 위치에서 Ray를 발사 
            Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
            RaycastHit hit; // 오브젝트의 정보를 담아놓을 곳

            // ray의 지정 값으로 최대거리 안에 레이어 마스크가 있는 오브젝트와 충돌하면 hit에 정보를 넘겨줌
            if (Physics.Raycast(ray, out hit, maxCheckDistance, layerMask))
            {
                // 이미 상호작용하고 있는 물체가 없다면
                if (hit.collider.gameObject != curInteractGameObject)
                {
                    // 충돌하고 있는 오브젝트를 현재 상호작용하는 오브젝트로 바꿈
                    curInteractGameObject = hit.collider.gameObject;
                    // 충돌한 오브젝트의 Collider 컴포넌트에 IInteractable 인터페이스를 구현한
                    // 컴포넌트가 있으면 curUnteractable 변수에 저장 / 없음면 null 반환
                    curInteractable = hit.collider.GetComponent<IInteractable>();
                    SetPromptText();
                }
            }
            else // 사거리 안에 없으면 다 null 처리
            {
                curInteractGameObject = null;
                curInteractable = null;
                promptText.gameObject.SetActive(false);
            }
        }
    }

    private void SetPromptText()
    {
        promptText.gameObject.SetActive(true);
        promptText.text = curInteractable.GetInteractPrompt();
    }

    public void OnInteractInput(InputAction.CallbackContext context)
    {
        // 화면에 상호작용 오브젝트를 감지중이고, 상호작용 키(E)를 눌렀을 때
        if(context.phase == InputActionPhase.Started && curInteractable != null)
        {
            curInteractable.OnInteract(); // 아이템 저장 - 각종 정보 제거
            curInteractGameObject = null;
            curInteractable = null;
            promptText.gameObject.SetActive(false);
        }
    }
}
