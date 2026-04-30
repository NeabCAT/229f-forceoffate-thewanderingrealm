using UnityEngine;
using UnityEngine.UI;

public class UI_HPBoss : MonoBehaviour
{
    public Slider hpSlider;
    public Boss boss;
    private int playerCount = 0;

    private void Start()
    {
        boss.enabled = false;
        hpSlider.gameObject.SetActive(false);
    }

    void Update()
    {
        if (boss == null) return;
        hpSlider.maxValue = boss.maxHp;
        hpSlider.value = boss.hp;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerCount++;

            if (playerCount == 1)
            {
                hpSlider.gameObject.SetActive(true);
                boss.enabled = true;
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

                hpSlider.gameObject.SetActive(false);
                boss.enabled = false;
                boss.hp = boss.maxHp;
            }
        }
    }
}
