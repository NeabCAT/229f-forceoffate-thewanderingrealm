using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject uiOption;
    public GameObject uiLoadGame;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Option()
    {
        Time.timeScale = 0f;
        uiOption.SetActive(true);
    }

    public void Back()
    {
        uiOption.SetActive(false);
        Time.timeScale = 1f;
    }

    public void Quit()
    {
        StartCoroutine(QuitRoutine());
    }

    IEnumerator QuitRoutine()
    {
        yield return new WaitForSecondsRealtime(1f);
        Application.Quit();
    }
}
