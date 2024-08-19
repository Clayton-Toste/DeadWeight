using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    Vector3 acceleration;


    void Update()
    {
        transform.position = Vector3.SmoothDamp(transform.position, target.position, ref acceleration, 0.2f);
    }
}
