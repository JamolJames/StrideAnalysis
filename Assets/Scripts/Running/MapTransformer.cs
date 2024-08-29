using UnityEngine;
using UnityEngine.UI;

public class MapTransformer : MonoBehaviour
{

    public Slider scaleSlider;
    float scaleSliderNumber;
    float rotationAmt = 20f;

    void Start() 
    {

        
    }

    void Update()
    {
        RotateScene();
        ScaleScene();
    }

    void RotateScene()
    {
        if (Input.GetKey(KeyCode.S)) // TODO: change keycode to VR controller 
        {
            ApplyRotation(rotationAmt);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            ApplyRotation(-rotationAmt);
        }
    }


    void ScaleScene()
    {
        scaleSliderNumber = scaleSlider.value;
        Vector3 scale = new Vector3(scaleSliderNumber, scaleSliderNumber, scaleSliderNumber);
        this.transform.localScale = scale;
    }

    void ApplyRotation(float rotateThisFrame)
    {
        transform.Rotate(Vector3.up * Time.deltaTime * rotateThisFrame);
    }


}
