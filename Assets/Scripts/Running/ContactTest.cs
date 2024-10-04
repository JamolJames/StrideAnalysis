using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactTest : MonoBehaviour
{
    void OnCollisionEnter(Collision other) 
    {
        GetComponent<MeshRenderer>().material.color = Color.white;
    }
}
