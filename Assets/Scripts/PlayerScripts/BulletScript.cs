using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{

    private Vector3 mousePosition;
    private Camera mainCamera; 
    public Rigidbody2D rb;
    public float force; 

    public float attackDamage = 30;

    public GameObject bulletEffect;

    // Start is called before the first frame update
    void Start()
    {
        // Bullets shoot in the direction of mouse cursor
        Vector3 rotation = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(rotation.x, rotation.y).normalized * force;
        float rotateZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rotateZ);
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

            case "Enemy":
            Destroy(effect, 0.3f);
            other.gameObject.GetComponent<EnemyHealth>().TakeDamage(attackDamage);
            Destroy(gameObject);
            break;

            case "EBullet":
            Destroy(effect, 0.3f);
            Destroy(gameObject);
            break;
            
        }
    }

    
}
