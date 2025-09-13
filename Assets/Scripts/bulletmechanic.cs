using UnityEngine;
using System.Collections;

public class BulletBehaviour : MonoBehaviour
{
    public float speed, damage, destroyTime;
    public float wait = 0.3f; // waktu tunggu sebelum destroy
    public float flyDuration = 3f;

    private Animator anim;
    private Collider2D bulletCollider;
    private float flyTimer;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        bulletCollider = GetComponent<Collider2D>();
        Destroy(gameObject, destroyTime); // backup auto destroy
    }

    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);

        flyTimer += Time.deltaTime;
        if (flyTimer >= flyDuration)
        {
            StartCoroutine(Hilang());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Jika kena Boss
        if (collision.CompareTag("Boss"))
        {
            BossHealth boss = collision.GetComponentInParent<BossHealth>();
            if (boss != null)
            {
                boss.TakeDamage(damage);
            }

            StartCoroutine(Hilang());
        }

        // Jika kena Obstacle
        if (collision.CompareTag("Obstacle"))
        {
            StartCoroutine(Hilang());
        }
    }

    private IEnumerator Hilang()
    {
        anim.SetTrigger("shatter"); // ganti trigger sesuai animator (bulletshatter)
        speed = 0;

        if (bulletCollider != null)
            bulletCollider.enabled = false;

        yield return new WaitForSeconds(wait);
        Destroy(gameObject);
    }
}
