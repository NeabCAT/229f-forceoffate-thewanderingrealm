using UnityEngine;

public class SignPop : MonoBehaviour
{
    public Animator anim;
    public GameObject textHint;
    public GameObject uiWindow;
    private bool canInteract = false;
    private bool isWindowOpen = false;

   

    void Start()
    {
        
        if (textHint != null) textHint.SetActive(false);
        if (uiWindow != null) uiWindow.SetActive(false);
    }

    void Update()
    {
        if (canInteract && Input.GetKeyDown(KeyCode.R))
        {
            ToggleWindow();
        }
    }

    void ToggleWindow()
    {
        isWindowOpen = !isWindowOpen; 

        if (isWindowOpen)
        {
            uiWindow.SetActive(true);          
            if (textHint != null) textHint.SetActive(false); 
            anim.SetBool("isPlayerNear", true); 
        }
        else
        {
            uiWindow.SetActive(false);          
            if (textHint != null) textHint.SetActive(true); 
            anim.SetBool("isPlayerNear", false); 
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canInteract = true;
            if (!isWindowOpen && textHint != null)
            {
                textHint.SetActive(true);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canInteract = false;
            isWindowOpen = false; 

            if (textHint != null) textHint.SetActive(false);
            if (uiWindow != null) uiWindow.SetActive(false);
            anim.SetBool("isPlayerNear", false);
        }
    }
}
