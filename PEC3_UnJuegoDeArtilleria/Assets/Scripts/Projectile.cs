using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private ParticleSystem explosionEffect;

    [HideInInspector] public int Damage;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Instantiate(explosionEffect, transform.position, Quaternion.identity, transform);
        Invoke(nameof(Destroy), explosionEffect.main.duration);

        var player = collision.collider.GetComponent<PlayerController>();
        if(player != null)
        {
            player.TakeDamage(Damage);
            GetComponent<Collider2D>().enabled = false;
        }
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }
}
