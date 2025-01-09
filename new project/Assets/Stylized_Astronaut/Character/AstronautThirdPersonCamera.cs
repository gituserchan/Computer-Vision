using UnityEngine;

namespace AstronautThirdPersonCamera
{
    public class AstronautThirdPersonCamera : MonoBehaviour
    {
        private const float Y_ANGLE_MIN = -20.0f;
        private const float Y_ANGLE_MAX = 80.0f;

        public Transform player;
        public float distance = 5.0f;
        public float mouseSensitivity = 2.0f;

        private float currentX = 0.0f;
        private float currentY = 20.0f;

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void Update()
        {
            // ���콺 �Է����� ī�޶� ȸ��
            currentX += Input.GetAxis("Mouse X") * mouseSensitivity;
            currentY -= Input.GetAxis("Mouse Y") * mouseSensitivity;
            currentY = Mathf.Clamp(currentY, Y_ANGLE_MIN, Y_ANGLE_MAX);
        }

        private void LateUpdate()
        {
            Vector3 dir = new Vector3(0, 0, -distance);
            Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
            transform.position = player.position + rotation * dir;
            transform.LookAt(player.position);
        }
    }
}
