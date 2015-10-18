using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;
using System.Reflection;

public abstract class StateMachineBase : MonoBehaviour {

    [HideInInspector]
	public new Transform transform;
    [HideInInspector]
	public new Rigidbody rigidbody;
    [HideInInspector]
	public new Animation animation;
    [HideInInspector]
	public CharacterController controller;
    [HideInInspector]
	public new NetworkView networkView;
    [HideInInspector]
	public new Collider collider;
    [HideInInspector]
	public new GameObject gameObject;

	void Awake() {
        gameObject = base.gameObject;
        collider = base.GetComponent<Collider>();
        transform = base.transform;
        animation = base.GetComponent<Animation>();
        rigidbody = base.GetComponent<Rigidbody>();
        networkView = base.GetComponent<NetworkView>();
        controller = GetComponent<CharacterController>();

        OnAwake();
	}
    protected virtual void OnAwake() { }

    public virtual void init() { }

	static IEnumerator DoNothingCoroutine() {
		yield break;
	}
	
	static void DoNothing() {
	}
	
	static void DoNothingCollider(Collider other) {
	}
	
	static void DoNothingCollision(Collision other) {
	}
	
	
	public Action DoUpdate = DoNothing;
	public Action DoLateUpdate = DoNothing;
	public Action DoFixedUpdate = DoNothing;
	public Action<Collider> DoOnTriggerEnter = DoNothingCollider;
	public Action<Collider> DoOnTriggerStay = DoNothingCollider;
	public Action<Collider> DoOnTriggerExit = DoNothingCollider;
	public Action<Collision> DoOnCollisionEnter = DoNothingCollision;
	public Action<Collision> DoOnCollisionStay = DoNothingCollision;
	public Action<Collision> DoOnCollisionExit = DoNothingCollision;
	public Action DoOnMouseEnter = DoNothing;
	public Action DoOnMouseUp = DoNothing;
	public Action DoOnMouseDown = DoNothing;
	public Action DoOnMouseOver = DoNothing;
	public Action DoOnMouseExit = DoNothing;
	public Action DoOnMouseDrag = DoNothing;
	public Action DoOnGUI = DoNothing;
	public Func<IEnumerator> ExitState = DoNothingCoroutine;
	
	private Enum currentState;
	
	public Enum _currentState {
		get
		{
			return currentState;
		}
		set
		{
			currentState = value;
			ConfigureCurrentState();			
		}
	}

    private Enum lastState;

    public Enum _lastState {
        get { return lastState; }
        set { lastState = value; }
    }
    

	
	void ConfigureCurrentState() {
		if(ExitState != null)
		{
			StartCoroutine(ExitState());
		}
		//Now we need to configure all of the methods
		DoUpdate = ConfigureDelegate<Action>("Update", DoNothing);
		DoOnGUI = ConfigureDelegate<Action>("OnGUI", DoNothing);
		DoLateUpdate = ConfigureDelegate<Action>("LateUpdate", DoNothing);
		DoFixedUpdate = ConfigureDelegate<Action>("FixedUpdate", DoNothing);
		DoOnMouseUp = ConfigureDelegate<Action>("OnMouseUp", DoNothing);
		DoOnMouseDown = ConfigureDelegate<Action>("OnMouseDown", DoNothing);
		DoOnMouseEnter = ConfigureDelegate<Action>("OnMouseEnter", DoNothing);
		DoOnMouseExit = ConfigureDelegate<Action>("OnMouseExit", DoNothing);
		DoOnMouseDrag = ConfigureDelegate<Action>("OnMouseDrag", DoNothing);
		DoOnMouseOver = ConfigureDelegate<Action>("OnMouseOver", DoNothing);
		DoOnTriggerEnter = ConfigureDelegate<Action<Collider>>("OnTriggerEnter", DoNothingCollider);
		DoOnTriggerExit = ConfigureDelegate<Action<Collider>>("OnTriggerExir", DoNothingCollider);
		DoOnTriggerStay = ConfigureDelegate<Action<Collider>>("OnTriggerEnter", DoNothingCollider);
		DoOnCollisionEnter = ConfigureDelegate<Action<Collision>>("OnCollisionEnter", DoNothingCollision);
		DoOnCollisionExit = ConfigureDelegate<Action<Collision>>("OnCollisionExit", DoNothingCollision);
		DoOnCollisionStay = ConfigureDelegate<Action<Collision>>("OnCollisionStay", DoNothingCollision);
		Func<IEnumerator> enterState = ConfigureDelegate<Func<IEnumerator>>("EnterState", DoNothingCoroutine);
		ExitState = ConfigureDelegate<Func<IEnumerator>>("ExitState", DoNothingCoroutine);
		EnableGUI();
		StartCoroutine(enterState());
		
		
	}
	
	
	T ConfigureDelegate<T>(string methodRoot, T Default) where T : class {
		var mtd = GetType().GetMethod(_currentState.ToString() + "_" + methodRoot, System.Reflection.BindingFlags.Instance 
			| System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.InvokeMethod);
		
		if(mtd != null)
		{
			return Delegate.CreateDelegate(typeof(T), this, mtd) as T;
		}
		else
		{
			return Default;
		}
		
	}
	
	
	
	protected void EnableGUI()
	{
		useGUILayout = DoOnGUI != DoNothing;
	}
	
	// Update is called once per frame
	void Update () 
	{
		DoUpdate();
	}
	
	void LateUpdate() 
	{
		DoLateUpdate();
	}
	
	void OnMouseEnter()
	{
		DoOnMouseEnter();
	}
	
	void OnMouseUp()
	{
		DoOnMouseUp();
	}
	
	void OnMouseDown()
	{
		DoOnMouseDown();
	}
	
	void OnMouseExit()
	{
		DoOnMouseExit();
	}
	
	void OnMouseDrag()
	{
		DoOnMouseDrag();
	}
	
	void FixedUpdate()
	{
		DoFixedUpdate();
	}
	void OnTriggerEnter(Collider other)
	{
		DoOnTriggerEnter(other);
	}
	void OnTriggerExit(Collider other)
	{
		DoOnTriggerExit(other);
	}
	void OnTriggerStay(Collider other)
	{
		DoOnTriggerStay(other);
	}
	void OnCollisionEnter(Collision other)
	{
		DoOnCollisionEnter(other);
	}
	void OnCollisionExit(Collision other)
	{
		DoOnCollisionExit(other);
	}
	void OnCollisionStay(Collision other)
	{
		DoOnCollisionStay(other);
	}
	void OnGUI()
	{
		DoOnGUI();
	}
	
	public IEnumerator WaitForAnimation(string name, float ratio)
	{
		var state = animation[name];
		return WaitForAnimation(state, ratio);
	}
	
	public static IEnumerator WaitForAnimation(AnimationState state, float ratio)
	{
		state.wrapMode = WrapMode.ClampForever;
		state.enabled = true;
		state.speed = state.speed == 0 ? 1 : state.speed;
		while(state.normalizedTime < ratio-float.Epsilon)
		{
			yield return null;
		}
	}
	
	public IEnumerator PlayAnimation(string name)
	{
		var state = animation[name];
		return PlayAnimation(state);
	}
	
	public static IEnumerator PlayAnimation(AnimationState state)
	{
		state.time = 0;
		state.weight = 1;
		state.speed = 1;
		state.enabled = true;
		var wait = WaitForAnimation(state, 1f);
		while(wait.MoveNext())
			yield return null;
		state.weight = 0;
		
	}

    public virtual void collision(Collider2D other) {

    }
}
