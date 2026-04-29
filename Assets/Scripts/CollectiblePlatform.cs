using UnityEngine;

public class CollectiblePlatform : MonoBehaviour
{
    public GameObject platform;

    private bool isCollected = false;

    void Start()
    {
        if (platform != null)
            platform.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!isCollected && other.CompareTag("Player"))
        {
            gameObject.SetActive(false);

            if (platform != null)
                platform.SetActive(true);

            isCollected = true;
        }
    }
}