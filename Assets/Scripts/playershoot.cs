using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private float shootCooldown;
    private float cooldownTimer = Mathf.Infinity;
    public GameObject bullet;
    public Transform shotPoint;

    void Update()
    {
        Shoot();
        cooldownTimer += Time.deltaTime;
    }

    void Shoot()
    {
        if (Input.GetKeyDown(KeyCode.K) && cooldownTimer > shootCooldown)
        {
            Instantiate(bullet, shotPoint.position, transform.rotation);
            cooldownTimer = 0;
        }
    }
}
