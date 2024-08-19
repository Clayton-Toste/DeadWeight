using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator)), RequireComponent(typeof(NavMeshAgent)), RequireComponent(typeof(Rigidbody2D))]
public class Ba : MonoBehaviour
{
    public bool correctingDirection;
    public float speed;

    Animator animator;
    NavMeshAgent agent;
    Rigidbody2D rb;

    void Start()
    {
        TryGetComponent(out animator);
        TryGetComponent(out agent);
        TryGetComponent(out rb);
        agent.updateUpAxis = false;
        agent.updateRotation = false;
        agent.updatePosition = false;
    }

    void Update()
    {
        agent.SetDestination(Player.singleton.transform.position);
        rb.linearVelocity = agent.velocity * speed;
    }
}
