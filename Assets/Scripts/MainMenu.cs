using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEditor;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] public string gameSceneName;

    public GameObject ui_Comfirm;

    public Button yes;
    public Button no;

    private void Awake()
    {
        yes.onClick.AddListener(() =>
        {
#if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
#else
            Application.Quit();
#endif
        });

        no.onClick.AddListener(() =>
        {
            ui_Comfirm.SetActive(false);
        });
    }

        public void PlayGame()
    {
        ScreenFader.Instance.FadeToScene(gameSceneName);
    }
    public void Quit()
    {
        StartCoroutine(QuitRoutine());
    }

    IEnumerator QuitRoutine()
    {
        ui_Comfirm.SetActive(true);
        yield return null;
        //Application.Quit();
    }
}