using UnityEngine;
using UnityEngine.Video;

public class LoadNewVideo : MonoBehaviour
{
    public VideoClip endDialog;
    void LoadVideo()
    {
        GetComponent<VideoPlayer>().clip = endDialog;
    }
}
