using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] Transform exitPoint;

    [HideInInspector] public ProjectileSO Projectile;
    [HideInInspector] public TeamColor TeamColor;

    Rigidbody2D bulletRB;

    [ContextMenu("Shoot")]
    public void Shoot()
    {
        GameObject bullet = Instantiate(Projectile.Prefab, exitPoint.position, exitPoint.rotation * Projectile.Prefab.transform.rotation);
        SpriteRenderer bulletSR = bullet.GetComponent<SpriteRenderer>();
        bulletSR.sprite = Projectile.SetSprite(TeamColor);
        bullet.layer = Projectile.GetLayerMask(TeamColor);
        Projectile bulletProjectile = bullet.GetComponent<Projectile>();
        bulletProjectile.Damage = Projectile.Damage;
        bulletRB = bullet.GetComponent<Rigidbody2D>();
    }

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
