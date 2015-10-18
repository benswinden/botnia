using UnityEngine;
using System.Collections;

public class EnemyExample : StateMachineBase {

    #region Variables & Attributes

    // VARIABLES

    public GameObject body;                  // The container for the sprite of the nemy

    public bool canRotate = true;                // Can the ship be rotated
    public bool canMove = true;                  // Can the ship move

    protected Vector3 moveTarget;                    // Used when following
    protected bool movingPlayer;                    // Moving towards the player
    protected bool movingPosition;                  // Moving to a position

    protected float playerDistance;                 // Distance from this ship to the player, checked in e

    protected Quaternion rotationTowardsTarget;
    protected bool avoiding;

    protected Vector3 steeringDirection;

    protected bool findNewPosition;
    public Transform nose;


    // ATTRIBUTES
    public int health;

    protected float speed;                      // The speed this thing is currently traveling
    public float speedDelta;                // How much speed changes in acceleration / deceleration
    public float maxSpeed;                  // Max speed this thing can travel

    public float turnSpeed;                     // How fast this thing can turn

    public bool dropOnDeath = true;
    public bool explodeOnCollision;

    public float detectionRange = 18;              // Range at which the enemy will detect the player


    public GameObject explosionPrefab;                     // The explosion this makes on death
    public GameObject dropPrefab;                     // The object that is instantiated on this enemies death

    #endregion


    public enum States {
        idling = 0,
        attacking = 1,
        dying = 2,
    }


    void Start() {     
        _currentState = States.idling;
        _lastState = States.idling;

        // Start checking for player distance
        if (Global.player) playerDistance = Vector3.Distance(Global.player.transform.position, transform.position);
            StartCoroutine(checkPlayerDistance());

        OnStart();
    }
    protected virtual void OnStart() { }
    

    // Called at the beginning of all state updates, used for checks that should happen in all states    
    protected virtual void OnUpdate() {
        
        checkInput();        
    }

    // Used for checks & functions that should be done in all states, after all other update functions
    protected virtual void OnLateUpdate() {

        // Health
        if (health <= 0) {
            _currentState = States.dying;
        }

    }


    // STATES

    #region Idling

    IEnumerator idling_EnterState() {

        yield break;
    }

    void idling_Update() {
        OnUpdate();

        // MOVEMENT
        // Rotate                
        if (canRotate && (movingPlayer || movingPosition)) {

            transform.rotation = Quaternion.Slerp(transform.rotation, rotationTowardsTarget, Time.deltaTime * turnSpeed);
        }
    }

    void idling_FixedUpdate() {        

        if (canMove) {
            
            GetComponent<Rigidbody2D>().velocity = new Vector2(-transform.right.y * speed, transform.right.x * speed);

        }
    }

    void idling_LateUpdate() {
        OnLateUpdate();

    }

    #endregion   

    #region Attacking

    IEnumerator attacking_EnterState() {

        // ATTACK CODE
        _currentState = States.idling;

        yield break;
    }

    #endregion

    #region Dying

    IEnumerator dying_EnterState() {
        Instantiate(explosionPrefab, transform.position, transform.rotation);
        if (dropOnDeath)
            Instantiate(dropPrefab, transform.position, transform.rotation);
        Destroy(gameObject);
        yield break;
    }

    #endregion


    #region CHECKS

    protected void checkInput() {
        avoiding = false;

        if (findNewPosition) {
            findMovePosition();
        }

        if (Global.player) {            

            // When not following player: Check player position against detection range
            if (playerDistance <= detectionRange && !movingPlayer) {

                moveTarget = Global.player.gameObject.transform.position;
                movingPlayer = true;
                movingPosition = false;
            }
            else if (Random.value > 0.993) {
                
                findMovePosition();
            }


            // Following player: check against detect range
            if (playerDistance <= detectionRange * 1.5 && movingPlayer) {

                // Linecast to player
                RaycastHit2D hit = Physics2D.Linecast(transform.position, Global.player.transform.position, 1 << LayerMask.NameToLayer("Collidable"));

                // Clear line to player
                if (!hit) {
                    
                    moveTarget = Global.player.gameObject.transform.position;                    
                }

                // Obstructed line to player
                else {
                    
                        // Check distance to obstruction
                    // Obstruction is far away, keep moving
                    if (Vector2.Distance(new Vector2(transform.position.x, transform.position.y), hit.point) >  2) {

                        moveTarget = Global.player.gameObject.transform.position;                        
                    }
                    // Obstruction is close, move around
                    else {                        
                        avoiding = true;

                        // Check whether the enemy is running into a collidable, if so add 90 degress to their rotation
                        if (!Physics2D.Linecast(transform.position, nose.transform.position, 1 << LayerMask.NameToLayer("Collidable"))) {

                            rotationTowardsTarget = Quaternion.AngleAxis(Mathf.Atan2(steeringDirection.y, steeringDirection.x) * 180 / Mathf.PI - 90, transform.forward);
                        }
                        else {

                            findMovePosition();                          
                        }
                    }
                }
            }
            else if (playerDistance > detectionRange * 1.5 && movingPlayer) {

                movingPlayer = false;
                if (Random.value > 0.99)
                    findMovePosition();
            }
        }


        if (movingPosition) {

                // Check for collidables
            // Linecast to target
            RaycastHit2D hit = Physics2D.Linecast(transform.position, moveTarget, 1 << LayerMask.NameToLayer("Collidable"));

            if (hit) {

                // Obstruction is close, move around                
                avoiding = true;

                // Check whether the enemy is running into a collidable, if so add 90 degress to their rotation
                if (!Physics2D.Linecast(transform.position, nose.transform.position, 1 << LayerMask.NameToLayer("Collidable"))) {

                    speed = 0;
                    movingPosition = false;
                    rotationTowardsTarget = Quaternion.AngleAxis(Mathf.Atan2(steeringDirection.y, steeringDirection.x) * 180 / Mathf.PI - 90, transform.forward);
                }
                else {

                    movingPlayer = true;        // Make this true so distance to target position will be smaller
                    findMovePosition();
                }
            }

            // Check distance
            float distance = Vector3.Distance(moveTarget, transform.position);
            
            if (distance < 0.5) {
                movingPosition = false;                              
            }

        }


        if ((movingPlayer || movingPosition)) {

            if (!avoiding)
                rotationTowardsTarget = Quaternion.AngleAxis(Mathf.Atan2(moveTarget.y - transform.position.y, moveTarget.x - transform.position.x) * 180 / Mathf.PI - 90, transform.forward);

            if (speed < maxSpeed) {

                speed += speedDelta;
            }
        }
        else if (speed > 0) {

            speed -= speedDelta;
        }
    }

    public void findMovePosition() {
        

        findNewPosition = false;
        Vector3 randomPosition;
        int dist; if (movingPlayer) dist = 2; else dist = 8;

        randomPosition = new Vector3(transform.position.x + Random.Range(-dist, dist), transform.position.y + Random.Range(-dist, dist), transform.position.z);

        if (!Physics2D.Linecast(transform.position, randomPosition, 1 << LayerMask.NameToLayer("Collidable"))) {
            moveTarget = randomPosition;
            movingPlayer = false;
            movingPosition = true;
        }
        else {
            findNewPosition = true;
        }        
    }

    #endregion


    #region COLLISION

    void OnTriggerEnter2D(Collider2D other) {
        
        switch (other.tag) {

            case "Player":
                if (explodeOnCollision) {
                    
                    _currentState = States.dying;           // Explode
                }
                break;
        }
    }

    #endregion


    IEnumerator checkPlayerDistance() {

        yield return new WaitForSeconds(0.2F);

        if (Global.player) {
            playerDistance = Vector3.Distance(Global.player.transform.position, transform.position);

            StartCoroutine(checkPlayerDistance());
        }
    }

    // Set direction received from steering components
    void Move(Vector3 dir) {

        steeringDirection = dir;
    }
}
