using UnityEngine;

public class MenuControls : MonoBehaviour
{
    bool isOpen = true;
    Canvas canvas;

    void Start() 
    {
        canvas = GetComponent<Canvas>();
        canvas.enabled = isOpen;
    }
    void Update()
    {
        ToggleMenu();
    }

    void ToggleMenu()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            isOpen = !isOpen;
            canvas.enabled = isOpen;
        }
        
    }
}
