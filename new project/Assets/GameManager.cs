using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public PlayerController player; 
    public GuardController guard;  
    public TextMeshProUGUI gameOverText; 

    private bool isGameOver = false;

    void Update()
    {
        if (isGameOver) return;

        if (!guard.IsLookingBack() && player.IsMoving())
        {
            Debug.Log("눈사람이 정면을 보고 있습니다. 플레이어 움직임 감지됨!");
            GameOver();
        }
    }

    public void GameOver()
    {
        isGameOver = true;
        Debug.Log("게임 종료: You Die");

        if (gameOverText != null)
        {
            gameOverText.text = "You Die";
            gameOverText.gameObject.SetActive(true);
        }

        Time.timeScale = 0f; 
    }
}
