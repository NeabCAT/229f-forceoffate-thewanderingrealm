using UnityEngine;
using UnityEngine.UI;

public class TriggerTeach : MonoBehaviour
{
    public GameObject uiTeach;
    public Button buttonBack;

    private bool hasShown = false;

    private void Awake()
    {
        buttonBack.onClick.AddListener(() =>
        {
            Time.timeScale = 1f;
            uiTeach.SetActive(false);
        });
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !hasShown)
        {
            Time.timeScale = 0;
            hasShown = true;
            uiTeach.SetActive(true);
        }
    }
}
