using UnityEngine;

public class AICharacter : PlayerController
{
    [HideInInspector] public bool IsEnemyNear;
    [HideInInspector] public Transform EnemyPosition;

    private float shootingTilt;
    private int direction;
    private bool changedDirectionInThisTurn;
    private bool changedWeaponInThisTurn;
    private float nextShootingTime;

    [SerializeField] float shootingRate = 0.5f;

    private void Start()
    {
        base.Start();

        shootingTilt = Random.Range(0, 0.5f);
        direction = Random.Range(0, 2) == 0 ? -1 : 1;
    }

    private void Update()
    {
        if (!HasTurn)
        {
            ResetAI();
            ResetFriction();
        }

        if (!HasTurn)
            return;

        base.Update();

        if(IsEnemyNear)
        {
            horizontalInput = 0;
            if (Mathf.Floor(tilt * 1000)/1000 != Mathf.Floor(shootingTilt*1000)/1000)
            {
                if(tilt > shootingTilt)
                {
                    tilt -= 0.001f;
                }
                else
                {
                    tilt += 0.001f;
                }
            }
            else
            {
                if(Time.time >= nextShootingTime)
                {
                    weaponComponent.Shoot();
                    nextShootingTime = Time.time + 1/shootingRate;
                }
            }
        }
        else
        {
            horizontalInput = direction;

            if(!changedDirectionInThisTurn && Random.Range(0, 100) < 1)
            {
                direction = -direction;
                changedDirectionInThisTurn = true;
            }

            if(!changedWeaponInThisTurn && Random.Range(0, 100) < 1)
            {
                changedWeaponInThisTurn = true;
                ChangeWeapon(direction);
            }
        }
    }

    public void ResetAI()
    {
        changedDirectionInThisTurn = false;
        changedWeaponInThisTurn = false;
    }
}
