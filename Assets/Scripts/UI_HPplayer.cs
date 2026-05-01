using UnityEngine;

public class UI_HPplayer : MonoBehaviour
{
    public Player player;
    public GameObject[] hearts;

    public static int savedHP = -1;

    void Start()
    {
        if (savedHP != -1)
            player.SetHealth(savedHP);

        savedHP = player.GetCurrentHealth();
        UpdateHearts();
    }

    void Update()
    {
        int currentHP = player.GetCurrentHealth();
        if (currentHP != savedHP)
        {
            savedHP = currentHP;
            UpdateHearts();
        }
    }

    void UpdateHearts()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].SetActive(i < savedHP);
        }
    }
}
