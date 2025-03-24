using UnityEngine;
using UnityEngine.UI;

public class RootMotionButton : MonoBehaviour
{
    public RootMotionToggle rootMotionScript;
    public Button toggleButton;

    void Start()
    {
        toggleButton.onClick.AddListener(ToggleRootMotion);
    }

    void ToggleRootMotion()
    {
        rootMotionScript.ToggleRunningMode();
    }
}