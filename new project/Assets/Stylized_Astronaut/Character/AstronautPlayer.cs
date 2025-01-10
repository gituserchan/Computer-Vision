using UnityEngine;

namespace AstronautPlayer
{
    public class AstronautPlayer : MonoBehaviour
    {
        private Animator anim;
        private CharacterController controller;

        public float moveSpeed = 6.0f;       // �̵� �ӵ�
        public float turnSpeed = 5.0f;       // ȸ�� �ӵ�
        public float gravity = 20.0f;        // �߷� ��
        public float jumpForce = 8.0f;
        public float velocityY = 0.0f;       // y�� �ӵ� (�߷� + ����)

        private Vector3 moveDirection = Vector3.zero; // �̵� ����

        void Start()
        {
            // CharacterController�� Animator ������Ʈ ��������
            controller = GetComponent<CharacterController>();
            if (controller == null)
            {
                Debug.LogError("CharacterController component is missing on this GameObject.");
            }

            anim = GetComponentInChildren<Animator>();
            if (anim == null)
            {
                Debug.LogError("Animator component is missing on the child object.");
            }
        }

        void Update()
        {
            // �̵� �Է� Ȯ��
            float verticalInput = Input.GetAxis("Vertical");   // �յ� �̵�
            float horizontalInput = Input.GetAxis("Horizontal"); // �¿� �̵�

            // ī�޶��� forward ������ �������� �̵� ���� ����
            Vector3 forwardMovement = Camera.main.transform.forward * verticalInput;
            Vector3 rightMovement = Camera.main.transform.right * horizontalInput;

            // y���� �����ϰ�, ī�޶��� ���⿡ �°� �̵�
            forwardMovement.y = 0;
            rightMovement.y = 0;

            // �̵� ���� ���
            moveDirection = (forwardMovement + rightMovement).normalized * moveSpeed;

            // �̵� ������ ������ ȸ�� (ī�޶��� �ٶ󺸴� ������ ��������)
            if (moveDirection != Vector3.zero)
            {
                // �̵� �������� ȸ��
                Quaternion toRotation = Quaternion.LookRotation(moveDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, turnSpeed * Time.deltaTime);
            }

            // ���� �Է� Ȯ��
            if (controller.isGrounded)
            {
                velocityY = -gravity * Time.deltaTime;  // ���� ���� �� �߷� ����

                if (Input.GetKeyDown(KeyCode.Space)) // �����̽��ٷ� ����
                {
                    velocityY = jumpForce;
                }
            }
            else
            {
                velocityY -= gravity * Time.deltaTime; // ���߿� ���� �� �߷� ����
            }


            moveDirection.y = velocityY;

            // ĳ���� �̵�
            controller.Move(moveDirection * Time.deltaTime);

            // �ִϸ��̼� ����
            if (verticalInput != 0 || horizontalInput != 0)
            {
                anim?.SetInteger("AnimationPar", 1); // �ȱ� �ִϸ��̼�
            }
            else
            {
                anim?.SetInteger("AnimationPar", 0); // ��� �ִϸ��̼�
            }
        }
    }
}
