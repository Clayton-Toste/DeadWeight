using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ShadowUpdater : MonoBehaviour
{
    Light2D[] lights;

    ShadowCaster2D shadowCaster;
    CircleCollider2D circleCollider;

    
    void Start()
    {
        TryGetComponent(out shadowCaster);
        TryGetComponent(out circleCollider);

        lights = Object.FindObjectsByType<Light2D>(FindObjectsSortMode.None);
    }
    void Update()
    {
        shadowCaster.enabled = !lights.Any(light => (light.transform.position - (Vector3)circleCollider.offset - transform.position).magnitude < circleCollider.radius);
    }
}