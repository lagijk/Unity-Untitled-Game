using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Enemy : MonoBehaviour
{
    public Animator anim;

    [Header ("Damages")] // Damage variable
    [SerializeField] private float attackDamage = 20f;
    [SerializeField] private float attackSpeed = 1f;
    private float canAttack;

    // Pathing variable
    public AIPath aiPath;

    // Knockback variable
    public float knockBackForce;

    void Start() {
        anim = GetComponent<Animator>(); 
    }

    void Update() {
        Facing();
        
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

    // Damages player when colliding (Attack method for enemy)
    private void OnCollisionStay2D(Collision2D other) {
        if (other.gameObject.tag == "Player") {
            if (attackSpeed <= canAttack) {
                anim.SetBool("isAttacking", true);
                other.gameObject.GetComponent<PlayerHealth>().TakeDamage(-attackDamage);
                canAttack = 0f;
                
                // Knockback player based on enemy position
                var player = other.gameObject.GetComponent<PlayerController>();
                player.knockBackCount = player.knockBackLength;

                if (other.transform.position.x < transform.position.x) {
                    player.knockFromRight = true;
                    
                }
                else {
                    player.knockFromRight = false;
                }

            }
            else {
                canAttack += Time.deltaTime;
            }
        }
    }

    // Knockback method for reciving damage from player bullet
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Bullet") {
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            Vector2 difference = (transform.position - other.transform.position).normalized;
            Vector2 force = difference * knockBackForce;
            rb.AddForce(force, ForceMode2D.Impulse);
            
        }
    }

    
    // Stop attacking function used in Unity animator
    void StopAttack() {
        if (anim.GetBool("isAttacking")) 
            anim.SetBool("isAttacking", false);
    }

}
