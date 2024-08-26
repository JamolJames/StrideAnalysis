// using UnityEngine;
// using System.Collections;

// public class StrideVisualizer : MonoBehaviour
// {
//     public PoolManager poolManager;
//     public DistanceDisplay distanceDisplay; // Reference to the DistanceDisplay script

//     public ContactTimeDisplay contactDisplay; // Reference to the contactTimeDisplay script
//     public float spawnOffset = 0.01f; // Small offset to prevent immediate collision with the plane
//     public float collisionCooldown = 0.1f; // Time in seconds to wait before allowing another collision
//     public Color measurementColor = Color.red; // Color for measuring cubes
//     public float colorChangeDuration = 0.5f; // Duration to keep the measurement color

//     private float lastCollisionTime;
//     private Vector3 lastCollisionPosition;
//     private bool hasLastCollision = false;
//     private GameObject lastMeasuredCube;

//     private void OnCollisionEnter(Collision collision)
//     {
//         // Check if the script is enabled and active
//         if (!this.enabled)
//             return;

//         // Ensure enough time has passed since the last collision
//         if (Time.time - lastCollisionTime < collisionCooldown)
//             return;

//         // Update the last collision time
//         lastCollisionTime = Time.time;

//         // Check if the collision is with the foot
//         if (collision.gameObject.CompareTag("Foot"))
//         {
//             Debug.Log("Collision detected with foot!");

//             // Iterate through all the contact points
//             foreach (ContactPoint contact in collision.contacts)
//             {
//                 Debug.Log("Contact point: " + contact.point);

//                 // Calculate distance from the last collision point
//                 if (hasLastCollision)
//                 {
//                     float distance = Vector3.Distance(lastCollisionPosition, contact.point);
//                     Debug.Log("Distance from last collision: " + distance + " meters");

//                     // Update the distance on the screen
//                     distanceDisplay.UpdateDistance(distance);

//                     // Change the color of the cube for measurement
//                     if (lastMeasuredCube != null)
//                     {
//                         StartCoroutine(ResetCubeColor(lastMeasuredCube));
//                     }
//                 }

//                 // Store the current collision point as the last collision point
//                 lastCollisionPosition = contact.point;
//                 hasLastCollision = true;

//                 // Apply a small offset to the contact point
//                 Vector3 spawnPosition = contact.point + contact.normal * spawnOffset;

//                 // Get a pooled object and activate it at the collision point
//                 GameObject square = poolManager.GetPooledObject();
//                 square.transform.position = spawnPosition;
//                 square.transform.rotation = Quaternion.identity;
//                 square.SetActive(true);

//                 // Change the color of the current cube for measurement
//                 ChangeCubeColor(square);
//                 lastMeasuredCube = square;
//             }
//         }
//     }

//     private void ChangeCubeColor(GameObject cube)
//     {
//         Renderer renderer = cube.GetComponent<Renderer>();
//         if (renderer != null)
//         {
//             renderer.material.color = measurementColor;
//         }
//     }

//     private IEnumerator ResetCubeColor(GameObject cube)
//     {
//         yield return new WaitForSeconds(colorChangeDuration);
//         Renderer renderer = cube.GetComponent<Renderer>();
//         if (renderer != null)
//         {
//             renderer.material.color = Color.white; // Assuming the default color is white
//         }
//     }
// }


using UnityEngine;
using System.Collections;

public class StrideVisualizer : MonoBehaviour
{
    public PoolManager poolManager;
    public DistanceDisplay distanceDisplay; // Reference to the DistanceDisplay script
    public ContactTimeDisplay contactDisplay; // Reference to the ContactTimeDisplay script
    public float spawnOffset = 0.01f; // Small offset to prevent immediate collision with the plane
    public float collisionCooldown = 0.1f; // Time in seconds to wait before allowing another collision
    public Color measurementColor = Color.red; // Color for measuring cubes
    public float colorChangeDuration = 0.5f; // Duration to keep the measurement color

    private float lastCollisionTime;
    private Vector3 lastCollisionPosition;
    private bool hasLastCollision = false;
    private GameObject lastMeasuredCube;

    private float contactStartTime;
    private bool isFootInContact = false;

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the script is enabled and active
        if (!this.enabled)
            return;

        // Ensure enough time has passed since the last collision
        if (Time.time - lastCollisionTime < collisionCooldown)
            return;

        // Update the last collision time
        lastCollisionTime = Time.time;

        // Check if the collision is with the foot
        if (collision.gameObject.CompareTag("Foot"))
        {
            // Debug.Log("Collision detected with foot!");

            // Start measuring contact time
            contactStartTime = Time.time;
            isFootInContact = true;

            // Iterate through all the contact points
            foreach (ContactPoint contact in collision.contacts)
            {
                // Debug.Log("Contact point: " + contact.point);

                // Calculate distance from the last collision point
                if (hasLastCollision)
                {
                    float distance = Vector3.Distance(lastCollisionPosition, contact.point);
                    // Debug.Log("Distance from last collision: " + distance + " meters");

                    // Update the distance on the screen
                    distanceDisplay.UpdateDistance(distance);

                    // Change the color of the cube for measurement
                    if (lastMeasuredCube != null)
                    {
                        StartCoroutine(ResetCubeColor(lastMeasuredCube));
                    }
                }

                // Store the current collision point as the last collision point
                lastCollisionPosition = contact.point;
                hasLastCollision = true;

                // Apply a small offset to the contact point
                Vector3 spawnPosition = contact.point + contact.normal * spawnOffset;

                // Get a pooled object and activate it at the collision point
                GameObject square = poolManager.GetPooledObject();
                square.transform.position = spawnPosition;
                square.transform.rotation = Quaternion.identity;
                square.SetActive(true);

                // Change the color of the current cube for measurement
                ChangeCubeColor(square);
                lastMeasuredCube = square;
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // Check if the collision is with the foot
        if (collision.gameObject.CompareTag("Foot") && isFootInContact)
        {
            // Stop measuring contact time
            float contactEndTime = Time.time;
            float contactDuration = contactEndTime - contactStartTime;
            isFootInContact = false;

            // Display the contact duration
            // Debug.Log("Foot contact duration: " + contactDuration + " seconds");
            contactDisplay.UpdateContactTime(contactDuration); // Assuming you have this method in your ContactTimeDisplay script
        }
    }

    private void ChangeCubeColor(GameObject cube)
    {
        Renderer renderer = cube.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = measurementColor;
        }
    }

    private IEnumerator ResetCubeColor(GameObject cube)
    {
        yield return new WaitForSeconds(colorChangeDuration);
        Renderer renderer = cube.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = Color.white; // Assuming the default color is white
        }
    }
}
