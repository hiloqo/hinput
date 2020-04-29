﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HinputClasses {
    /// <summary>
    /// Hinput class representing a given stick, such as the left stick, the right stick or the D-Pad, on every gamepad at once.
    /// </summary>
    public class AnyGamepadStick : Stick {
        // --------------------
        // ID
        // --------------------

        public override Gamepad gamepad {
            get { return ((AnyGamepad) internalGamepad).gamepad; }
        }


        // --------------------
        // CONSTRUCTOR
        // --------------------

        public AnyGamepadStick(string name, Gamepad internalGamepad, int index)
            : base(name, internalGamepad, index, true) {
            _pressedSticks = Hinput.gamepad.Select(g => g.sticks[index]).ToList();
        }


        // --------------------
        // PRESSED STICKS
        // --------------------

        //List of all sticks of this type
        private List<Stick> _allSticks;

        private List<Stick> allSticks {
            get {
                if (_allSticks != null) return _allSticks;

                _allSticks = Hinput.gamepad.Select(g => g.sticks[index]).ToList();
                return _allSticks;
            }
        }

        private List<Stick> _pressedSticks;
        private int _lastPressedSticksUpdateFrame = -1;

        /// <summary>
        /// Returns a list of every stick of this type that is currently outside of its dead zone.
        /// </summary>
        /// <remarks>
        /// If no gamepad has a stick of this type outside of its dead zone, returns every stick of this type.
        /// </remarks>
        public List<Stick> pressedSticks {
            get {
                if (_lastPressedSticksUpdateFrame == Time.frameCount) return _pressedSticks;

                _pressedSticks = allSticks.Where(s => !s.inDeadZone).ToList();
                _lastPressedSticksUpdateFrame = Time.frameCount;
                return _pressedSticks;
            }
        }

        /// <summary>
        /// Returns the stick of this type that is currently being pressed.
        /// </summary>
        /// <remarks>
        /// If no stick of this type is pressed, returns null.
        /// If several sticks of this type are pressed, returns the pressed stick from the gamepad with the smallest index.
        /// </remarks>
        public Stick pressedStick {
            get {
                if (pressedSticks.Count == 0) return null;
                else return pressedSticks[0];
            }
        }


        // --------------------
        // PUBLIC PROPERTIES - RAW
        // --------------------

        public override float horizontalRaw {
            get {
                if (pressedSticks.Count == 0) return allSticks.Select(stick => stick.horizontalRaw).Average();
                else return pressedSticks.Select(stick => stick.horizontalRaw).Average();
            }
        }

        public override float verticalRaw {
            get {
                if (pressedSticks.Count == 0) return allSticks.Select(stick => stick.verticalRaw).Average();
                else return pressedSticks.Select(stick => stick.verticalRaw).Average();
            }
        }
    }
}