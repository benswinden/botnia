
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StateMachineExample : StateMachineBase {

    #region Variables


    // Audio    
    protected AudioSource source;                 // The audio source
    protected AudioClip[] clips;                  // An array of all clips that will be used in the audiosource   

    public enum States {
        idling = 0,
    }

    #endregion
        

    protected override void OnAwake() {
    }

    void Start() {    

        _currentState = States.idling;

        // Audio
        source = GetComponent<AudioSource>();

        // Load an array with audio from the resource folder
        clips = new AudioClip[1];
        clips[0] = (AudioClip)Resources.Load("_SOUNDS/SOUNDNAME");        
        source.clip = clips[0];
        source.volume = 0.2F;
        source.loop = false;

        OnStart();
    }
    protected virtual void OnStart() { }


    void Update() {

        OnUpdate();
    }
    protected virtual void OnUpdate() { }

    // Used for checks & functions that should be done in all states, after all other update functions
    void LateUpdate() {

        OnLateUpdate();
    }
    protected virtual void OnLateUpdate() { }

    void FixedUpdate() {

        OnFixedUpdate();
    }
    protected virtual void OnFixedUpdate() { }

    // STATES

    #region Idling

    IEnumerator idling_EnterState() {
        yield break;
    }

    void idling_Update() {
        OnUpdate();
    }

    void idling_FixedUpdate() {
        OnFixedUpdate();
    }

    void idling_LateUpdate() {
        OnLateUpdate();        
    }    

    IEnumerator idling_ExitState() {
        _lastState = States.idling;
        yield break;
    }

    #endregion    
}