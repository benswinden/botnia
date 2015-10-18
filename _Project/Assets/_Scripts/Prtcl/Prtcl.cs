/*
 * PRTCL - Particle
 * 
 * Base class for any object that will need to be spawned from a generator
 *  
 */ 

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Prtcl : MonoBehaviour {

    #region Variables

    // VARIABLES    

    float time;

    float vX;                               // Velocity along the x - axis
    float vY;                               // Velocity along the y - axis

    float initAlpha;                        // Initial alpha

    GoTween scaleTween;


    // SETTINGS

    public SpriteRenderer sprite;               // A child object that contains the prtcl's sprite, this is so that rotation is independant of movement direction
            
    public List<Sprite> spriteList;             // If sprites are given, randomly pick one    

    public bool maintainRotation;
    public bool randomStartRotation;            // Whether this PRTCLs starting rotation should be randomized

    public bool moveAwayFromContact;            // If true, all particles will move away from the direction in which the object that instantiated them was facing    

    public bool __________________;

    public bool dies;                          // Decides whether the prtcl kills itself after a set amount of time. Only applies when not fading out
    public float lifetimeMin;
    public float lifetimeMax;

    public bool ___________________;

    public bool speedRandomization;             // Adds some randomization to the speed    
    public float speed;                         // Speed at which this object moves    

    public bool _____________________;

    public float alpha;                         // The objects starting alpha value, defaults to 1

    public bool fadeIn;                         // Fade this object in
    public bool fadeOut;                        // Whether or not this will fade    
    public float fadeTime;
    public float fadeStartTime;                 // Time before fading starts, if this prtcl fades in, this timer will not start until fully faded in

    public bool ____________________;

    public bool scaleUp;                        // Starts at 0 scale and goes up infinitely
    public float scaleTime = 2;
    public float scaleUpTo;                     // The value to scale this object up to

    public bool ______________________;

    public bool randomScale;                    // Object starts at a random scale between minScale and maxScale
    public float scale = 1;                     // Scale of the sprite
    public float minScale;
    public float maxScale;
    

    #endregion


    void Awake() {    

        if (alpha == 0) {
            alpha = 1.0f;
            initAlpha = 1.0f;
        }
        else {
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, alpha);
            initAlpha = alpha;
        }

        if (fadeIn) {
            alpha = 0;
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, alpha);
        }

        time = Time.time;        

        // Random 360 degree rotation
        Quaternion randomRotation = Quaternion.Euler(0, 0, Random.Range(0, 360));        


        if (moveAwayFromContact) {

            Vector3 dir = transform.right ;

            int ranDegree = Random.Range(0, 45);
            int ranNum = 0; if (Random.value < 0.5f) ranNum = 1; else ranNum = -1;

            // Rotation based on the the initial rotation
            randomRotation = Quaternion.AngleAxis(Mathf.Atan2(dir.y, dir.x) * 180 / Mathf.PI + (ranDegree * ranNum) - 90, transform.forward);            
        }

        // Rotate
        if (!maintainRotation)
            transform.rotation = randomRotation;

        // Speed randomization
        if (speedRandomization) {

            int ranNum = 0; if (Random.value < 0.5f) ranNum = 1; else ranNum = -1;
            speed += ( speed * Random.Range(0.0f, 0.2f) * ranNum) ;
        }

        // Alter fade time slightly to give some differentiation
        fadeTime += fadeTime * Random.Range(0.0f, 0.2f);

        //Decide when it should die
        if (dies)
            StartCoroutine(destroySelf(Random.Range(lifetimeMin, lifetimeMax)));
    }

    void Start() {    

        // Set the sprite
        if (spriteList.Count != 0) {
            sprite.sprite = spriteList[Random.Range(0, spriteList.Count - 1)];
        }

        // Set Scale
        if (randomScale) {
            float ranScale = Random.Range(minScale, maxScale);
            transform.localScale = new Vector3(ranScale, ranScale, ranScale);
        }
        else if (scale != 1)
            transform.localScale = new Vector3(scale, scale, scale);

        // Set start of rotation of sprite 
        if (randomStartRotation) sprite.gameObject.transform.Rotate(0, 0, Random.Range(0, 360));

        if (scaleUp) {
            
        scaleTween = Go.to(transform, scaleTime, new GoTweenConfig()
                .scale(new Vector3(scaleUpTo, scaleUpTo, 1))
                .setEaseType(GoEaseType.SineInOut));
        }
    }

    void Update() {    

        transform.Translate(transform.right * speed * Time.deltaTime);

        // FADE
        if (fadeIn) {            
            if (alpha < initAlpha) {
                sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, alpha);
                alpha = Mathf.Lerp(0, initAlpha, (Time.time - time) / fadeTime);
            }
            else {
                time = Time.time;
                fadeIn = false;
            }
        }  
        else if (fadeOut) {
            if (fadeStartTime != 0) {
                fadeOut = false;
                StartCoroutine(waitForFade());
            }
            else {
                
                if (alpha > 0) {
                    sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, alpha);
                    alpha = Mathf.Lerp(initAlpha, 0, (Time.time - time) / fadeTime);
                }
                else {

                    if (scaleTween != null)
                        scaleTween.destroy();

                    Destroy(gameObject);
                }
            }
        }  
    }

    public void resetTime() {

        time = Time.time;
    }

    IEnumerator waitForFade() {        

        yield return new WaitForSeconds(fadeStartTime);

        time = Time.time;
        fadeStartTime = 0;
        fadeOut = true;
    }

    IEnumerator destroySelf(float time) {

        yield return new WaitForSeconds(time);

        if (scaleTween != null)
            scaleTween.destroy();

        Destroy(gameObject);
    }
}