using UnityEngine;

public class AICharacter : PlayerController
{
    [HideInInspector] public bool IsEnemyNear; //bool that indicates if an enemy is near
    [HideInInspector] public Transform EnemyPosition; //Transform of the enemy that is near
    
    private float shootingTilt; //the tilt the AI should reach before shooting
    private float nextShootingTime; //when the AI can shoot again
    private int direction; //the direction of the character (-1 or 1)
    private bool changedDirectionInThisTurn; //bool to determine if the AI can change direction again
    private bool changedWeaponInThisTurn; //bool to determine if the AI can change weapon again

    public static bool teamChanged; //static bool that determines if the AI can change characters (only once per turn)

    [SerializeField] float shootingRate = 0.5f; //rate at which the AI can shoot

    /// <summary>
    /// Start method where we initilize the shootingTilt and the direction
    /// </summary>
    private void Start()
    {
        base.Start();

        shootingTilt = Random.Range(0, 0.5f);
        direction = Random.Range(0, 2) == 0 ? -1 : 1;
    }

    /// <summary>
    /// Update method where we reset the character if it's not our turn
    /// we call the base update
    /// we check if the enemy is near and decide what action should be done:
    /// switch direction if we are facing the other side, tilt the weapon or shoot
    /// If the enemy isn't near, we walk and randomly decide whether to change weapon,
    /// change direction and change character (all limited to once per turn)
    /// </summary>
    private void Update()
    {
        if (!HasTurn)
        {
            ResetAI();
            ResetFriction();
            ResetAnimator();
            return;
        }

        base.Update();

        if(IsEnemyNear)
        {
            if(EnemyPosition.position.x > transform.position.x && !facingRight)
            {
                FlipRight();
            }
            else if(EnemyPosition.position.x < transform.position.x && facingRight)
            {
                FlipLeft();
            }
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
                    shootingTilt = Random.Range(0, 0.5f);
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

            if (teamChanged && Random.Range(0, 100) < 1)
            {
                teamChanged = false;
                GameplayManager.Instance.ChangeCharacter();
            }
        }
    }

    /// <summary>
    /// Method to reset the booleans that define if we changed direction or weapon in this turn
    /// </summary>
    public void ResetAI()
    {
        changedDirectionInThisTurn = false;
        changedWeaponInThisTurn = false;
    }
}
