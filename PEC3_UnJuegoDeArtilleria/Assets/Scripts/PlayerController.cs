using System;
using UnityEngine;
using UnityEngine.Experimental.U2D.Animation;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float speed = 5;
    [SerializeField] float floorCheckDistance = 0.1f;
    [SerializeField] LayerMask groundMask;
    [SerializeField] Transform feet;
    [SerializeField] PhysicsMaterial2D fullFriction;
    [SerializeField] PhysicsMaterial2D noFriction;
    [SerializeField] float jumpForce = 10;
    [SerializeField] WeaponSO[] weapons;
    [SerializeField] Transform hand;
    [SerializeField] Transform weaponParent;
    [SerializeField] SpriteLibraryAsset charactersLibraryAsset;
    [SerializeField] SpriteResolver[] bodyParts;
    [SerializeField] int initialHealth = 100;

    public bool IsAI;
    public bool HasTurn;
    public string CharacterName;
    public CharacterType CharacterType;
    public TeamColor TeamColor;
    public int TeamID;

    public Action<string> OnNameAdded;
    public Action<int> OnHealthChanged;
    public static Action<int, PlayerController> OnCharacterDead;

    private Rigidbody2D rigidbody2d;
    private bool isGrounded;
    private float slopeAngle;
    private Vector2 slopeNormalPerpendicular;
    private bool isOnSlope;
    private float previousSlopeAngle;
    private RaycastHit2D hit;
    private bool isJumping;
    private bool canJump;
    protected float horizontalInput;
    private WeaponSO currentWeapon;
    private int currentWeaponIndex;
    protected Weapon weaponComponent;
    private float verticalInput;
    protected float tilt = 0.5f;
    private Animator animator;
    private int health;
    protected bool facingRight = true;
    private static float nextChangeTime;

    protected void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
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

    protected void ResetAnimator()
    {
        animator.SetBool("walking", false);
    }

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

    private void CheckIsGrounded()
    {
        isGrounded = Physics2D.OverlapCircle(feet.position, floorCheckDistance, groundMask);

        if(rigidbody2d.velocity.y <= 0f)
        {
            isJumping = false;
        }

        if(isGrounded && !isJumping)
        {
            canJump = true;
        }    
    }

    private void Jump()
    {
        if(canJump)
        {
            canJump = false;
            isJumping = true;
            rigidbody2d.AddForce(new Vector3(0, jumpForce, 0), ForceMode2D.Impulse);
        }
    }

    private void Move()
    {
        if (isGrounded && !isOnSlope && !isJumping)
        {
            rigidbody2d.velocity = new Vector3(speed * horizontalInput, 0, 0);
        }
        else if (isGrounded && isOnSlope && !isJumping)
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

    protected void FlipRight()
    {
        facingRight = true;
        transform.Rotate(0, -180, 0);
    }

    protected void FlipLeft()
    {
        facingRight = false;
        transform.Rotate(0, 180, 0);
    }

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
            weaponSpriteRenderer.sprite = currentWeapon.SetSprite(TeamColor);
        animator.SetTrigger(currentWeapon.name);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        OnHealthChanged?.Invoke(health);
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        OnCharacterDead?.Invoke(TeamID, this);
        Destroy(gameObject);
    }

    protected void ResetFriction()
    {
        rigidbody2d.sharedMaterial = fullFriction;
    }
}

public enum CharacterType
{
    FemaleAdventurer, FemalePerson, MaleAdventurer, MalePerson, Robot, Zombie
}
