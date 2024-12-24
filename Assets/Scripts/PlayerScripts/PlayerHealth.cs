using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerHealth : MonoBehaviour
{
    public Animator anim;

    [Header ("Healthes")] // Variables for health
    public float maxHealth = 10f;
    public float currentHealth = 0f; 

    public HealthBarManager healthBarManager;

    [Header ("Immunes")] // Variable for invulnerability
    private bool invincible = false;
    [SerializeField] private float invincibilityTime = 1f;


    
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        anim = GetComponent<Animator>(); 
    }

    // Update is called once per frame
    void Update()
    {
        // Display the correct number of hearts based on current Health
        healthBarManager.DrawHearts();
    }

    // Player taking damage method
    public void TakeDamage(float damage){
        currentHealth += damage;
        anim.SetTrigger("playerHurt");
        StartCoroutine(Invulnerability());

        if (currentHealth > maxHealth) {
            currentHealth = maxHealth;
        }
        
        else if (currentHealth <= 0f) {
            currentHealth = 0f; //Set player currentHealth to 0, prevent negative currentHealth
            anim.SetBool("playerIsDead", true);
            Destroy(gameObject, 2f);
        }

    }

    
    // Player invincible temporarily after taking damage

    private IEnumerator Invulnerability() {
        invincible = true;
        Physics2D.IgnoreLayerCollision(9, 7, true);
        yield return new WaitForSeconds(invincibilityTime);
        invincible = false;
        Physics2D.IgnoreLayerCollision(9, 7, false);
    }

    
}
