using UnityEngine;
using UnityEngine.UI;

public class UI_HPBoss : MonoBehaviour
{
    public GameObject[] hearts;
    public GameObject heartsContainer; // Parent GameObject ของ hearts ทั้งหมด
    public Boss boss;
    private int playerCount = 0;

    private void Start()
    {
        if (boss != null)
            boss.enabled = false;

        // ซ่อน container แทนการซ่อนทีละหัวใจ
        if (heartsContainer != null)
            heartsContainer.SetActive(false);
    }

    void Update()
    {
        if (boss == null)
        {
            UpdateHearts(0);
            if (heartsContainer != null)
                heartsContainer.SetActive(false);
            return;
        }
        UpdateHearts(boss.hp);
    }

    void UpdateHearts(int currentHP)
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (hearts[i] == null) continue;
            hearts[i].SetActive(i < currentHP);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerCount++;
            if (playerCount == 1 && boss != null)
            {
                // เปิด container ก่อน แล้วค่อย update หัวใจ
                if (heartsContainer != null)
                    heartsContainer.SetActive(true);

                boss.enabled = true;
                UpdateHearts(boss.hp);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other == null) return;

        if (other.CompareTag("Player"))
        {
            playerCount--;
            if (playerCount <= 0)
            {
                playerCount = 0;

                // ซ่อน container
                if (heartsContainer != null)
                    heartsContainer.SetActive(false);

                if (boss != null)
                {
                    boss.enabled = false;
                    boss.hp = boss.maxHp;
                }
            }
        }
    }

    private void OnDestroy()
    {
        boss = null;
    }
}
