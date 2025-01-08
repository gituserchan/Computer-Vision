using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    private GuardController guardController;

    void Start()
    {
        guardController = FindObjectOfType<GuardController>();
        if (guardController == null)
        {
            Debug.LogError("GuardController를 찾을 수 없습니다!");
        }
    }

    public bool IsMoving()
    {
        return Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) ||
               Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow);
    }

    void Update()
    {
        if (guardController == null) return;

        if (!guardController.IsLookingBack() && IsMoving())
        {
            Debug.Log("게임 종료: 정면에서 움직임이 감지됨!");
            FindObjectOfType<GameManager>()?.GameOver(); 
            return; 
        }

        float horizontal = 0;
        float vertical = 0;

        if (Input.GetKey(KeyCode.UpArrow)) vertical = 1;
        if (Input.GetKey(KeyCode.DownArrow)) vertical = -1;
        if (Input.GetKey(KeyCode.LeftArrow)) horizontal = -1;
        if (Input.GetKey(KeyCode.RightArrow)) horizontal = 1;

        Vector3 movement = new Vector3(horizontal, 0, vertical).normalized;
        transform.Translate(movement * speed * Time.deltaTime);
    }
}
