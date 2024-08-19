using UnityEngine;

public class Title : MonoBehaviour
{
    public GameObject start;

    void Update()
    {
        start.SetActive(Time.time % 1.0f > 0.5f);
    }
}
