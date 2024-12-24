using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header ("Healthes")] 
    public Animator anim;
    public float maxHealth = 100f;
    private float currentHealth;
    
    
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        anim = GetComponent<Animator>(); 
    }

   
    // Method for enemy taking damage
    public void TakeDamage(float damage){
        currentHealth -= damage;

        anim.SetTrigger("hurt");

        if (currentHealth <= 0) {
            Die();
        }

    }

    // Death method
    void Die() {
        anim.SetBool("isDead", true);

        //Destroys enemy
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
        Destroy(gameObject, 0.5f);

    }
}
