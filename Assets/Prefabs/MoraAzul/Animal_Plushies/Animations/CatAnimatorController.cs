using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerInput))]
public class CatController : MonoBehaviour
{
    private Rigidbody rb;
    private Animator animator;
    private PlayerInput playerInput;

    [SerializeField] private float speed = 5f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();
    }

    void FixedUpdate()
    {
        // Leer el input como Vector2 (WASD o joystick)
        Vector2 moveInput = playerInput.actions["Move"].ReadValue<Vector2>();

        // Convertir a Vector3 para movimiento en el plano XZ
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);

        // Aplicar velocidad al Rigidbody
        rb.linearVelocity = new Vector3(move.x * speed, rb.linearVelocity.y, move.z * speed);

        // Animación: si hay movimiento, activar Run
        bool isRun = move.magnitude > 0.1f;
        animator.SetBool("IsRun", isRun);

        // Rotar el gato hacia la dirección de movimiento
        if (isRun)
        {
            transform.forward = move;
        }
    }
}