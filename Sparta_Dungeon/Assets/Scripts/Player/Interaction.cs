using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interaction : MonoBehaviour
{
    public float checkRate = 0.05f; // 0.05�� �������� Ray�� üũ��
    private float lastCheckTime; // ���� ���������� üũ �ߴ���
    public float maxCheckDistance; //�ִ� ���� �Ÿ�
    public LayerMask layerMask;

    // ĳ��
    // �����ͳ� ���ҽ��� �ӽ� �����صξ��ٰ� ������ �ٽ� �ʿ��� �� �� ������ ȿ�������� �����ϴ� ���
    // ��ġ ���� ����ϴ� ������ ���� ��� ���� ���� �δ� �Ͱ� ���
    // ���� ����ߴ� ����� ĳ�ÿ� ������ ������ ����� �� �ٷ� ĳ�� ���� ������ ����Ͽ� �ӵ��� ����

    // ĳ���ϴ� �ڷᰡ �� �� ������ ����� ����
    public GameObject curInteractGameObject; // �÷��̾ ���� �ٶ󺸰� �ִ� ���� ������Ʈ ��ü�� ����
    private IInteractable curInteractable; // curInteractGameObject�� �پ��ִ� IInteractable �������̽� ������Ʈ�� ����

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
        // ���� ������ �����ϰ��� ���ǹ��� �޾Ƴ���
        if (Time.time - lastCheckTime > checkRate)
        {
            lastCheckTime = Time.time;

            // ī�޶� ȭ���� ���̿� �ʺ��� �߰� ��ġ���� Ray�� �߻� 
            Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
            RaycastHit hit; // ������Ʈ�� ������ ��Ƴ��� ��

            // ray�� ���� ������ �ִ�Ÿ� �ȿ� ���̾� ����ũ�� �ִ� ������Ʈ�� �浹�ϸ� hit�� ������ �Ѱ���
            if (Physics.Raycast(ray, out hit, maxCheckDistance, layerMask))
            {
                // �̹� ��ȣ�ۿ��ϰ� �ִ� ��ü�� ���ٸ�
                if (hit.collider.gameObject != curInteractGameObject)
                {
                    // �浹�ϰ� �ִ� ������Ʈ�� ���� ��ȣ�ۿ��ϴ� ������Ʈ�� �ٲ�
                    curInteractGameObject = hit.collider.gameObject;
                    // �浹�� ������Ʈ�� Collider ������Ʈ�� IInteractable �������̽��� ������
                    // ������Ʈ�� ������ curUnteractable ������ ���� / ������ null ��ȯ
                    curInteractable = hit.collider.GetComponent<IInteractable>();
                    SetPromptText();
                }
            }
            else // ��Ÿ� �ȿ� ������ �� null ó��
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
        // ȭ�鿡 ��ȣ�ۿ� ������Ʈ�� �������̰�, ��ȣ�ۿ� Ű(E)�� ������ ��
        if(context.phase == InputActionPhase.Started && curInteractable != null)
        {
            curInteractable.OnInteract(); // ������ ���� - ���� ���� ����
            curInteractGameObject = null;
            curInteractable = null;
            promptText.gameObject.SetActive(false);
        }
    }
}
