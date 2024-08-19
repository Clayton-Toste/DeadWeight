using UnityEngine;

public class Jukebox : MonoBehaviour
{
    public void PlaySound(Object sound)
    {
        AudioSource.PlayClipAtPoint(sound as AudioClip, transform.position, 0.2f);
    }
}
