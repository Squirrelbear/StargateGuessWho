using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashUI : MonoBehaviour
{
    public Sprite flashOnSprite;
    public Sprite flashOffSprite;
    public float frequency = 1;

    private Image image;
    private bool flashOn;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        InvokeRepeating("changeFlash", 2, frequency);
    }

    void changeFlash()
    {
        flashOn = !flashOn;
        image.sprite = flashOn ? flashOnSprite : flashOffSprite;
    }
}
