using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Option_ESC : MonoBehaviour
{
    public GameObject uiOption;
    public GameObject uiESC;

    public Slider Music_Vol;
    public Slider SFX_Vol;
    public AudioMixer mainAuio;

    void Update()
    {
        ESCOpen();
    }

    public void Option()
    {
        uiOption.SetActive(true);
    }

    public void Back()
    {
        Time.timeScale = 1f;
        uiOption.SetActive(false);
    }

    public void ChangeMusicVolume()
    {
        float dB = Mathf.Log10(Mathf.Max(0.0001f, Music_Vol.value)) * 20;
        mainAuio.SetFloat("Music_Vol", dB);
    }
    public void ChangeSFXVolume()
    {
        float dB = Mathf.Log10(Mathf.Max(0.0001f, SFX_Vol.value)) * 20;
        mainAuio.SetFloat("SFX_Vol", dB);
    }

    public void ESCOpen()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 0f;
            uiESC.SetActive(true);
        }
        
    }

    public void ESCBack()
    {
        Time.timeScale = 1f;
        uiESC.SetActive(false);
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

}
