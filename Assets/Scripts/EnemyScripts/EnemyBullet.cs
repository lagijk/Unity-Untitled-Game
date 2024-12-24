using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    GameObject target;
    public Rigidbody2D rb;
    public float force; 

    public float attackDamage = 20;

    public GameObject bulletEffect;

    // Start is called before the first frame update
    void Start()
    {
        // Bullets shoot in the direction of player
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player");
        Vector2 rotation = (target.transform.position) - transform.position;
        rb.velocity = new Vector2(rotation.x, rotation.y).normalized * force;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Destroys bullet when colliding
    void OnTriggerEnter2D(Collider2D other) {
        GameObject effect = Instantiate(bulletEffect, transform.position, Quaternion.identity);
        switch(other.gameObject.tag) {
            case "Environment":
            Destroy(effect, 0.3f);
            Destroy(gameObject);
            break;

            case "Player":
            Destroy(effect, 0.3f);
            other.gameObject.GetComponent<PlayerHealth>().TakeDamage(-attackDamage);
            Destroy(gameObject);
            break;
            
            case "Enemy":
            Destroy(effect);
            break;

            case "Bullet":
            Destroy(effect, 0.3f);
            Destroy(gameObject);
            break;
        }
    }

   
}

