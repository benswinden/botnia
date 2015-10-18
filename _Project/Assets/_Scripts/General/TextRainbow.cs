using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class TextRainbow : MonoBehaviour {

    private HSBColor hsbColor;
    private bool lerpUp = true;

    void Start() {

        hsbColor = new HSBColor(Random.Range(0, 1.0f), 1, 1, GetComponent<Text>().color.a);
    }

    void Update() {

        if (lerpUp && hsbColor.h >= 0.99)
            lerpUp = false;
        else if (!lerpUp && hsbColor.h <= 0.01)
            lerpUp = true;

        if (lerpUp)
            hsbColor.h = hsbColor.h + 0.005f;
        else
            hsbColor.h = hsbColor.h - 0.005f;

        GetComponent<Text>().color = HSBColor.ToColor(hsbColor);
    }
}
