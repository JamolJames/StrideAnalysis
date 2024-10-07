using UnityEngine;
using UnityEngine.UI;

public class ObjectController : MonoBehaviour
{
    // Assign the GameObject you want to control in the Inspector
    public GameObject targetObject;

    // UI sliders
    public Slider scaleSlider;
    public Slider rotationSlider;
    public Slider speedSlider;

    // Scale factor range
    public float minScale = 0.1f;
    public float maxScale = 2.0f;

    // Speed multiplier for translation (or any other animation)
    public float speedMultiplier = 10f;

    private Vector3 originalScale;
    private float originalSpeed;

    void Start()
    {
        // Initialize the original scale and speed
        originalScale = targetObject.transform.localScale;
        originalSpeed = speedMultiplier;

        // Add listeners to the sliders
        scaleSlider.onValueChanged.AddListener(OnScaleChanged);
        rotationSlider.onValueChanged.AddListener(OnRotationChanged);
        speedSlider.onValueChanged.AddListener(OnSpeedChanged);
    }

    void OnScaleChanged(float value)
    {
        // Map slider value to scale range and apply to the object
        float scaleValue = Mathf.Lerp(minScale, maxScale, value);
        targetObject.transform.localScale = originalScale * scaleValue;
    }

    void OnRotationChanged(float value)
    {
        // Rotate the object around the Y-axis based on the slider value
        targetObject.transform.rotation = Quaternion.Euler(0, value * 360f, 0);
    }

    void OnSpeedChanged(float value)
    {
        // Adjust speed multiplier based on slider value
        speedMultiplier = originalSpeed * value;
    }

    void Update()
    {
        // Example: Use the speed multiplier for translating the object (if applicable)
        // This could be animation or movement logic
        targetObject.transform.Translate(Vector3.forward * speedMultiplier * Time.deltaTime);
    }
}
