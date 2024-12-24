using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunScript : MonoBehaviour
{

    public GameObject bullet;
    public Transform firePoint;


    // Weapon shooting method
     public void Shoot() {
        Instantiate(bullet, firePoint.position, firePoint.rotation);
    }

    

}
