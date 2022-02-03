using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashUI : MonoBehaviour
{
    public Sprite flashOnSprite;
    public Sprite flashOffSprite;
    public float frequency = 1;

    private SpriteRenderer spriteRenderer;
    private bool flashOn;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        InvokeRepeating("changeFlash", 2, frequency);
    }

    void changeFlash()
    {
        flashOn = !flashOn;
        spriteRenderer.sprite = flashOn ? flashOnSprite : flashOffSprite;
    }
}
