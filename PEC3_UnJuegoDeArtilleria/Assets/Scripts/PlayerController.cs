using System;
using UnityEngine;
using UnityEngine.Experimental.U2D.Animation;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float speed = 5; //character speed
    [SerializeField] float floorCheckDistance = 0.1f; //floor detection distance
    [SerializeField] LayerMask groundMask; //layer mask for the ground
    [SerializeField] Transform feet; //transform for the floor detection
    [SerializeField] PhysicsMaterial2D fullFriction; //PhysicsMaterial for full friction
    [SerializeField] PhysicsMaterial2D noFriction; //PhysicsMaterial for no friction
    [SerializeField] WeaponSO[] weapons; //weapons
    [SerializeField] Transform hand; //hand point
    [SerializeField] Transform weaponParent; //Transform we'll use as the weapon's parent
    [SerializeField] SpriteLibraryAsset charactersLibraryAsset; //Library Asset for the character sprites
    [SerializeField] SpriteResolver[] bodyParts; //body part sprites
    [SerializeField] int initialHealth = 100; //initial health

    public bool IsAI; //whether the character is AI or not
    public bool HasTurn; //whether the character has the turn or not
    public string CharacterName; //the character's name
    public CharacterType CharacterType; //the type of character
    public TeamColor TeamColor; //the team color
    public int TeamID; //the team id

    public Action<int> OnHealthChanged; //Action for when the health changes
    public static Action<int, PlayerController> OnCharacterDead; //Action for when a character dies

    private Rigidbody2D rigidbody2d; //rigidbody
    private bool isGrounded; //whether the character is grounded or not
    private float slopeAngle; //the slope's angle
    private Vector2 slopeNormalPerpendicular; //the slope's perpendicular
    private bool isOnSlope; //whether the character is on a slope or not
    private float previousSlopeAngle; //the previous slope angle
    private RaycastHit2D hit; //raycast hit
    private WeaponSO currentWeapon; //the currently selected weapon
    private int currentWeaponIndex; //the index of the currently selected weapon
    private float verticalInput; //vertical input (to tilt the character)
    private Animator animator; //Animator component
    private int health; //current health
    private AudioSource audioSource; //AudioSource component

    private static float nextChangeTime; //time until next possible change of character (necessary to avoid multiple characters triggering the method)

    protected float horizontalInput; //horizontal input
    protected Weapon weaponComponent; //Weapon component
    protected float tilt = 0.5f; //tilt of the character
    protected bool facingRight = true; //whether the character is facing right or left

    /// <summary>
    /// Start method where we initialize everything
    /// </summary>
    protected void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        currentWeapon = weapons[0];
        currentWeaponIndex = 0;
        ChangeWeapon(0);
        string maskName = "";
        switch(TeamColor)
        {
            case TeamColor.Red:
                maskName = "RedTeam";
                break;
            case TeamColor.Blue:
                maskName = "BlueTeam";
                break;
            case TeamColor.Purple:
                maskName = "PurpleTeam";
                break;
            case TeamColor.Orange:
                maskName = "OrangeTeam";
                break;
            case TeamColor.Green:
                maskName = "GreenTeam";
                break;
        }
        if(maskName != "")
            gameObject.layer = LayerMask.NameToLayer(maskName);
        for(int i = 0; i < bodyParts.Length; i++)
        {
            bodyParts[i].SetCategoryAndLabel(bodyParts[i].GetCategory(), CharacterType.ToString());
        }

        health = initialHealth;
        OnHealthChanged?.Invoke(health);
    }

    /// <summary>
    /// Update method where we reset the friction and animator if the character doesn't have the turn
    /// If it does, we check if we are on the ground, read the player's input (if the character isn't AI),
    /// check if we are on a slope, move the character and tilt it
    /// </summary>
    protected void Update()
    {
        if (!HasTurn)
        {
            ResetFriction();
            ResetAnimator();
            return;
        }

        CheckIsGrounded();

        if (!IsAI)
        {
            horizontalInput = Input.GetAxis("Horizontal");

            if (Time.time >= nextChangeTime && Input.GetButtonDown("Jump"))
            {
                GameplayManager.Instance.ChangeCharacter();
                nextChangeTime = Time.time + 1;
            }

            verticalInput = Input.GetAxis("Vertical");

            if (verticalInput > 0)
                tilt += 0.001f;
            else if (verticalInput < 0)
                tilt -= 0.001f;

            if (Input.mouseScrollDelta.y > 0)
                ChangeWeapon(1);
            else if (Input.mouseScrollDelta.y < 0)
                ChangeWeapon(-1);

            if (Input.GetMouseButtonUp(0))
            {
                weaponComponent.Shoot();
            }
        }

        GetSlopeInfo();

        Move();

        tilt = Mathf.Clamp(tilt, 0, 1);
        animator.SetFloat("tilt", tilt);
    }

    /// <summary>
    /// Method to reset the walking animation to false
    /// </summary>
    protected void ResetAnimator()
    {
        animator.SetBool("walking", false);
    }

    /// <summary>
    /// Method to check if we are on a slope or not and change the friction accordingly
    /// </summary>
    private void GetSlopeInfo()
    {
        hit = Physics2D.Raycast(feet.position, Vector2.down, floorCheckDistance, groundMask);
        if (hit)
        {
            isGrounded = true;
            slopeNormalPerpendicular = Vector2.Perpendicular(hit.normal).normalized;
            slopeAngle = Vector2.Angle(hit.normal, Vector2.up);

            if (slopeAngle != previousSlopeAngle)
            {
                isOnSlope = true;
                previousSlopeAngle = slopeAngle;
            }
        }

        if (isOnSlope && horizontalInput == 0)
        {
            rigidbody2d.sharedMaterial = fullFriction;
        }
        else
        {
            rigidbody2d.sharedMaterial = noFriction;
        }
    }

    /// <summary>
    /// Method to check if we are grounded
    /// </summary>
    private void CheckIsGrounded()
    {
        isGrounded = Physics2D.OverlapCircle(feet.position, floorCheckDistance, groundMask);   
    }

    /// <summary>
    /// Method to move the character, keeping in mind the horizontal input, if we are on a slope,
    /// if we are grounded and which way we are facing
    /// </summary>
    private void Move()
    {
        if (isGrounded && !isOnSlope)
        {
            rigidbody2d.velocity = new Vector3(speed * horizontalInput, 0, 0);
        }
        else if (isGrounded && isOnSlope)
        {
            rigidbody2d.velocity = new Vector3(speed * slopeNormalPerpendicular.x * -horizontalInput, speed * slopeNormalPerpendicular.y * -horizontalInput, 0);
        }
        else if (!isGrounded)
        {
            rigidbody2d.velocity = new Vector3(speed * horizontalInput, rigidbody2d.velocity.y);
        }

        if (horizontalInput != 0)
        {
            if(facingRight && horizontalInput < 0)
            {
                FlipLeft();
            }
            else if(!facingRight && horizontalInput > 0)
            {
                FlipRight();
            }
        }

        animator.SetBool("walking", horizontalInput != 0);
    }

    /// <summary>
    /// Method to flip the character to face right
    /// </summary>
    protected void FlipRight()
    {
        facingRight = true;
        transform.Rotate(0, -180, 0);
    }

    /// <summary>
    /// Method to flip the character to face left
    /// </summary>
    protected void FlipLeft()
    {
        facingRight = false;
        transform.Rotate(0, 180, 0);
    }

    /// <summary>
    /// Method to change the selected weapon
    /// </summary>
    /// <param name="direction">int to determine which way we change weapon, up or down</param>
    public void ChangeWeapon(int direction)
    {
        for(int i = 0; i< weaponParent.childCount; i++)
        {
            Destroy(weaponParent.GetChild(i).gameObject);
        }
        currentWeaponIndex += direction;
        if(currentWeaponIndex >= weapons.Length)
        {
            currentWeaponIndex = 0;
        }
        else if(currentWeaponIndex < 0)
        {
            currentWeaponIndex = weapons.Length - 1;
        }
        currentWeapon = weapons[currentWeaponIndex];
        GameObject weapon = Instantiate(currentWeapon.Prefab, hand.position, hand.rotation, weaponParent);
        weaponComponent = weapon.GetComponent<Weapon>();
        weaponComponent.Projectile = currentWeapon.Projectile;
        weaponComponent.TeamColor = TeamColor;
        SpriteRenderer weaponSpriteRenderer = weapon.GetComponent<SpriteRenderer>();
        if(weaponSpriteRenderer != null)
            weaponSpriteRenderer.sprite = currentWeapon.GetSprite(TeamColor);
        animator.SetTrigger(currentWeapon.name);
    }

    /// <summary>
    /// Method to add damage to the character
    /// If health is 0 or negative, we die
    /// </summary>
    /// <param name="damage">amount of damage received</param>
    public void TakeDamage(int damage)
    {
        audioSource.Play();
        health -= damage;
        OnHealthChanged?.Invoke(health);
        if (health <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// Method to kill the character
    /// </summary>
    private void Die()
    {
        OnCharacterDead?.Invoke(TeamID, this);
        Destroy(gameObject);
    }

    /// <summary>
    /// Method to reset the rigidbody's material to full friction
    /// </summary>
    protected void ResetFriction()
    {
        rigidbody2d.sharedMaterial = fullFriction;
    }
}

public enum CharacterType //enum that defines the types of characters
{
    FemaleAdventurer, FemalePerson, MaleAdventurer, MalePerson, Robot, Zombie
}
