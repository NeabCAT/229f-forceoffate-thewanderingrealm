using UnityEngine;
using UnityEngine.UI;

public class UI_HPBoss : MonoBehaviour
{
    public Slider hpSlider;
    public Boss boss;
    private int playerCount = 0;

    private void Start()
    {
        if (boss != null)
        {
            boss.enabled = false;
            hpSlider.maxValue = boss.maxHp;
        }
        hpSlider.gameObject.SetActive(false);
    }

    void Update()
    {
        if (boss == null)
        {
            if (hpSlider != null) hpSlider.value = 0;
            return;
        }

        hpSlider.value = boss.hp;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerCount++;
            if (playerCount == 1)
            {
                if (hpSlider != null) hpSlider.gameObject.SetActive(true);
                if (boss != null) boss.enabled = true;
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerCount--;
            if (playerCount <= 0)
            {
                playerCount = 0;
                if (hpSlider != null) hpSlider.gameObject.SetActive(false);

                if (boss != null)
                {
                    boss.enabled = false;
                    boss.hp = boss.maxHp;
                }
            }
        }
    }
}
