using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    private PlayerInput playerInput;
    private Rigidbody rb;

    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 5f;

    private bool isGrounded;

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Leer input
        Vector2 moveInput = playerInput.actions["Move"].ReadValue<Vector2>();

        // Direcciones relativas a la cámara
        Vector3 forward = Camera.main.transform.forward;
        forward.y = 0;
        forward.Normalize();

        Vector3 right = Camera.main.transform.right;
        right.y = 0;
        right.Normalize();

        // Movimiento horizontal relativo a la cámara
        Vector3 move = forward * moveInput.y + right * moveInput.x;

        // Mantener la velocidad vertical actual (para no pisar el salto)
        Vector3 velocity = rb.linearVelocity;
        velocity.x = move.x * speed;
        velocity.z = move.z * speed;
        rb.linearVelocity = velocity;

        // Salto
        if (playerInput.actions["Jump"].triggered && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Solo marcar grounded si el objeto tiene tag "Ground"
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }

        // Si toca un ratón, baja vida pero no afecta grounded
        if (collision.gameObject.CompareTag("Mouse"))
        {
            // Aquí llamas a tu lógica de daño
            Debug.Log("Tocó un ratón, pierde vida");
        }
    }

    void OnCollisionExit(Collision collision)
    {
        // Solo desmarcar grounded si se deja de tocar el suelo
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}