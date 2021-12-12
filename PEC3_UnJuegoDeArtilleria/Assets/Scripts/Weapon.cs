using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] Transform exitPoint; //point the projectile should exit from

    [HideInInspector] public ProjectileSO Projectile; //type of projectile the weapon shoots
    [HideInInspector] public TeamColor TeamColor; //TeamColor

    Rigidbody2D bulletRB; //the rigidbody of the bullet

    /// <summary>
    /// Method to shoot a bullet
    /// We instantiate the prefab of the ProjectileSO and set the sprite, layer mask and damage of the projectile
    /// We wait until the LateUpdate to activate the rigidbody in order to avoid weird physics behaviours
    /// </summary>
    public void Shoot()
    {
        GameObject bullet = Instantiate(Projectile.Prefab, exitPoint.position, exitPoint.rotation * Projectile.Prefab.transform.rotation);
        SpriteRenderer bulletSR = bullet.GetComponent<SpriteRenderer>();
        bulletSR.sprite = Projectile.GetSprite(TeamColor);
        bullet.layer = Projectile.GetLayerMask(TeamColor);
        Projectile bulletProjectile = bullet.GetComponent<Projectile>();
        bulletProjectile.Damage = Projectile.Damage;
        bulletRB = bullet.GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// LateUpdate method where we define the rigidbody gravity and give the shooting force to the last saved bullet
    /// </summary>
    private void LateUpdate()
    {
        if (bulletRB != null)
        {
            bulletRB.gravityScale = Projectile.GravityAffection;
            bulletRB.AddForce(transform.right * Projectile.Speed, ForceMode2D.Impulse);
            bulletRB = null;
        }
    }
}
