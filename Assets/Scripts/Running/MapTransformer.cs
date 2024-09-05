using UnityEngine;
using UnityEngine.UI;

public class MapTransformer : MonoBehaviour
{
    public Slider scaleSlider;
    public Slider rotationSlider;
    private float scaleSliderNumber;
    private float rotationSliderNumber;
    float rotationAmt = 20f;
    float scaleAmt = 20f; 

    // Transform thisTransform;
    Vector3 objectScale;

    void Start() 
    {
        // Transform thisTransform = GetComponent<Transform>();
        // objectScale = thisTransform.localScale;
        
    }

    void Update()
    {
        // RotateScene();
        ApplyRotation();
        ScaleScene();
    }

    // void RotateScene()
    // {
    //     if (Input.GetKey(KeyCode.S)) // TODO: change keycode to VR controller 
    //     {
    //         ApplyRotation(rotationAmt);
    //     }
    //     else if (Input.GetKey(KeyCode.D))
    //     {
    //         ApplyRotation(-rotationAmt);
    //     }
    // }

    void ScaleScene()
    {
        // TODO: change keycode to VR controller
            scaleSliderNumber = scaleSlider.value * scaleAmt;
            Vector3 scale = new Vector3(scaleSliderNumber, scaleSliderNumber,scaleSliderNumber);
            transform.localScale = scale;
    }

    // void ApplyRotation(float rotateThisFrame)
    // {
    //     rotationSliderNumber = rotationSlider.value * rotationAmt;
    //     transform.Rotate(Vector3.up * Time.deltaTime * rotateThisFrame);
    // }

    void ApplyRotation()
    {
        rotationSliderNumber = rotationSlider.value * rotationAmt;
        Vector3 rotate = new Vector3(0,rotationSliderNumber,0);
        transform.rotation = Quaternion.Euler(rotate);
    }


}
