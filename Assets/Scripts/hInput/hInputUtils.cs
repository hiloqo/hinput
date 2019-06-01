﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class gathers a couple of useful things that I didnt want to expose in the hInput class.
public static class hInputUtils {
	// --------------------
	// SETTINGS
	// --------------------

	//By how much to increase diagonals (in %), because otherwise the max stick distance is sometimes less than 1.
	//Does not affect raw inputs.
	public static float distanceIncrease { get { return 0.01f; } }

	//The maximum amount of gamepads supported by the game
	public static float maxGamepads { get { return 8; } }

	//By how much to increase deltaTime (in %) when comparing it, to account for rounding errors.
	public static float deltaTimeEpsilon { get { return 0.1f; } }


	// --------------------
	// BUTTONS AND AXES
	// --------------------

	public static bool GetButton (string fullName) { return GetButton (fullName, true); }
	public static bool GetButton (string fullName, bool logError) {
		try {
			return Input.GetButton (fullName);
		} catch {
			if (logError) hInputNotSetUpError ();
			return false;
		}
	}

	public static float GetAxis (string fullName) { return GetAxis (fullName, true); }
	public static float GetAxis (string fullName, bool logError) {
		try {
			return Input.GetAxisRaw (fullName);
		} catch {
			if (logError) hInputNotSetUpError ();
			return 0;
		}
	}

	private static void hInputNotSetUpError () {
		Debug.LogWarning("Warning : hInput has not been set up, so gamepad inputs cannot be recorded."+
		"To set it up, go to the hInput menu and click \"Set Up hInput\".");
	}


	// --------------------
	// TIME
	// --------------------

	//The time it was last time the game was updated
	public static float lastUpdated;

	//The duration it took to process the previous frame
	private static float deltaTime;

	public static void UpdateTime () {
		float time = Time.time;
		deltaTime = time - lastUpdated;
		lastUpdated = time;
	}

	//The previous frame was processed in less than this duration.
	public static float maxDeltaTime { get { return (deltaTime)*(1 + deltaTimeEpsilon); } }

	public static bool isUpToDate { get { return lastUpdated == Time.time; } }


	// --------------------
	// OPERATING SYSTEM
	// --------------------

	//The user's operating system. Assigned when first called.
	private static string _os;
	public static string os { 
		get { 
			if (_os == null) {
				#if UNITY_EDITOR_WIN
					_os = "Windows";
				#elif UNITY_STANDALONE_WIN
					_os = "Windows";
				#elif UNITY_EDITOR_OSX
					_os = "Mac";
				#elif UNITY_STANDALONE_OSX
					_os = "Mac";
				#elif UNITY_EDITOR_LINUX
					_os = "Linux";
				#elif UNITY_STANDALONE_LINUX
					_os = "Linux";
				#else
					Debug.LogError("hInput Error : Unknown OS !");
				#endif
			}

			return _os;
		} 
	}
}