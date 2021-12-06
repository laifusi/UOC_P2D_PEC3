using UnityEngine;

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
    [SerializeField] TeamColor teamColor;
    [SerializeField] Transform hand;

    private Rigidbody2D rigidbody2d;
    private bool isGrounded;
    private float slopeAngle;
    private Vector2 slopeNormalPerpendicular;
    private bool isOnSlope;
    private float previousSlopeAngle;
    private RaycastHit2D hit;
    private bool isJumping;
    private bool canJump;
    private float horizontalInput;
    private WeaponSO currentWeapon;
    private int currentWeaponIndex;

    private void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        currentWeapon = weapons[0];
        currentWeaponIndex = 0;
        ChangeWeapon();
    }

    private void Update()
    {
        CheckIsGrounded();

        horizontalInput = Input.GetAxis("Horizontal");

        GetSlopeInfo();

        Move();

        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }
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
    }

    public void ChangeWeapon()
    {
        if(currentWeaponIndex + 1 >= weapons.Length)
        {
            currentWeaponIndex = 0;
        }
        else
        {
            currentWeaponIndex++;
        }
        currentWeapon = weapons[currentWeaponIndex];
        GameObject weapon = Instantiate(currentWeapon.Prefab, hand.position, hand.rotation, transform);
        Weapon weaponComponent = weapon.GetComponent<Weapon>();
        weaponComponent.Projectile = currentWeapon.Projectile;
        SpriteRenderer weaponSpriteRenderer = weapon.GetComponent<SpriteRenderer>();
        weaponSpriteRenderer.sprite = currentWeapon.SetSprite(teamColor);
    }
}
