using UnityEngine;
using UnityEngine.UI; // para usar Slider

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private Slider healthBar;
    [SerializeField] private float maxHealth = 1f; // vida máxima = 1 (100%)
    private float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.value = currentHealth;
    }

    void OnCollisionEnter(Collision collision)
    {
        // Si toca un objeto con tag "Mouse"
        if (collision.gameObject.CompareTag("Mouse"))
        {
            TakeDamage(0.25f); // baja 1/4 de la vida
        }
    }

    void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        healthBar.value = currentHealth;

        if (currentHealth <= 0f)
        {
            Debug.Log("Jugador sin vida");
            // aquí puedes añadir lógica de muerte, reinicio, etc.
        }
    }
}