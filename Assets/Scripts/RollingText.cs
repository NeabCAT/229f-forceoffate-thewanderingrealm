using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RollingText : MonoBehaviour
{

    public float scollSpeed = 40f;
    private RectTransform rectTransform;
    private bool isRolling = false;
    public string mainMenuScene = "MainMenu";

    public void Show()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Time.timeScale = 0f;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isRolling)
        {
            rectTransform.anchoredPosition += new Vector2(0, scollSpeed * Time.deltaTime);
            StartCoroutine(ToMainMenu());
        }
    }

    public void Rolling()
    {
        isRolling = true;
    }

    IEnumerator ToMainMenu()
    {
        yield return new WaitForSecondsRealtime(50f);

        isRolling = false;

        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        SceneManager.LoadScene(mainMenuScene);
    }
}
