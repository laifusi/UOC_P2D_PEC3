using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] Transform exitPoint;

    [HideInInspector] public ProjectileSO Projectile;
    [HideInInspector] public TeamColor TeamColor;

    Rigidbody2D bulletRB;

    private void Update()
    {
        
    }

    [ContextMenu("Shoot")]
    public void Shoot()
    {
        GameObject bullet = Instantiate(Projectile.Prefab, exitPoint.position, exitPoint.rotation * Projectile.Prefab.transform.rotation);
        SpriteRenderer bulletSR = bullet.GetComponent<SpriteRenderer>();
        bulletSR.sprite = Projectile.SetSprite(TeamColor);
        bulletRB = bullet.GetComponent<Rigidbody2D>();
        bulletRB.gravityScale = Projectile.GravityAffection;
        bulletRB.velocity = transform.right * Projectile.Speed;
    }

    private void LateUpdate()
    {
        if (bulletRB != null)
        {
            Debug.Break();
            bulletRB.gravityScale = Projectile.GravityAffection;
            bulletRB.AddForce(transform.right * Projectile.Speed, ForceMode2D.Impulse);
            bulletRB = null;
        }
    }
}
