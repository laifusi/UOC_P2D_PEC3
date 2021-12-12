using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private ParticleSystem explosionEffect; //ParticleSystem for the explosion

    [HideInInspector] public int Damage; //amount of damage the projectile does

    /// <summary>
    /// OnCollisionEnter to check if we hit a character
    /// </summary>
    /// <param name="collision">Collider2D we hit</param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        GetComponent<AudioSource>().Play();
        Instantiate(explosionEffect, transform.position, Quaternion.identity, transform);
        Invoke(nameof(Destroy), explosionEffect.main.duration);

        var player = collision.collider.GetComponent<PlayerController>();
        if(player != null)
        {
            player.TakeDamage(Damage);
            GetComponent<Collider2D>().enabled = false;
        }
    }

    /// <summary>
    /// Method to destroy the GameObject
    /// </summary>
    private void Destroy()
    {
        Destroy(gameObject);
    }
}
