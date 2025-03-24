using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class TimelineSlider : MonoBehaviour
{
    public PlayableDirector director; 
    public Slider timeSlider; 

    bool isDragging; // To prevent conflicts between UI input and playback

    void Start()
    {
        if (director != null && timeSlider != null)
        {
            // Set the slider's max value to match the Timeline duration
            timeSlider.maxValue = (float)director.duration;
            timeSlider.minValue = 0;
            timeSlider.value = (float)director.time;

            // Add listener for user interaction
            timeSlider.onValueChanged.AddListener(OnSliderValueChanged);
        }
    }

    void Update()
    {
        if (director != null && timeSlider != null && !isDragging)
        {
            // Update slider value only when not dragging
            timeSlider.value = (float)director.time;
        }
    }

    // Called when the user drags the slider
    public void OnSliderValueChanged(float newTime)
    {
        if (director != null)
        {
            isDragging = true;
            director.time = newTime; // Seek Timeline to new time
            director.Evaluate(); // Forces the Timeline to refresh
        }
    }

    // Prevent conflicts between user input and playback updates
    public void OnPointerDown()
    {
        isDragging = true;
    }

    public void OnPointerUp()
    {
        isDragging = false;
    }
}

