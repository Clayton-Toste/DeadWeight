using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class Title : MonoBehaviour
{
    public GameObject start;
    public VideoPlayer videoPlayer;

    void Update()
    {
        start.SetActive(Time.time % 1.0f > 0.5f);

        if (Input.anyKeyDown)
        {
            videoPlayer.gameObject.SetActive(true);
        }

        if ((ulong)videoPlayer.frame + 1 == videoPlayer.frameCount && videoPlayer.frameCount > 0)
        {
            SceneManager.LoadScene("Tutorial");
        }
    }
}
