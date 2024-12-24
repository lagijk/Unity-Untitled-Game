using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthHeart : MonoBehaviour
{
    // Reference to the heart sprites
    public Sprite fullHeart, halfHeart, emptyHeart;
    Image heartImage;

    private void Awake() {
        heartImage = GetComponent<Image>();
    }


    // Method that checks the health status and provides the appropriate sprite
    public void SetHeartImage(HeartStatus status) {
        switch(status) {
            case HeartStatus.Empty:
                heartImage.sprite = emptyHeart;
                break;
            case HeartStatus.Half:
                heartImage.sprite = halfHeart;
                break;
            case HeartStatus.Full:
                heartImage.sprite = fullHeart;
                break;
        }
    }
}

public enum HeartStatus 
{
    Empty = 0,
    Half = 1,
    Full = 2

}

