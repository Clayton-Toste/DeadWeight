using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(CompositeShadowCaster2D)), RequireComponent(typeof(SpriteRenderer))]
public class ShadowUpdater : MonoBehaviour
{
    static readonly FieldInfo shapePathField;
    static readonly FieldInfo shadowRebuildField;

    public GameObject shadowSpiller;

    List<ShadowCaster2D> shadowCasters = new();
    SpriteRenderer spriteRenderer;

    Sprite oldSprite;

    static ShadowUpdater()
    {
        shapePathField = typeof(ShadowCaster2D).GetField("m_ShapePath", BindingFlags.NonPublic | BindingFlags.Instance);
        shadowRebuildField = typeof(ShadowCaster2D).GetField("m_ForceShadowMeshRebuild", BindingFlags.NonPublic | BindingFlags.Instance);

    }

    void Start()
    {
        TryGetComponent(out spriteRenderer);
    }

    void Update()
    {
        if (spriteRenderer.sprite != oldSprite)
        {
            while (spriteRenderer.sprite.GetPhysicsShapeCount() > shadowCasters.Count)
            {
                Instantiate(shadowSpiller, transform).TryGetComponent(out ShadowCaster2D shadowCaster);
                shadowCasters.Add(shadowCaster);
            }
            for (int i = 0; i < shadowCasters.Count; i++)
            {
                if (i < spriteRenderer.sprite.GetPhysicsShapeCount())
                {
                    shadowCasters[i].enabled = true;
                    List<Vector2> shape = new();
                    spriteRenderer.sprite.GetPhysicsShape(i, shape);
                    shapePathField.SetValue(shadowCasters[i], shape.Select(point => (Vector3)point).ToArray());
                    shadowRebuildField.SetValue(shadowCasters[i], true);
                }
                else
                {
                    shadowCasters[i].enabled = false;
                }
            }
            oldSprite = spriteRenderer.sprite;
        }
    }
}