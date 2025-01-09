using UnityEngine;

public class FlipController : MonoBehaviour
{
    private Rigidbody rb;
    public float torqueMultiplier = 100f; // ȸ������ ������ ���� ���� ������ !!

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Rigidbody ������Ʈ�� �����ɴϴ�.
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody�� Cube2�� �߰����� �ʾҽ��ϴ�!");
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        // �浹�� ������Ʈ�� Cube1���� Ȯ��
        if (collision.gameObject.name == "Cube1")
        {
            Debug.Log("Cube1�� Cube2�� �����ƽ��ϴ�!");

            // �浹 ������ ��ݷ� ���
            Vector3 hitDirection = collision.contacts[0].point - transform.position;
            hitDirection = hitDirection.normalized;

            // Cube2�� ȸ�� ��(Torque)�� ���մϴ�.
            Vector3 torque = Vector3.Cross(Vector3.up, hitDirection) * torqueMultiplier;
            rb.AddTorque(torque, ForceMode.Impulse);

            // ������ ����
            CheckFlipState();
        }
    }


    // Update is called once per frame
    void Update()
    {

    }
    void CheckFlipState()
    {
        // Cube2�� Up ���͸� Ȯ���Ͽ� ������ ���¸� ����
        if (transform.up.y < 0)
        {
            Debug.Log("Cube2�� ���������ϴ�!");
        }
    }
}
