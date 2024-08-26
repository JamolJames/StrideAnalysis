using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI title;
    public Button button1;
    // Start is called before the first frame update
    void Start()
    {
        title.text = "Stride Tracker";
        button1.GetComponentInChildren<TextMeshProUGUI>().text = "Confirm";
    }

}
