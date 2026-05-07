using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Option_ESC : MonoBehaviour
{
    public GameObject uiOption;
    public GameObject uiESC;

    public Slider Music_Vol;
    public Slider SFX_Vol;
    public AudioMixer mainAuio;

    public GameObject uiHp;
    public GameObject logo;
    public TextMeshProUGUI nameGame;

    public Toggle musicToggle;
    public Toggle sfxToggle;

    float lastMusicVolume;
    float lastSFXVolume;

    public TextMeshProUGUI musicPercentText;
    public TextMeshProUGUI sfxPercentText;

    void Start()
    {
        float music = PlayerPrefs.GetFloat("Music_Vol", 1f);
        float sfx = PlayerPrefs.GetFloat("SFX_Vol", 1f);

        Music_Vol.value = music;
        SFX_Vol.value = sfx;

        float musicdB = Mathf.Log10(Mathf.Max(0.0001f, music)) * 20;
        float sfxdB = Mathf.Log10(Mathf.Max(0.0001f, sfx)) * 20;

        mainAuio.SetFloat("Music_Vol", musicdB);
        mainAuio.SetFloat("SFX_Vol", sfxdB);

        musicPercentText.text = Mathf.RoundToInt(music * 100f) + "%";
        sfxPercentText.text = Mathf.RoundToInt(sfx * 100f) + "%";
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ESCOpen();
        }
    }

    public void Option()
    {
        if (uiOption != null) uiOption.SetActive(true);
        if (logo != null) logo.SetActive(false);
        if (nameGame != null) nameGame.gameObject.SetActive(false);
    }

    public void Back()
    {
        Time.timeScale = 1f;
        if (uiOption != null) uiOption.SetActive(false);
        if (logo != null) logo.SetActive(true);
        if (nameGame != null) nameGame.gameObject.SetActive(true);
    }

    public void ChangeMusicVolume()
    {
        float value = Music_Vol.value;

        float dB = Mathf.Log10(Mathf.Max(0.0001f, value)) * 20;
        mainAuio.SetFloat("Music_Vol", dB);

        PlayerPrefs.SetFloat("Music_Vol", value);
        musicPercentText.text = Mathf.RoundToInt(value * 100f) + "%";
    }
    public void ChangeSFXVolume()
    {
        float value = SFX_Vol.value;

        float dB = Mathf.Log10(Mathf.Max(0.0001f, SFX_Vol.value)) * 20;
        mainAuio.SetFloat("SFX_Vol", dB);

        PlayerPrefs.SetFloat("SFX_Vol", value);
        sfxPercentText.text = Mathf.RoundToInt(value * 100f) + "%";
    }

    public void ESCOpen()
    {
        if (uiESC == null) return;
        Time.timeScale = 0f;
        uiESC.SetActive(true);
        if (uiHp != null) uiHp.SetActive(false);
    }

    public void ESCBack()
    {
        Time.timeScale = 1f;
        if (uiESC != null) uiESC.SetActive(false);
        if (uiHp != null) uiHp.SetActive(true);
    }


    //public void BackToMainMenu()
    //{
    //    Time.timeScale = 1f;
    //    SceneManager.LoadScene("MainMenu");
    //}

    public void ToggleMusic()
    {
        if (musicToggle.isOn)
        {
            lastMusicVolume = Music_Vol.value;
            musicPercentText.text = "0%";

            Music_Vol.SetValueWithoutNotify(0);
            mainAuio.SetFloat("Music_Vol", -80f);
        }
        else
        {
            Music_Vol.SetValueWithoutNotify(lastMusicVolume);

            ChangeMusicVolume();
        }
    }

    public void Togglesfx()
    {
        if (sfxToggle.isOn)
        {
            lastSFXVolume = SFX_Vol.value;
            sfxPercentText.text = "0%";

            SFX_Vol.SetValueWithoutNotify(0);
            mainAuio.SetFloat("SFX_Vol", -80f);
        }
        else
        {
            SFX_Vol.SetValueWithoutNotify(lastSFXVolume);

            ChangeSFXVolume();
        }
    }
}
