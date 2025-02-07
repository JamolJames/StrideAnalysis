using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContactTimeDisplay : MonoBehaviour
{
    public Text contactTimeText;
    float timeT;

    public void UpdateContactTime(float newTime)
    {
        timeT = newTime;
        contactTimeText.text = "Contact Time: " + timeT.ToString("F2") + " s";
    }




}
