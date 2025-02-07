
using UnityEngine;

public class ContactTest : MonoBehaviour
{
    void OnCollisionEnter(Collision other) 
    {
        // GetComponent<MeshRenderer>().material.color = Color.white;
        if (other.gameObject.CompareTag("RFoot"))
        {
            DidHit();
        }
    }

    void DidHit()
    {
        Debug.Log("Right Foot Contact");
    }
}
