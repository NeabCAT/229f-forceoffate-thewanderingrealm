using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        ScreenFader.Instance.FadeToScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void Quit()
    {
        StartCoroutine(QuitRoutine());
    }

    IEnumerator QuitRoutine()
    {
        yield return new WaitForSecondsRealtime(1f);
        Debug.Log("Quit");
        Application.Quit();
    }
}
