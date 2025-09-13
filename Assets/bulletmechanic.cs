using UnityEngine;
using System.Collections;

public class BulletBehaviour : MonoBehaviour
{
    public float speed, damage, destroyTime;
    public float wait = 0.3f;
    public float flyDuration = 3f;

    private Animator anim;
    private Collider2D bulletCollider;
    private float flyTimer;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        bulletCollider = GetComponent<Collider2D>(); // Ambil collider apapun
        Destroy(gameObject, destroyTime);
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
        if (collision.CompareTag("Boss"))
        {
            BossHealth boss = collision.GetComponentInParent<BossHealth>();
            if (boss != null)
            {
                boss.TakeDamage(damage);
            }

            StartCoroutine(Hilang());
        }
    }

    private IEnumerator Hilang()
    {
        anim.SetTrigger("ilang");
        speed = 0;
        if (bulletCollider != null) // aman meski collider bukan Box
            bulletCollider.enabled = false;

        yield return new WaitForSeconds(wait);
        Destroy(gameObject);
    }
}
