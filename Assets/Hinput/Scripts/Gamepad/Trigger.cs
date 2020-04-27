﻿using HinputClasses.Internal;

namespace HinputClasses {
    /// <summary>
    /// Hinput class representing the left or right trigger of a controller.
    /// </summary>
    public class Trigger : Pressable {
    	// --------------------
    	// ID
    	// --------------------
    
    	/// <summary>
    	/// Returns the real index of a trigger on its gamepad.
    	/// </summary>
    	/// <remarks>
    	/// If this trigger is anyInput, return -1.
    	/// </remarks>
    	public readonly int internalIndex;
    	
    	/// <summary>
    	/// Returns the index of a trigger on its gamepad.
    	/// </summary>
    	/// <remarks>
    	/// If this trigger is anyInput, return the index of the input that is currently being pressed.
    	/// </remarks>
    	public int index { get { return internalIndex; } }
    	
    	
    	// --------------------
    	// CONSTRUCTOR
    	// --------------------
    
    	public Trigger (string name, Gamepad internalGamepad, int index) : 
    		base(name, internalGamepad, internalGamepad.internalFullName + "_" + name) {
    		this.internalIndex = index;
    		initialValue = measuredPosition;
    	}
    
    
    	// --------------------
    	// INITIAL VALUE
    	// --------------------
    	
    	private readonly float initialValue;
    	private bool hasBeenMoved;
    
    	// The value of the trigger's position, given by the gamepad driver.
    	// In some instances, until an input is recorded triggers will have a non-zero measured resting position.
    	private float measuredPosition { 
    		get {
    			if (Utils.os == "Mac") return (Utils.GetAxis(internalFullName) + 1)/2;
    			return Utils.GetAxis(internalFullName);	
    		}
    	}
    
    	
    	// --------------------
    	// UPDATE
    	// --------------------
    
    	// If no input have been recorded before, make sure the resting position is zero
    	// Else just return the measured position.
    	protected override void UpdatePositionRaw() {
    		float measuredPos = measuredPosition;
    
    		if (hasBeenMoved) {
    			positionRaw = measuredPos;
    		} else if (measuredPos.IsNotEqualTo(initialValue)) {
    			hasBeenMoved = true;
    			positionRaw = measuredPos;
    		}
    		else positionRaw = 0f;
    	}
    
    
    	// --------------------
    	// PROPERTIES
    	// --------------------
    
    	/// <summary>
    	/// Returns the position of the trigger, between 0 and 1.
    	/// </summary>
    	public override float position { 
    		get { 
    			float posRaw = positionRaw;
    
    			if (posRaw < Settings.triggerDeadZone) return 0f;
    			else return ((posRaw - Settings.triggerDeadZone)/(1 - Settings.triggerDeadZone));
    		} 
    	}
    
    	/// <summary>
    	/// Returns true if the position of the trigger is beyond the limit of its pressed zone. Returns false otherwise.
    	/// </summary>
    	/// <remarks>
    	/// The size of the pressed zone of the triggers can be changed with the triggerPressedZone property of Settings.
    	/// </remarks>
    	public override bool pressed { get { return position >= Settings.triggerPressedZone; } }
    
    	/// <summary>
    	/// Returns true if if the position of the trigger is within the limits of its dead zone. Returns false otherwise.
    	/// </summary>
    	/// <remarks>
    	/// The size of the dead zone of the triggers can be changed with the triggerDeadZone property of Settings.
    	/// </remarks>
    	public override bool inDeadZone { get { return position < Settings.triggerDeadZone; } }
    }
}