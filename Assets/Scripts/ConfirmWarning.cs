using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ConfirmWarning : MonoBehaviour
{
    public GameObject uiconfirm;

    public Button menu;
    public Button no;
    public Button confirm;

    private void Awake()
    {
        menu.onClick.AddListener(() =>
        {
            uiconfirm.SetActive(true);
        });

        no.onClick.AddListener(() =>
        {
            uiconfirm.SetActive(false);
        });

        confirm.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("MainMenu");
            Time.timeScale = 1f;
        });
    }
}
