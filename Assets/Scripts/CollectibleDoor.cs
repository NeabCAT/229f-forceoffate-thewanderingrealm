using UnityEngine;

public class CollectibleDoor : MonoBehaviour
{
    public GameObject door;
    private bool isCollected = false;

    void Start()
    {
        if (door != null)
            door.SetActive(true);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!isCollected && other.CompareTag("Player"))
        {
            gameObject.SetActive(false);

            if (door != null)
                door.SetActive(false);

            isCollected = true;
        }
    }
}