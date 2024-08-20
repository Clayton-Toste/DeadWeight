using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

[RequireComponent(typeof(Animator)), RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    public static Player singleton;

    [Header("Config")]
    public int startingHealth;
    public GameObject dieGUI;
    public GameObject winGUI;
    public VideoPlayer outroVideo;
    public Slider healthSlider;
    public TMP_Text soulsCounter, soulsOutro, soulsEnding;
    public AudioClip hitNoise, deathNoise;
    public GameObject moveTutorial, dashTutorial, sickleTutorial, scytheTutorial;

    [Header("Controls")]
    public bool receivingInput;
    public Vector2 targetMovement;


    int health;
    public int Health
    {
        get => health;
        set
        {
            health = value;
            healthSlider.value = (float)health / startingHealth;
        }
    }
    int souls;
    public int Souls
    {
        get => souls;
        set
        {
            souls = value;
            soulsCounter.text = $"SOULS: {souls}";
            soulsOutro.text = $"Total Souls: {souls}";
            soulsEnding.text = $"YOU GOT {souls} SOULS!";
        }
    }

    Animator animator;
    Rigidbody2D rb2;
    InputAction playerMove, playerAim, playerDash, playerScytheKeyboard, playerScytheMouse, playerSickleKeyboard, playerSickleMouse;
    Vector2 acceleration;

    void Start()
    {
        singleton = this;

        Health = startingHealth;

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

            if (movement.magnitude != 0 && moveTutorial.activeSelf)
            {
                moveTutorial.SetActive(false);
                dashTutorial.SetActive(true);
            }
            else if (playerDash.WasPressedThisFrame() && movement.magnitude != 0.0f)
            {
                animator.SetTrigger("Dash");
                if (dashTutorial.activeSelf)
                {
                    dashTutorial.SetActive(false);
                    sickleTutorial.SetActive(true);
                }
            }
            else if (playerScytheKeyboard.WasPressedThisFrame())
            {
                Vector2 aim = playerAim.ReadValue<Vector2>();

                if (aim.magnitude == 0.0f)
                    aim = movement;

                animator.SetTrigger($"Scythe{GetDirection(aim)}");

                if (scytheTutorial.activeSelf)
                {
                    scytheTutorial.SetActive(false);
                }
            }
            else if (playerScytheMouse.WasPressedThisFrame())
            {
                Vector2 aim = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()) - transform.position;

                animator.SetTrigger($"Scythe{GetDirection(aim)}");

                if (scytheTutorial.activeSelf)
                {
                    scytheTutorial.SetActive(false);
                }
            }
            else if (playerSickleKeyboard.WasPressedThisFrame())
            {
                Vector2 aim = playerAim.ReadValue<Vector2>();

                if (aim.magnitude == 0.0f)
                    aim = movement;

                animator.SetTrigger($"Sickle{GetDirection(aim)}");

                if (sickleTutorial.activeSelf)
                {
                    sickleTutorial.SetActive(false);
                    scytheTutorial.SetActive(true);
                }
            }
            else if (playerSickleMouse.WasPressedThisFrame())
            {
                Vector2 aim = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()) - transform.position;

                animator.SetTrigger($"Sickle{GetDirection(aim)}");
                
                if (sickleTutorial.activeSelf)
                {
                    sickleTutorial.SetActive(false);
                    scytheTutorial.SetActive(true);
                }
            }
        }

        Vector2 stabilizedVelocity = targetMovement.normalized * (MathF.Abs(targetMovement.x) + MathF.Abs(targetMovement.y)) * new Vector2(1.0f, MathF.Sqrt(2.0f) / 2.0f);
        rb2.linearVelocity = Vector2.SmoothDamp(rb2.linearVelocity, stabilizedVelocity, ref acceleration, 0.02f);

        if ((ulong)outroVideo.frame + 1 == outroVideo.frameCount && outroVideo.frameCount > 0)
        {
            outroVideo.transform.GetChild(0).gameObject.SetActive(false);
            winGUI.SetActive(true);
        }
    }

    static string GetDirection(Vector2 vector)
    {
        if (vector == Vector2.zero)
        {
            return "Down";
        }
        return new string[4] { "Down", "Left", "Up", "Right" }[((int)MathF.Round(MathF.Atan2(vector.x, vector.y) * 2.0f / MathF.PI) + 2) % 4];
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.name == "GameEnd")
        {
            outroVideo.gameObject.SetActive(true);
            transform.position = new Vector3(1000.0f, 0.0f);
            animator.enabled = false;
            return;
        }

        if (collider.transform.parent == null)
            return;

        collider.transform.parent.TryGetComponent(out Enemy enemy);

        if (enemy == null)
            return;

        AudioSource.PlayClipAtPoint(hitNoise, transform.position, 0.2f);

        Health -= enemy.damage;
        if (Health <= 0)
        {
            AudioSource.PlayClipAtPoint(deathNoise, transform.position, 0.2f);
            animator.SetTrigger("Die");
            dieGUI.SetActive(true);
            gameObject.SetActive(false);
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene("Game");
    }
}
