using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class GesturePathRenderer : MonoBehaviour {

    public float fadeAmount;

    bool fading = false;
    LineRenderer line;


    void Awake() {

        line = GetComponent<LineRenderer>();

        line.material = new Material(Shader.Find("Sprites/Default"));
        line.material.color = new Color(0.190f, 0.956f, 0.734f, 1);
    }

    void Update() {


        if (fading && line.material.color.a > 0) {

            line.material.color = new Color(line.material.color.r, line.material.color.g, line.material.color.b, line.material.color.a - fadeAmount);
        }
        else if (fading && line.material.color.a <= 0) {
            
            Destroy(gameObject);
        }
    }

    public void setColor(Color color) {

        line.material.color = color;
    }

    public void fade() {        
        fading = true;
    }

    public void fade(float amount) {

        fadeAmount = amount;
        fading = true;
    }
}