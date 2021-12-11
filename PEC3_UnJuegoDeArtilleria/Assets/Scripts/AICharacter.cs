using UnityEngine;

public class AICharacter : PlayerController
{
    [HideInInspector] public bool IsEnemyNear;
    [HideInInspector] public Transform EnemyPosition;

    private float shootingTilt;
    private int direction;
    private bool changedDirectionInThisTurn;
    private bool changedWeaponInThisTurn;

    private void Start()
    {
        base.Start();

        shootingTilt = Random.Range(0, 0.5f);
        direction = Random.Range(0, 2) == 0 ? -1 : 1;
    }

    private void Update()
    {
        if (!HasTurn)
            return;

        base.Update();

        if(IsEnemyNear)
        {
            if (tilt != shootingTilt)
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
                weaponComponent.Shoot();
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
