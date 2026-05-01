using System.Collections;
using UnityEngine;

public class EndCredit : MonoBehaviour
{
    private Boss boss;
    public GameObject UI_endCredit;
    public RollingText rollingText;
    public GameObject UI_Hp;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        boss = FindFirstObjectByType<Boss>();
    }

    // Update is called once per frame
    void Update()
    {
        if (boss.hp <= 0)
        {
            StartCoroutine(EndCreditGame());
        }
    }

    IEnumerator EndCreditGame()
    {
        yield return new WaitForSeconds(1f);

        UI_endCredit.SetActive(true);
        UI_Hp.SetActive(false);
        rollingText.Rolling();
    }
}
