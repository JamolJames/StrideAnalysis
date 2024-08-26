using UnityEngine;

public class MapTransformer : MonoBehaviour
{
    [SerializeField] float scaleAmt = 1;

    float rotationAmt = 20f;

    // Transform thisTransform;
    Vector3 objectScale;

    void Start() 
    {
        Transform thisTransform = GetComponent<Transform>();
        objectScale = thisTransform.localScale;
        
    }

    void Update()
    {
        RotateScene();
        ScaleScene(scaleAmt);
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

    void ScaleScene(float scale)
    {
        if(Input.GetKey(KeyCode.X)) // TODO: change keycode to VR controller
        {
            // transform.localScale = new Vector3(scale, scale, scale);
            Debug.Log(scale);
            // objectScale * scale;
        }
        // objectScale * scaleAmt;
    }

    void ApplyRotation(float rotateThisFrame)
    {
        transform.Rotate(Vector3.up * Time.deltaTime * rotateThisFrame);
    }


}
