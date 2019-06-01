﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hAxis {
	// --------------------
	// NAME
	// --------------------

	private string fullAxisName;
	private string fullPositiveButtonName;
	private string fullNegativeButtonName;


	// --------------------
	// CONSTRUCTORS
	// --------------------

	// D-pad constructor
	public hAxis (string fullAxisName, string fullPositiveButtonName, string fullNegativeButtonName) {
		this.fullAxisName = fullAxisName;
		this.fullPositiveButtonName = fullPositiveButtonName;
		this.fullNegativeButtonName = fullNegativeButtonName;
	}

	// left/right stick constructor
	public hAxis (string fullAxisName) {
		this.fullAxisName = fullAxisName;
		this.fullPositiveButtonName = "";
		this.fullNegativeButtonName = "";
	}

	
	// --------------------
	// PROPERTIES
	// --------------------

	// The D-pad will be recorded as two axes or four buttons, depending on the gamepad driver used.
	// Measure both the axes and the buttons, and ignore the one that returns an error.
	private float _positionRaw;
	public float positionRaw { 
		get {
			float axisValue = hInputUtils.GetAxis(fullAxisName, false);

			float buttonValue = 0f;
			if (fullPositiveButtonName != "" && fullNegativeButtonName != "") {
				if (hInputUtils.GetButton(fullPositiveButtonName, false)) buttonValue = 1;
				if (hInputUtils.GetButton(fullNegativeButtonName, false)) buttonValue = -1;
			}

			return (axisValue + buttonValue);
		} 
	}
}