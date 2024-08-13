using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DistanceDisplay : MonoBehaviour
{
    public Text distanceText; // Reference to the Text component
    private float distance;

    // Method to update the distance
    public void UpdateDistance(float newDistance)
    {
        distance = newDistance;
        distanceText.text = "Stride Length: " + distance.ToString("F2") + " m";
    }
}
// using UnityEngine;
// using UnityEngine.UI;

// public class DistanceDisplay : MonoBehaviour
// {
//     public Text distanceText; // Reference to the Text component
//     public Text timeText;     // Reference to another Text component for time

//     // Method to update the distance
//     public void UpdateDistance(float newDistance)
//     {
//         distanceText.text = "Distance: " + newDistance.ToString("F2") + " meters";
//     }

//     // Method to update the contact time
//     public void UpdateTime(float contactTime)
//     {
//         timeText.text = "Contact Time: " + contactTime.ToString("F2") + " seconds";
//     }
// }
