using UnityEngine;

namespace AstronautPlayer
{
    public class AstronautPlayer : MonoBehaviour
    {
        private Animator anim;
        private CharacterController controller;

        public float moveSpeed = 6.0f;       // 이동 속도
        public float turnSpeed = 5.0f;       // 회전 속도
        public float gravity = 20.0f;        // 중력 값
        public float jumpForce = 8.0f;
        public float velocityY = 0.0f;       // y축 속도 (중력 + 점프)

        private Vector3 moveDirection = Vector3.zero; // 이동 방향

        void Start()
        {
            // CharacterController와 Animator 컴포넌트 가져오기
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
            // 이동 입력 확인
            float verticalInput = Input.GetAxis("Vertical");   // 앞뒤 이동
            float horizontalInput = Input.GetAxis("Horizontal"); // 좌우 이동

            // 카메라의 forward 방향을 기준으로 이동 방향 설정
            Vector3 forwardMovement = Camera.main.transform.forward * verticalInput;
            Vector3 rightMovement = Camera.main.transform.right * horizontalInput;

            // y축은 제외하고, 카메라의 방향에 맞게 이동
            forwardMovement.y = 0;
            rightMovement.y = 0;

            // 이동 방향 계산
            moveDirection = (forwardMovement + rightMovement).normalized * moveSpeed;

            // 이동 방향이 있으면 회전 (카메라의 바라보는 방향을 기준으로)
            if (moveDirection != Vector3.zero)
            {
                // 이동 방향으로 회전
                Quaternion toRotation = Quaternion.LookRotation(moveDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, turnSpeed * Time.deltaTime);
            }

            // 점프 입력 확인
            if (controller.isGrounded)
            {
                velocityY = -gravity * Time.deltaTime;  // 땅에 있을 때 중력 적용

                if (Input.GetKeyDown(KeyCode.Space)) // 스페이스바로 점프
                {
                    velocityY = jumpForce;
                }
            }
            else
            {
                velocityY -= gravity * Time.deltaTime; // 공중에 있을 때 중력 적용
            }


            moveDirection.y = velocityY;

            // 캐릭터 이동
            controller.Move(moveDirection * Time.deltaTime);

            // 애니메이션 설정
            if (verticalInput != 0 || horizontalInput != 0)
            {
                anim?.SetInteger("AnimationPar", 1); // 걷기 애니메이션
            }
            else
            {
                anim?.SetInteger("AnimationPar", 0); // 대기 애니메이션
            }
        }
    }
}
