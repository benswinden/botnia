using System;
using InControl;
using UnityEngine;


public class PlayerActions : PlayerActionSet {
	
	public PlayerAction Left;
	public PlayerAction Right;
	public PlayerAction Up;
	public PlayerAction Down;

    public PlayerAction Return;


	public PlayerActions() {		

		Left = CreatePlayerAction( "Left" );
		Right = CreatePlayerAction( "Right" );
		Up = CreatePlayerAction( "Up" );
		Down = CreatePlayerAction( "Down" );

        Return = CreatePlayerAction("Return");		
	}


	public static PlayerActions CreateWithDefaultBindings()
	{
		var playerActions = new PlayerActions();

		playerActions.Up.AddDefaultBinding( Key.UpArrow );
		playerActions.Down.AddDefaultBinding( Key.DownArrow );
		playerActions.Left.AddDefaultBinding( Key.LeftArrow );
		playerActions.Right.AddDefaultBinding( Key.RightArrow );

		playerActions.Left.AddDefaultBinding( InputControlType.LeftStickLeft );
		playerActions.Right.AddDefaultBinding( InputControlType.LeftStickRight );
		playerActions.Up.AddDefaultBinding( InputControlType.LeftStickUp );
		playerActions.Down.AddDefaultBinding( InputControlType.LeftStickDown );

		playerActions.Left.AddDefaultBinding( InputControlType.DPadLeft );
		playerActions.Right.AddDefaultBinding( InputControlType.DPadRight );
		playerActions.Up.AddDefaultBinding( InputControlType.DPadUp );
		playerActions.Down.AddDefaultBinding( InputControlType.DPadDown );

        playerActions.Return.AddDefaultBinding(Key.Return);
        playerActions.Return.AddDefaultBinding(Key.Space);

		playerActions.ListenOptions.IncludeUnknownControllers = true;
		playerActions.ListenOptions.MaxAllowedBindings = 3;
//			playerActions.ListenOptions.MaxAllowedBindingsPerType = 1;
//			playerActions.ListenOptions.UnsetDuplicateBindingsOnSet = true;

		playerActions.ListenOptions.OnBindingFound = ( action, binding ) =>
		{
			if (binding == new KeyBindingSource( Key.Escape ))
			{
				action.StopListeningForBinding();
				return false;
			}
			return true;
		};

		playerActions.ListenOptions.OnBindingAdded += ( action, binding ) =>
		{
			Debug.Log( "Binding added... " + binding.DeviceName + ": " + binding.Name );
		};

		playerActions.ListenOptions.OnBindingRejected += ( action, binding, reason ) =>
		{
			Debug.Log( "Binding rejected... " + reason );
		};

		return playerActions;
	}
}
