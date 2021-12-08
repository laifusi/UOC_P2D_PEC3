using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private ParticleSystem explosionEffect;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Instantiate(explosionEffect, transform.position, Quaternion.identity, transform);
        Invoke(nameof(Destroy), explosionEffect.main.duration);
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }
}
