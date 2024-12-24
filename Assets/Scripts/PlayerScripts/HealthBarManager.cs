using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarManager : MonoBehaviour
{
    public GameObject heartPrefab;
    public PlayerHealth playerHealth;
    List<HealthHeart> hearts = new List<HealthHeart>();

    private void Start() 
    {
        DrawHearts();
    }

    // Method to draw hearts
    public void DrawHearts() {
        ClearHearts();  // Start with a fresh canvas

        float maxHealthRemainder = playerHealth.maxHealth % 2;
        int heartsToMake = (int)((playerHealth.maxHealth / 2) + maxHealthRemainder); 

        for (int i = 0; i < heartsToMake; i++) {
            CreateEmptyHeart();
        }

        // Creates the number of hearts based on currentHealth
        for (int i = 0; i < hearts.Count; i++) {
            int heartStatusRemainder = (int)Mathf.Clamp(playerHealth.currentHealth - (i * 2), 0, 2);
            hearts[i].SetHeartImage((HeartStatus)heartStatusRemainder);
        }
    }

    // Method to create an empty heart gameObject and add to hearts list
    public void CreateEmptyHeart() {
        GameObject newHeart = Instantiate(heartPrefab);
        newHeart.transform.SetParent(transform);
       

        HealthHeart heartComponent = newHeart.GetComponent<HealthHeart>();
        heartComponent.SetHeartImage(HeartStatus.Empty);
        hearts.Add(heartComponent);

    }

    // Method to clear all the heart gameObjects
    public void ClearHearts() {
        foreach (Transform t in transform) {
            Destroy(t.gameObject);
        }
        hearts = new List<HealthHeart>();
    }

}