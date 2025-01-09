using UnityEngine;

public class ThrowController : MonoBehaviour
{
    public Rigidbody rb; // Cube1의 Rigidbody
    public float throwForce = 30f; // 던질 힘
    public float arcHeight = 2f; // 포물선 최고 높이
    private bool isHolding = false; // Cube1을 잡고 있는지 여부
    private Transform holdPoint; // 잡았을 때 Cube1의 위치
    private Vector3 targetPosition; // 마우스로 조준한 목표 위치

    // Start is called before the first frame update
    void Start()
    {
        if (rb == null)
        {
            Debug.LogError("Rigidbody 없음!");
        }

        // 캐릭터의 손 위치를 기준으로 할 점(이 위치는 임의로 생성 가능)
        holdPoint = new GameObject("HoldPoint").transform;
        holdPoint.SetParent(transform); // 플레이어(캐릭터)에 붙임
        holdPoint.localPosition = new Vector3(0.5f, 1.2f, 0.5f); // 캐릭터 기준 위치 조정
        Debug.Log($"[Start] HoldPoint 초기 위치: {holdPoint.localPosition}");

        // 초기 목표 위치를 설정
        targetPosition = transform.position + transform.forward * 1f; // 플레이어 전방 기본 위치
    }

    // 마우스를 사용하여 목표 위치를 설정
    void SetTargetPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f)) // Ray의 최대 거리 100
        {
            targetPosition = hit.point; // Ray가 맞은 지점으로 설정
            Debug.Log($"[SetTargetPosition] 목표 위치(Raycast): {targetPosition}");
        }
        else
        {
            // Ray가 아무것도 맞히지 못한 경우, 기본 목표 지점 설정
            targetPosition = transform.position + transform.forward * 1f;
            Debug.Log("[SetTargetPosition] Ray가 맞은 지점 없음. 기본 위치로 설정.");
        }
    }

    // 스페이스바로 던지기
    void LaunchTowardsTarget()
    {
        Vector3 startPoint = holdPoint.position;

        // 목표 지점 보정: Y값을 holdPoint와 동일한 높이로 설정
        Vector3 correctedTargetPosition = new Vector3(targetPosition.x, startPoint.y, targetPosition.z);

        // 디버깅: 보정된 목표 위치 확인
        Debug.Log($"[LaunchTowardsTarget] 보정된 목표 위치: {correctedTargetPosition}");

        Vector3 direction = correctedTargetPosition - startPoint;

        // 던지기 방향 및 속도 계산
        float horizontalDistance = new Vector3(direction.x, 0, direction.z).magnitude;
        float verticalDistance = correctedTargetPosition.y - startPoint.y;

        // 포물선 속도 계산
        float velocityY = Mathf.Sqrt(-2 * Physics.gravity.y * arcHeight); // Y축 속도
        float timeToApex = Mathf.Abs(velocityY / Physics.gravity.y); // 최고점까지 시간
        float totalTime = timeToApex + Mathf.Sqrt((2 * (verticalDistance + arcHeight)) / Mathf.Abs(Physics.gravity.y)); // 총 비행 시간
        float velocityXZ = horizontalDistance / totalTime;

        Vector3 launchVelocity = new Vector3(direction.x, 0, direction.z).normalized * velocityXZ;
        launchVelocity.y = velocityY;

        // Rigidbody에 속도 적용
        rb.isKinematic = false;
        rb.linearVelocity = launchVelocity;

        Debug.Log($"[LaunchTowardsTarget] 던진 속도: {launchVelocity}");
    }


    // Cube1 잡기 시도
    void TryPickUp()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f)) // Ray 거리 제한
        {
            if (hit.collider.CompareTag("Cube1")) // Cube1 태그 확인
            {
                // Cube1 잡기
                isHolding = true;
                rb.isKinematic = true; // 물리 반응 끔
                rb.transform.position = holdPoint.position; // HoldPoint 위치로 이동
                rb.transform.SetParent(holdPoint); // 플레이어에 붙임
                Debug.Log("[TryPickUp] Cube1을 잡았습니다!");
            }
        }
        else
        {
            Debug.Log("[TryPickUp] Ray가 아무것도 맞히지 않았습니다.");
        }
    }

    // Cube1 놓기
    void DropObject()
    {
        if (isHolding)
        {
            isHolding = false;
            rb.isKinematic = false; // 물리 반응 재활성화
            rb.transform.SetParent(null); // 부모에서 분리
            Debug.Log("[DropObject] Cube1을 놓았습니다!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // E 키로 Cube1 잡기 또는 놓기
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!isHolding)
            {
                TryPickUp();
            }
            else
            {
                DropObject();
            }
        }

        // 마우스 왼쪽 버튼으로 목표 위치 설정
        if (isHolding && Input.GetMouseButtonDown(0)) // 왼쪽 클릭
        {
            SetTargetPosition(); // 마우스 조준 위치 설정
        }

        // 스페이스바로 던지기
        if (isHolding && Input.GetKeyDown(KeyCode.F))
        {
            LaunchTowardsTarget(); // 설정된 목표로 던지기
            DropObject(); // 던지고 나면 놓음
        }
    }
}
