using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScreenFader : MonoBehaviour
{
    public static ScreenFader Instance;

    [Header("Fade Settings")]
    public Image fadePanel;          // ลาก FadePanel มาใส่ตรงนี้
    public float fadeDuration = 1f;

    void Awake()
    {
        // Singleton ให้เรียกใช้จาก script อื่นได้ง่าย
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Fade ออก แล้วโหลด Scene
    public void FadeToScene(int sceneIndex)
    {
        StartCoroutine(FadeOutAndLoad(sceneIndex));
    }

    IEnumerator FadeOutAndLoad(int sceneIndex)
    {
        // Fade to black
        yield return StartCoroutine(Fade(0f, 1f));

        // โหลด Scene
        SceneManager.LoadScene(sceneIndex);

        // Fade กลับมา
        yield return StartCoroutine(Fade(1f, 0f));
    }

    IEnumerator Fade(float startAlpha, float endAlpha)
    {
        fadePanel.gameObject.SetActive(true);

        float elapsed = 0f;
        Color color = fadePanel.color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            color.a = Mathf.Lerp(startAlpha, endAlpha, elapsed / fadeDuration);
            fadePanel.color = color;
            yield return null;
        }

        color.a = endAlpha;
        fadePanel.color = color;

        // ซ่อน panel ถ้า fade จบแล้ว (alpha = 0)
        if (endAlpha == 0f)
            fadePanel.gameObject.SetActive(false);
    }
}