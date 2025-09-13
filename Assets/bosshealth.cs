using UnityEngine;
using UnityEngine.UI;
using TMPro; // penting untuk pakai TextMeshPro

public class BossHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 20f;   // default 20
    private float currentHealth;

    [Header("UI Settings")]
    public Image healthUI;          // drag health bar (Image dengan Fill)
    public TextMeshProUGUI healthText; // drag TMP text untuk 20/20

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // biar gak minus
        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Destroy(gameObject); // musuh mati
        }
    }

    private void UpdateHealthUI()
    {
        if (healthUI != null)
            healthUI.fillAmount = currentHealth / maxHealth;

        if (healthText != null)
            healthText.text = currentHealth + "/" + maxHealth;
    }
}
