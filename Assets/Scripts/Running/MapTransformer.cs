using UnityEngine;
using UnityEngine.UI;

public class MapTransformer : MonoBehaviour
{
    // UI Sliders
    public Slider scaleSlider;
    public Slider rotationSlider;
    public Slider positionXSlider;
    public Slider positionYSlider;
    public Slider positionZSlider;
    public Slider directionSlider;

    // Constants for adjusting scale and rotation
    public float rotationMultiplier = 360f;
    public float scaleMultiplier = 10f;
    public float positionMultiplier = 10f;

    void Update()
    {
        // Apply transformations
        if (scaleSlider != null) ApplyScale();
        if (rotationSlider != null) ApplyRotation();
        if (positionXSlider != null && positionYSlider != null && positionZSlider != null) ApplyPosition();
    }

    // Apply scale transformation based on slider value
    void ApplyScale()
    {
        float scaleValue = scaleSlider.value * scaleMultiplier;
        Vector3 newScale = new Vector3(scaleValue, scaleValue, scaleValue);
        transform.localScale = newScale;
    }

    // Apply rotation transformation based on slider value
    void ApplyRotation()
    {
        float rotationValue = rotationSlider.value * rotationMultiplier;
        transform.rotation = Quaternion.Euler(0, rotationValue, 0);
    }

    // Apply position transformation based on slider values
    void ApplyPosition()
    {
        float xPos = positionXSlider.value * positionMultiplier;
        float yPos = positionYSlider.value * positionMultiplier;
        float zPos = positionZSlider.value * positionMultiplier;

        transform.position = new Vector3(xPos, yPos, zPos);
    }
}
