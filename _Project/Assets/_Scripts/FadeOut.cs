using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class FadeOut : MonoBehaviour {

    public float fadeAmount;
    SpriteRenderer spriteRenderer;

    void Awake() {

        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update() {


        if (spriteRenderer.color.a > 0) {

            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, spriteRenderer.color.a - fadeAmount);
        }
        else if (spriteRenderer.color.a <= 0) {

            Destroy(gameObject);
        }
    }
}