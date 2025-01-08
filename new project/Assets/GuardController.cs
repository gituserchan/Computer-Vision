using UnityEngine;
using System.Collections;
using TMPro;

public class GuardController : MonoBehaviour
{
    public Animator animator; 
    private bool isLookingBack = false; 
    public TextMeshProUGUI statusText; 

    void Start()
    {
        StartCoroutine(GameLoop());
    }

    IEnumerator GameLoop()
    {
        while (true)
        {
            yield return StartCoroutine(CountdownToChange("정면 바라보기", false));
            isLookingBack = false;
            UpdateStatusText("정면 바라보기!");
            animator?.SetTrigger("LookFront");
            yield return new WaitForSeconds(Random.Range(3, 7)); 

            yield return StartCoroutine(CountdownToChange("뒤를 바라보기", true));
            isLookingBack = true;
            UpdateStatusText("뒤를 바라보기!");
            animator?.SetTrigger("LookBack");
            yield return new WaitForSeconds(Random.Range(3, 7)); 
        }
    }

    IEnumerator CountdownToChange(string nextState, bool nextIsLookingBack)
    {
        for (int i = 3; i > 0; i--)
        {
            Debug.Log($"{nextState} {i}초 전");
            yield return new WaitForSeconds(1f); 
        }
    }

    public bool IsLookingBack()
    {
        return isLookingBack;
    }

    private void UpdateStatusText(string status)
    {
        if (statusText != null)
        {
            statusText.text = status; 
        }
        Debug.Log("Guard 상태: " + status); 
    }
}
