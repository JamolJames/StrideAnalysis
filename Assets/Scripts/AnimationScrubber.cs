using UnityEngine;
using UnityEngine.UI;


public class AnimationScrubber : MonoBehaviour
{
    public Slider scrubBar; // Slider to control playback
    public GameObject runner;
    void Start()
    {
        scrubBar = GetComponent<Slider>();
    }

    public void RunnerTimeController()
    {
        Animator runnerAnimator = runner.GetComponent<Animator>();
        runnerAnimator.SetFloat("TimeValue", scrubBar.value);
        
    }
}
