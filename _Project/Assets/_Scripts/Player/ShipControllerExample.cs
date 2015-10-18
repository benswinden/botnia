using UnityEngine;
using UnityEngine.UI;
using InControl;
using System.Collections;
using System.Collections.Generic;


public class ShipControllerExample : StateMachineBase {
     
    #region Variables & Attributes

    // VARIABLES

    protected float vY;
    protected float vX;
    protected float driftvY;
    protected float driftvX;

    protected bool maxVelocityActive = true;

    protected float thrustAmount;                   // The amount of thrust the controller is applying to accelerate or deccelerate
    protected float thrustAcceleration = 0.01F;             // The amount of thrust increases
    protected float dragAmount;                     // Amount of drag being applied to the ship. default is always equal to friction

    protected float rotateAxis;                     // Value of the joystick axis used to rotate the ship

    public bool canRotate = true;                // Can the ship be rotated
    public bool canMove = true;                  // Can the ship move    
    public bool canControl = true;

    Quaternion rotationTowardsTarget;

    protected float friction;

    protected bool collided = false;

    protected bool stopping = false;

    protected float lastX;

    protected bool triggerOff;              // Used for controlling automatic vs non auto guns, sent as a bool to the fire method, true when it is the first time calling fire after having the finger off the trigger

    public GameObject cameraTarget;

    // ATTRIBUTES

    protected bool canDrift = true;                // Can the ship drift   

    protected float velocity;
    protected float maxVelocity;
    protected float minVelocity;

    protected float turnSpeed;
    protected float acceleration;
    protected float deceleration;    

    protected float shakeFactor;


    // Audio    
    protected AudioSource source;                 // The audio source
    protected AudioClip[] clips;                  // An array of all clips that will be used in the audiosource   

    public enum States {
        idling = 0,
        thrusting = 1,
        reversing = 2,
        drifting = 3,
        collided = 4,
        dying = 5,
    }


    #endregion

    protected override void OnAwake() {
    }

    void Start() {    

        _currentState = States.idling; 

        setCameraTarget();

        // Audio
        source = GetComponent<AudioSource>();

        // Load an array with audio from the resource folder
        clips = new AudioClip[3];
        clips[0] = (AudioClip)Resources.Load("_SOUNDS/Ship/crash3");
        clips[1] = (AudioClip)Resources.Load("_SOUNDS/Effects/warp");
        clips[2] = (AudioClip)Resources.Load("_SOUNDS/Objects/dock");
        source.clip = clips[0];
        source.volume = 0.2F;
        source.loop = false;

        OnStart();
    }
    protected virtual void OnStart() { }

    void Update() {

        checkInput();

        OnUpdate();
    }
    protected virtual void OnUpdate() { }

    // Used for checks & functions that should be done in all states, after all other update functions
    void LateUpdate() {    

        if (maxVelocityActive) {
            // Max Velocity
            if (GetComponent<Rigidbody2D>().velocity.magnitude > maxVelocity) {
                GetComponent<Rigidbody2D>().velocity = Vector2.ClampMagnitude(GetComponent<Rigidbody2D>().velocity, maxVelocity);
            }
            else if (GetComponent<Rigidbody2D>().velocity.magnitude < -maxVelocity) {
                GetComponent<Rigidbody2D>().velocity = Vector2.ClampMagnitude(GetComponent<Rigidbody2D>().velocity, -maxVelocity);
            }

            if (velocity > maxVelocity)
                velocity = maxVelocity;
            if (velocity < -maxVelocity)
                velocity = -maxVelocity;
        }

        // Check for movement on z-axis and correct
        if (transform.position.z != 0)
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);                
      
            //ACTIONS        
        if (stopping) {
            stop();
        }

        OnLateUpdate();
    }
    protected virtual void OnLateUpdate() { }

    void FixedUpdate() {    

        var v3 = Input.mousePosition;
        v3.z = -(transform.position.x - Camera.main.transform.position.x);
        v3 = Camera.main.ScreenToWorldPoint(v3);        

        rotationTowardsTarget = Quaternion.AngleAxis(Mathf.Atan2(v3.y - transform.position.y, v3.x - transform.position.x) * 180 / Mathf.PI - 90, transform.forward);

        if (canRotate)
            transform.rotation = Quaternion.Slerp(transform.rotation, rotationTowardsTarget, Time.deltaTime * turnSpeed);

        OnFixedUpdate();
    }
    protected virtual void OnFixedUpdate() { }

    public void setCameraTarget() {

        if (cameraTarget) {
            Global.cameraController.smoothFollowTarget = cameraTarget;
        }
    }

    // STATES

    #region Idling

    IEnumerator idling_EnterState() {
        yield break;
    }

    void idling_Update() {
        OnUpdate();

        // ACTIONS
        checkForFire();
    }

    void idling_FixedUpdate() {
        OnFixedUpdate();

        // Movement
        if (canMove) {

            GetComponent<Rigidbody2D>().drag = dragAmount;
            //rigidbody2D.velocity = new Vector2(-transform.right.y * velocity, transform.right.x * velocity);

        }
    }

    void idling_LateUpdate() {
        OnLateUpdate();

        // Apply friction to the ship
        if (velocity > 0)
            velocity -= friction;
    }

    IEnumerator idling_ExitState() {
        _lastState = States.idling;
        yield break;
    }

    #endregion

    #region Thrusting

    IEnumerator thrusting_EnterState() {
        yield break;
    }

    void thrusting_Update() {
        OnUpdate();

        // ACTIONS
        checkForFire();
    }

    void thrusting_FixedUpdate() {
        OnFixedUpdate();

        if (canMove) {

            velocity += acceleration * thrustAmount;

            // Movement                        
            GetComponent<Rigidbody2D>().AddForce(new Vector2(-transform.right.y * velocity * 3.5F, transform.right.x * velocity * 3.5F));
                        
            GetComponent<Rigidbody2D>().drag = dragAmount;

            lastX = transform.right.x;
        }
    }

    void thrusting_LateUpdate() {
        OnLateUpdate();
    }

    IEnumerator thrusting_ExitState() {
        _lastState = States.thrusting;
        yield break;
    }

    #endregion

    #region Reversing

    IEnumerator reversing_EnterState() {
        yield break;
    }

    void reversing_Update() {
        OnUpdate();


        // ACTIONS
        checkForFire();
    }

    void reversing_FixedUpdate() {
        OnFixedUpdate();

        if (canMove) {

            velocity += acceleration * thrustAmount;

            // Check for the player reversing directions           

            // Movement                        
            GetComponent<Rigidbody2D>().AddForce(new Vector2(-transform.right.y * velocity * 3.5F, transform.right.x * velocity * 3.5F));

            GetComponent<Rigidbody2D>().drag = dragAmount;

            lastX = transform.right.x;
        }
    }

    void reversing_LateUpdate() {
        OnLateUpdate();
    }

    IEnumerator reversing_ExitState() {
        _lastState = States.thrusting;
        yield break;
    }

    #endregion

    #region Drifting

    IEnumerator drifting_EnterState() {

        // Don't set drift velocities if coming from collision state since they will already be set
        if (!_lastState.Equals(States.collided)) {
            driftvX = transform.right.x;
            driftvY = -transform.right.y;
        }

        yield break;
    }

    void drifting_Update() {
        OnUpdate();


        // ACTIONS
        checkForFire();
    }

    void drifting_FixedUpdate() {
        OnFixedUpdate();

        if (canMove) {

            if (velocity > minVelocity) {                           // Only deccelerate while above the minimum
                velocity += deceleration * thrustAmount;
            }

            
            // Movement   

            GetComponent<Rigidbody2D>().drag = dragAmount;            
        }
    }

    void drifting_LateUpdate() {
        OnLateUpdate();

        // Apply friction to the ship
        if (velocity > 0)
            velocity -= friction;
    }

    IEnumerator drifting_ExitState() {
        _lastState = States.drifting;
        yield break;
    }

    #endregion

    #region Collided

    IEnumerator collided_EnterState() {

        yield return new WaitForSeconds(0.2F);

        _currentState = States.drifting;

    }

    void collided_Update() {

        // Movement
        GetComponent<Rigidbody2D>().velocity = new Vector2(driftvY * velocity, driftvX * velocity);
    }

    IEnumerator collided_ExitState() {
        _lastState = States.collided;
        yield break;
    }

    #endregion

    #region Dying

    IEnumerator dying_EnterState() {
                                
        Destroy(gameObject);        

        yield break;
    }

    #endregion


    #region CONTROLS

    protected void checkInput() {

        // Use last device which provided input.
        //var inputDevice = InputManager.ActiveDevice;

        // Rotation Input        
        if (Input.GetAxis("Horizontal") != 0 && canControl)              // Receiving left joystick input
            rotateAxis = Input.GetAxis("Horizontal");
        else
            rotateAxis = 0;


        // Thrust Input        
        if (Input.GetAxis("3rd-axis") < 0 && canControl) {                    // Apply thrust

            thrustAmount = -Input.GetAxis("3rd-axis");
        }
        else if (Input.GetAxis("3rd-axis") > 0 && canControl) {              // If canReverse, apply negative force

            thrustAmount = -Input.GetAxis("3rd-axis");
        }
        else if (Input.GetAxis("3rd-axis") > 0 && canControl) {                           // Otherwise, apply drag to stop the ship

                dragAmount += deceleration;                     
        }        
                                

        // Reset values in special cases        
        if (thrustAmount > 0.79 && dragAmount > 0 && _currentState.Equals(States.drifting)) {
            dragAmount = friction;
        }


        // Don't change current state if: warping or dying
        if (!_currentState.Equals(States.dying)) {
            
            // Check the engine state
            // Left trigger and not already thrusting
            if ((thrustAmount > 0) && dragAmount <= friction &&  !_currentState.Equals(States.thrusting))
                _currentState = States.thrusting;
            else if ((thrustAmount < 0) && !_currentState.Equals(States.reversing))
                _currentState = States.reversing;
            // No trigger, in thrusting state and not currently idling
            else if (thrustAmount == 0 && dragAmount <= friction && (_currentState.Equals(States.thrusting) || _currentState.Equals(States.reversing) || _currentState.Equals(States.idling)))       // Only move into idling state from thrusting
                _currentState = States.drifting;
            // Right trigger and not currently drifting
            //else if (dragAmount > 0 && !_currentState.Equals(States.drifting) && canDrift)
            //    _currentState = States.drifting;

        }        

    }


    #endregion

    #region COLLISION

    public override void collision(Collider2D other) {
        
        switch (other.tag) {

            case "Collidable":
                
                if (GetComponent<Rigidbody2D>().velocity.magnitude > 0.7f) {

                    // Play crash sound
                    if (!source.isPlaying) {
                        source.volume = 0.025f;
                        source.clip = clips[0];
                        source.Play();
                    }

                    // Screenshake
                    Global.cameraController.screenShake(shakeFactor);
                }

                break;
        }
    }

    #endregion

    #region FUNCTIONS

    // Screenshake*
    public virtual void shake() {

        Global.cameraController.screenShake(shakeFactor);
    }

    // Used to stop the ship. Called every update until the ship stops
    public virtual void stop() {

        if (GetComponent<Rigidbody2D>().velocity.magnitude > 0 && !stopping) {

            stopping = true;
        }
        else if (GetComponent<Rigidbody2D>().velocity.magnitude > 0) {
            
            dragAmount += deceleration;         // Slower than normal deceleration for dramatic effect
        }
        else {

            stopping = false;
            GetComponent<Rigidbody2D>().drag = 0;
        }
    }


    #endregion
    
    #region PLAYER ACTIONS

    // Check for default fire
    public void checkForFire() {

        if (Input.GetMouseButton(0) && canControl) {

            if (triggerOff) {
                //ship.fire(true);
            }
            else
                //ship.fire(false);

                triggerOff = false;
        }
        else {
            triggerOff = true;
        }
    }


    #endregion

}
