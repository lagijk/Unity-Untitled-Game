using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Starter classes
    public Rigidbody2D rb;
    public Animator anim;

    // Reference to gunscript in gun
    public GunScript gunScript;

    [Header ("Movements")] // Movement variables
    public float moveSpeed = 5f;
    private Vector2 moveDirection;
    private Vector2 lastMoveDirection;


    [Header ("Dashes")] // Dash variables
    [SerializeField] float dashSpeed = 10f;
    [SerializeField] float dashDuration = 1f;
    [SerializeField] float dashCooldown = 1f;
    bool isDashing;
    bool canDash = true;

    [Header ("Attacks")] // Attack variables
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange;
    [SerializeField] private LayerMask attackMask;
    public float attackDamage = 25;

    // Prevents attack spam
    public float attackRate = 2f;
    float nextAttackTime = 0f;

    [Header ("Knockbacks")] // Knockback variables
    public float knockBackForce;
    public float knockBackLength;
    public float knockBackCount;
    public bool knockFromRight;

    [Header ("Shootings")] // Shooting variables
    public float shootRate = 1f;
    float nextShotTime = 0f;




    //  private void Awake() {
    //      weaponController = GetComponentInChildren<WeaponController>();
    //  }

    // Start is called before the first frame update
    void Start() {
        rb = gameObject.GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>(); 
        gunScript.GetComponent<Renderer>().enabled = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        // Movements
        ProcessInputs();
        Animate();

        if (isDashing) {
            return;
        }

        // Attack check and prevents spam
        if (Time.time >= nextAttackTime) {
            if (Input.GetMouseButtonDown(0)) {
                Attack();
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
        

        // Dashing check
        if (Input.GetKeyDown(KeyCode.Space) && canDash) {
            StartCoroutine(Dash());
            anim.SetBool("isDashing", true);        
        }

        // Shooting check and hides gun after shooting, also prevents bullet spam
         if (Input.GetMouseButtonDown(1) && Time.time > nextShotTime) {
            nextShotTime = Time.time + shootRate;
            StartCoroutine(HideAfter());
            gunScript.Shoot();
        }
       
        
    }

    // Consistent update for user inputs
    void FixedUpdate() {

        // Physics Calculations

        if (isDashing) {
            return;
        }

        // Prevents player from moving when attacking
        if (anim.GetBool("isAttacking")) {
            rb.velocity = Vector2.zero;
        }
        else {
            Move();
        }

        // Flip hitbox
        if (moveDirection.x < 0) {
            attackPoint.transform.localPosition = new Vector3(-0.65f, 0, 0);
        }
        else if (moveDirection.x > 0) {
            attackPoint.transform.localPosition = new Vector3(0.65f, 0, 0);
        }
        else if (moveDirection.y < 0) {
            attackPoint.transform.localPosition = new Vector3(0, -0.65f, 0);
        }
        else if (moveDirection.y > 0) {
            attackPoint.transform.localPosition = new Vector3(0, 0.65f, 0);
        }
        

    }

    // Attack method
    void Attack() {
        anim.SetBool("isAttacking", true);

        // Enemy detection in range of attack
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, attackMask);
        
        foreach(Collider2D enemy in hitEnemies) {
            enemy.GetComponent<EnemyHealth>().TakeDamage(attackDamage);
        }
    }
    

    // Visualization for hitbox
    private void OnDrawGizmos() {
         if (attackPoint == null) {
             return;
         }
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    // Stop attacking function used in Unity animator
    void StopAttack() {
        if (anim.GetBool("isAttacking")) 
            anim.SetBool("isAttacking", false);
    }

  
    // Method for movement input
    void ProcessInputs() {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        // checks condition for player's last position and stays there
        if ((moveX == 0 && moveY == 0) && moveDirection.x != 0 || moveDirection.y != 0) {
            lastMoveDirection = moveDirection;
        }

        moveDirection = new Vector2(moveX, moveY).normalized; //makes movement consistent to 1

    }

    // Movement velocity method and player knockback
    void Move() {
        if (knockBackCount <= 0) {
            rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
        }
        else {
            if (knockFromRight) {
                rb.velocity = new Vector2(-knockBackForce, 0);
            }
            if (!knockFromRight) {
                rb.velocity = new Vector2(knockBackForce, 0);
            }
            knockBackCount -= Time.deltaTime;
        }

    }

    // Movement animations method
    void Animate() {
        // movement animations
        anim.SetFloat("AnimMoveX", moveDirection.x);
        anim.SetFloat("AnimMoveY", moveDirection.y);
        anim.SetFloat("AnimMoveMag", moveDirection.magnitude);
        anim.SetFloat("AnimLastMoveX", lastMoveDirection.x);
        anim.SetFloat("AnimLastMoveY", lastMoveDirection.y);

    }

    // Dash method, ignores damage during dash
    private IEnumerator Dash() {
        canDash = false;
        isDashing = true;
        Physics2D.IgnoreLayerCollision(9, 7, true);
        rb.velocity = new Vector2(moveDirection.x * dashSpeed, moveDirection.y * dashSpeed);
        yield return new WaitForSeconds(dashDuration);
        isDashing = false;
        Physics2D.IgnoreLayerCollision(9, 7, false);
        anim.SetBool("isDashing", false);

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
    
    // Hides gun with a timer
    private IEnumerator HideAfter() {
        gunScript.GetComponent<Renderer>().enabled = true;
        yield return new WaitForSeconds(2);
        gunScript.GetComponent<Renderer>().enabled = false;
    }
}
