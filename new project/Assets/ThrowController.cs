using UnityEngine;

public class ThrowController : MonoBehaviour
{
    public Rigidbody rb; // Cube1의 Rigidbody
    public float throwForce = 30f;
    public float arcHeight = 2f; // 포물선 최고 높이
    private bool isHolding = false; // Cube1을 잡고 있는지 여부
    private Transform holdPoint; // 잡았을 때 Cube1의 위치

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (rb == null)
        {
            Debug.LogError("Rigidbody 없음!");
        }

        // 캐릭터의 손 위치를 기준으로 할 점(이 위치는 임의로 생성 가능)
        holdPoint = new GameObject("HoldPoint").transform;
        holdPoint.SetParent(transform); // 플레이어(캐릭터)에 붙임
        holdPoint.localPosition = new Vector3(0, 1.5f, 1); // 캐릭터 기준 위치 조정
    }
    void LaunchTowardsTarget(Vector3 targetPosition)
    {
        Vector3 direction = targetPosition - transform.position;
        float horizontalDistance = new Vector3(direction.x, 0, direction.z).magnitude;
        float verticalDistance = direction.y;

        float velocityY = Mathf.Sqrt(-2 * Physics.gravity.y * arcHeight);
        float timeToApex = Mathf.Abs(velocityY / Physics.gravity.y);
        float totalTime = timeToApex + Mathf.Sqrt((2 * (verticalDistance + arcHeight)) / Mathf.Abs(Physics.gravity.y));
        float velocityXZ = horizontalDistance / totalTime;

        Vector3 launchVelocity = new Vector3(direction.x, 0, direction.z).normalized * velocityXZ;
        launchVelocity.y = velocityY;

        rb.linearVelocity = launchVelocity; // 물체를 던짐
        Debug.Log("Cube1을 던졌습니다!");
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground")) 
        {
            Debug.Log("Cube가 착지했습니다");
        }
    }
    void TryPickUp()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.CompareTag("Cube1")) // Cube1 태그 확인
            {
                // Cube1 잡기
                isHolding = true;
                rb.isKinematic = true; // 물리 반응 끔
                rb.transform.position = holdPoint.position; // HoldPoint 위치로 이동
                rb.transform.SetParent(holdPoint); // 플레이어에 붙임
                Debug.Log("Cube1을 잡았습니다!");
            }
        }
    }

    void DropObject()
    {
        if (isHolding)
        {
            isHolding = false;
            rb.isKinematic = false; // 물리 반응 재활성화
            rb.transform.SetParent(null); // 부모에서 분리
            Debug.Log("Cube1을 놓았습니다!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // E 키로 Cube1 잡기
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

        // 마우스 왼쪽 클릭으로 던지기
        if (isHolding && Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log("던질 위치: " + hit.point);

                LaunchTowardsTarget(hit.point);
                DropObject(); // 던지고 나면 놓음
            }
        }

    }
}