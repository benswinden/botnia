using UnityEngine;
using InControl;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class TwinStickExample : MonoBehaviour {

    public float speed = 375f;
    public float turnSpeed = 10f;

      
    InputDevice device;

    GameObject cameraTargetParent;
    GameObject body;

    // Audio    
    protected AudioSource source;                 // The audio source
    protected AudioClip[] clips;                  // An array of all clips that will be used in the audiosource   


    void Awake() {

        //Global.player = this;    
    }

    void Start() {

        if (transform.Find("CameraTargetParent")) {

            Global.cameraController.smoothFollowTarget = transform.Find("CameraTargetParent/CameraTarget").gameObject;
            cameraTargetParent = transform.Find("CameraTargetParent").gameObject;
        }

        body = transform.Find("Body").gameObject;
    }

    void Update() {

        device = InputManager.ActiveDevice;

        checkInput();
    }

    public void checkInput() {

        if (Mathf.Abs(device.RightStick.X) > 0.3f || Mathf.Abs(device.RightStick.Y) > 0.3f) {

            Quaternion rotationTowardsTarget = Quaternion.AngleAxis(Mathf.Atan2((device.RightStick.Vector.y * 50) - cameraTargetParent.transform.position.y, (device.RightStick.Vector.x * 50) - cameraTargetParent.transform.position.x) * 180 / Mathf.PI - 90, cameraTargetParent.transform.forward);

            cameraTargetParent.transform.rotation = Quaternion.Slerp(cameraTargetParent.transform.rotation, rotationTowardsTarget, Time.deltaTime * turnSpeed);
        }
        else if (Mathf.Abs(device.LeftStick.X) > 0.1f || Mathf.Abs(device.LeftStick.Y) > 0.1f) {

            Quaternion rotationTowardsTarget = Quaternion.AngleAxis(Mathf.Atan2((device.LeftStick.Vector.y * 50) - cameraTargetParent.transform.position.y, (device.LeftStick.Vector.x * 50) - cameraTargetParent.transform.position.x) * 180 / Mathf.PI - 90, cameraTargetParent.transform.forward);

            cameraTargetParent.transform.rotation = Quaternion.Slerp(cameraTargetParent.transform.rotation, rotationTowardsTarget, Time.deltaTime * turnSpeed);
            body.transform.rotation = Quaternion.Slerp(body.transform.rotation, rotationTowardsTarget, Time.deltaTime * turnSpeed);
        }

        //    body.transform.Rotate(0, 0, Vector2.Angle(Vector2.zero, device.RightStick.Vector) / 2);

        GetComponent<Rigidbody2D>().AddForce(new Vector2(device.LeftStick.X * speed, device.LeftStick.Y * speed));

    }    

    #region COLLISION

    void OnTriggerEnter2D(Collider2D other) {

        switch (other.tag) {

            case "Enemy":
                

                break;
        }
    }

    #endregion

}
