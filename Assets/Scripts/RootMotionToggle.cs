using UnityEngine;
using UnityEngine.Playables;

public class RootMotionToggle : MonoBehaviour
{
    public PlayableDirector timeline; // Reference to Timeline
    public Animator animator;
    private bool isRunningInPlace;

    void Start()
    {
        if (animator == null)
            animator = GetComponent<Animator>();

        if (timeline == null)
            timeline = GetComponent<PlayableDirector>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ToggleRunningMode();
        }

        if (isRunningInPlace)
        {
            transform.position -= new Vector3(animator.deltaPosition.x, 0, animator.deltaPosition.z);
        }
    }

    public void ToggleRunningMode()
    {
        isRunningInPlace = !isRunningInPlace;

        if (isRunningInPlace)
        {
            animator.applyRootMotion = false;
            timeline.time = 0; // Reset Timeline (optional)
            timeline.Pause();  // Pause Timeline when running in place
        }
        else
        {
            animator.applyRootMotion = true;
            timeline.Play();   // Resume Timeline when moving
        }
    }
}