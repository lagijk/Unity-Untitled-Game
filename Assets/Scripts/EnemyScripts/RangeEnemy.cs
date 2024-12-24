using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class RangeEnemy : MonoBehaviour
{
    
    private Transform player;
    public GameObject bullet;
    public Transform firePoint;
    public Animator anim;

    [Header ("Damages")] // Damage variable
    [SerializeField] private float shootingRange;
    [SerializeField] private float lineOfSite;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float nextAttacktime;
    [SerializeField] private float moveSpeed;


    // Pathing variable
    public AIPath aiPath;

    void Start() {

        player = GameObject.FindGameObjectWithTag("Player").transform;
        anim = GetComponent<Animator>(); 
        
    }

    void Update() {
        Facing();
        
        // Move toward player position
        float distanceFromPlayer = Vector2.Distance(player.position, transform.position);
        if (distanceFromPlayer < lineOfSite && distanceFromPlayer > shootingRange) {
            transform.position = Vector2.MoveTowards(this.transform.position, player.position, moveSpeed * Time.deltaTime);
        }
        // Shoots bullet at player
        else if (distanceFromPlayer <= shootingRange && nextAttacktime < Time.time) {
            anim.SetBool("isAttacking", true);
            Instantiate(bullet, firePoint.transform.position, Quaternion.identity);
            nextAttacktime = Time.time + attackSpeed;
        }
        
    }

    // Change enemy direction to face toward player (pathfinding)
    void Facing() {
        anim.SetBool("isMoving", true);
        if (aiPath.desiredVelocity.x >= 0.01f) {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if (aiPath.desiredVelocity.x <= -0.01f) {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }

    // Visualization for attack range
    private void OnDrawGizmos() {
        Gizmos.DrawWireSphere(transform.position, lineOfSite);
        Gizmos.DrawWireSphere(transform.position, shootingRange);
    }
    
    // Stop attacking function used in Unity animator
    void StopAttack() {
        if (anim.GetBool("isAttacking")) 
            anim.SetBool("isAttacking", false);
    }

}
