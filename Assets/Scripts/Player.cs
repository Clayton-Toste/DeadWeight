using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Animator)), RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    public static Player singleton;
    public bool receivingInput;
    public Vector2 targetMovement;

    public GameObject die;

    Animator animator;
    Rigidbody2D rb2;

    InputAction playerMove, playerAim, playerDash, playerScytheKeyboard, playerScytheMouse, playerSickleKeyboard, playerSickleMouse;

    Vector2 acceleration;

    void Start()
    {
        singleton = this;

        TryGetComponent(out animator);
        TryGetComponent(out rb2);

        playerMove = InputSystem.actions.FindAction("Player/Move");
        playerAim = InputSystem.actions.FindAction("Player/Aim");
        playerDash = InputSystem.actions.FindAction("Player/Dash");
        playerScytheKeyboard = InputSystem.actions.FindAction("Player/ScytheKeyboard");
        playerScytheMouse = InputSystem.actions.FindAction("Player/ScytheMouse");
        playerSickleKeyboard = InputSystem.actions.FindAction("Player/SickleKeyboard");
        playerSickleMouse = InputSystem.actions.FindAction("Player/SickleMouse");
    }

    void Update()
    {
        if (receivingInput)
        {
            Vector2 movement = playerMove.ReadValue<Vector2>();

            animator.SetFloat("MoveX", movement.x);
            animator.SetFloat("MoveY", movement.y);

            if (playerDash.WasPressedThisFrame() && movement.magnitude != 0.0f)
            {
                animator.SetTrigger("Dash");
            }

            if (playerScytheKeyboard.WasPressedThisFrame())
            {
                Vector2 aim = playerAim.ReadValue<Vector2>();

                if (aim.magnitude == 0.0f)
                    aim = movement;

                animator.SetTrigger($"Scythe{GetDirection(aim)}");
            }

            if (playerScytheMouse.WasPressedThisFrame())
            {
                Vector2 aim = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()) - transform.position;

                animator.SetTrigger($"Scythe{GetDirection(aim)}");
            }

            if (playerSickleKeyboard.WasPressedThisFrame())
            {
                Vector2 aim = playerAim.ReadValue<Vector2>();

                if (aim.magnitude == 0.0f)
                    aim = movement;

                animator.SetTrigger($"Sickle{GetDirection(aim)}");
            }

            if (playerSickleMouse.WasPressedThisFrame())
            {
                Vector2 aim = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()) - transform.position;

                animator.SetTrigger($"Sickle{GetDirection(aim)}");
            }
        }

        Vector2 stabilizedVelocity = targetMovement.normalized * (MathF.Abs(targetMovement.x) + MathF.Abs(targetMovement.y)) * new Vector2(1.0f, MathF.Sqrt(2.0f) / 2.0f);
        rb2.linearVelocity = Vector2.SmoothDamp(rb2.linearVelocity, stabilizedVelocity, ref acceleration, 0.1f);
    }

    static string GetDirection(Vector2 vector)
    {   
        if (vector == Vector2.zero)
        {
            return "Down";
        }
        return new string[4] { "Down", "Left", "Up", "Right" }[((int)MathF.Round(MathF.Atan2(vector.x, vector.y) * 2.0f / MathF.PI) + 2) % 4];
    }

    [ContextMenu("M")]
    void Die()
    {
        animator.SetTrigger("Die");
        die.SetActive(true);
    }

    public void Restart()
    {
        SceneManager.LoadScene("Game");
    }
}
