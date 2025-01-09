using UnityEngine;

public class ThrowController : MonoBehaviour
{
    public Rigidbody rb;
    public float throwForce = 30f;
    public float arcHeight = 2f; // 포물선 최고 높이 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (rb == null)
        {
            Debug.LogError("Rigidbody 없음!");
        }
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

        rb.isKinematic = false;
        rb.linearVelocity = launchVelocity;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground")) 
        {
            Debug.Log("Cube가 착지했습니다");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // ���콺 Ŭ�� ����
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); 
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit)) 
            {
                Debug.Log("충돌 위치 " + hit.point);

                LaunchTowardsTarget(hit.point);
            }
        }
    }
}