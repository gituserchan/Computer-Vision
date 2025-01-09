using UnityEngine;

public class FlipController : MonoBehaviour
{
    private Rigidbody rb;
    public float torqueMultiplier = 100f; // 회전력을 조정할 배율 여기 수정함 !!

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Rigidbody 컴포넌트를 가져옵니다.
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody가 Cube2에 추가되지 않았습니다!");
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        // 충돌한 오브젝트가 Cube1인지 확인
        if (collision.gameObject.name == "Cube1")
        {
            Debug.Log("Cube1이 Cube2를 내리쳤습니다!");

            // 충돌 지점의 충격량 계산
            Vector3 hitDirection = collision.contacts[0].point - transform.position;
            hitDirection = hitDirection.normalized;

            // Cube2에 회전 힘(Torque)을 가합니다.
            Vector3 torque = Vector3.Cross(Vector3.up, hitDirection) * torqueMultiplier;
            rb.AddTorque(torque, ForceMode.Impulse);

            // 뒤집힘 판정
            CheckFlipState();
        }
    }


    // Update is called once per frame
    void Update()
    {

    }
    void CheckFlipState()
    {
        // Cube2의 Up 벡터를 확인하여 뒤집힘 상태를 판정
        if (transform.up.y < 0)
        {
            Debug.Log("Cube2가 뒤집혔습니다!");
        }
    }
}
