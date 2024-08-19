using System;
using System.Linq;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject shuyet;

    float countdown = 5.0f;

    void Update()
    {
        countdown -= Time.deltaTime;
        if (countdown <= 0.0f)
        {
            countdown += 5.0f;
            if ((Player.singleton.transform.position - transform.position).magnitude < 7.0f && FindObjectsByType<Enemy>(FindObjectsSortMode.None).Where(enemy => (enemy.transform.position - transform.position).magnitude < 20.0f).Count() < 10)
            {
                for (float theta = 0; theta < 2 * MathF.PI; theta += MathF.PI / 3.0f)
                {
                    RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(MathF.Sin(theta), MathF.Cos(theta)), 2.0f, 1);

                    if (hit.collider == null)
                    {
                        Instantiate(shuyet, transform.position + 2.0f * new Vector3(MathF.Sin(theta), MathF.Cos(theta)), Quaternion.identity);
                    }
                }
            }
        }
    }
}
