using JetBrains.Annotations;
using Oculus.Interaction.HandGrab;
using UnityEngine;

public class ToggleLock : MonoBehaviour
{
    [CanBeNull] public HandGrabInteractable handGrabInteractable;
    
    public void ToggleHandGrabState()
    {
        if (handGrabInteractable != null)
        {
            handGrabInteractable.enabled = !handGrabInteractable.enabled; // Toggle between true and false
        }
    }
}
