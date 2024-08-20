using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator)), RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    [Header("Config")]
    public int startingHealth;
    public int value;
    public float range;
    public int damage;
    public AudioClip hitNoise;

    float accelX;
    float moveX;

    int health;

    Animator animator;
    NavMeshAgent agent;

    void Start()
    {
        health = startingHealth;

        TryGetComponent(out animator);
        TryGetComponent(out agent);
        agent.updateUpAxis = false;
        agent.updateRotation = false;
    }

    void Update()
    {
        agent.SetDestination(Player.singleton.transform.position);

        animator.SetBool("Activated", (Player.singleton.transform.position - transform.position).magnitude <= 10);
        animator.SetBool("InRange", agent.remainingDistance < range);
        if (agent.speed != 0)
            moveX = Mathf.SmoothDamp(moveX, MathF.Sign(agent.velocity.x), ref accelX, 0.1f);
        animator.SetFloat("MoveX", moveX);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.transform.parent == null)
            return;

        collider.transform.parent.TryGetComponent(out Player player);

        if (player == null)
            return;

        health -= (int)collider.transform.localScale.z;
        AudioSource.PlayClipAtPoint(hitNoise, transform.position, 0.2f);
        if (health <= 0)
        {
            animator.SetTrigger("Die");
        }
    }

    public void Die()
    {
        Player.singleton.Souls += value;
        Destroy(gameObject);
    }

    public void ResetFacing()
    {
        moveX = transform.position.x > Player.singleton.transform.position.x ? -1 : 1;
    }
}
