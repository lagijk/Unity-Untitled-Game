using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public SpriteRenderer characterRenderer, weaponRenderer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Rotation weapon based on mouse position
        Vector3 rotation = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float rotateZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rotateZ);
        
        // Flip weapon when needed
        Vector2 scale = transform.localScale;
        if (rotation.x < 0) {
            scale.y = -1;
        }
        else if (rotation.x > 0) {
            scale.y = 1;
        }
        transform.localScale = scale;

        // Hides weapon when rotating across player's head
        if (transform.eulerAngles.z > 0 && transform.eulerAngles.z < 180) {
            weaponRenderer.sortingOrder = characterRenderer.sortingOrder - 1;
        }
        else {
            weaponRenderer.sortingOrder = characterRenderer.sortingOrder + 1;
        }
    }

}
