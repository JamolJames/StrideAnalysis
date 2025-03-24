using UnityEngine;
using UnityEngine.Playables;

public class PlayAnimation : MonoBehaviour
{
    public PlayableDirector timeline;

    public void PlayTimeline()
    {
        timeline.Play();
    }

    public void PauseTimeline()
    {
        timeline.Pause();
    }
}




