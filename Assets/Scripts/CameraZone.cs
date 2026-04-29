using UnityEngine;

public class CameraZone : MonoBehaviour
{
    [Header("Camera Settings for this Zone")]
    public float targetOrthographicSize = 5f;
    public Vector2 offset = new Vector2(0f, 2f);

    void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player == null) return;


        Collider2D zone = GetComponent<Collider2D>();
        if (zone != null && zone.bounds.Contains(player.transform.position))
        {
            if (Camera.main == null) return; 
            CameraFollow cam = Camera.main.GetComponent<CameraFollow>();
            if (cam != null)
                cam.SnapToZone(targetOrthographicSize, offset);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        CameraFollow cam = Camera.main.GetComponent<CameraFollow>();
        if (cam != null)
        {
            cam.SetZone(targetOrthographicSize, offset);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        CameraZone[] allZones = FindObjectsByType<CameraZone>(FindObjectsSortMode.None);
        foreach (CameraZone zone in allZones)
        {
            if (zone == this) continue;
            Collider2D col = zone.GetComponent<Collider2D>();
            if (col != null && col.bounds.Contains(other.transform.position))
            {
                CameraFollow cam = Camera.main.GetComponent<CameraFollow>();
                if (cam != null)
                    cam.SetZone(zone.targetOrthographicSize, zone.offset);
                return;
            }
        }

        CameraFollow cam2 = Camera.main.GetComponent<CameraFollow>();
        if (cam2 != null)
            cam2.ResetZone();
    }


}