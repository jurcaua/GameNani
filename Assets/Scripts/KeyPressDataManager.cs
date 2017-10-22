using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public static class KeyPressDataManager {

    public static Dictionary<KeyCode, Keydata> dictionary = new Dictionary<KeyCode, Keydata>();
    public static KeyCode[] checkedKeys = { KeyCode.A, KeyCode.B, KeyCode.C, KeyCode.D, KeyCode.E, KeyCode.F, KeyCode.G, KeyCode.H, KeyCode.I, KeyCode.J,
                                           KeyCode.K, KeyCode.L, KeyCode.M, KeyCode.N, KeyCode.O, KeyCode.P, KeyCode.Q, KeyCode.R, KeyCode.S, KeyCode.T,
                                           KeyCode.U, KeyCode.V, KeyCode.W, KeyCode.X, KeyCode.Y, KeyCode.Z,
                                           KeyCode.Alpha0, KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4,
                                           KeyCode.Alpha5, KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9,
                                           KeyCode.Space, KeyCode.Escape, KeyCode.LeftShift, KeyCode.RightShift, KeyCode.LeftControl,
                                           KeyCode.Tab, KeyCode.CapsLock, KeyCode.LeftAlt, KeyCode.LeftWindows };

    public static int CountOf(KeyCode key) {
        return dictionary[key].count;
    }

    public static float LongestHoldTimeOf(KeyCode key) {
        return dictionary[key].longestHold;
    }

    // general keycode register function 
    public static void Register(KeyCode keycode, int presses = 1) {

        if (!dictionary.ContainsKey(keycode)) {
            dictionary.Add(keycode, new Keydata(keycode, presses));
        } else {
            dictionary[keycode].Inc();
        }
    }

    // general keycode register function, focus on initial key press event
    public static void RegisterDown(KeyCode keycode, int presses = 1) {

        Register(keycode, presses);
        dictionary[keycode].setStartTime(Time.time);
    }

    // general keycode register function, focus on key release event
    public static void RegisterUp(KeyCode keycode, int presses = 1) {
        
        dictionary[keycode].setEndTime(Time.time);
    }
}

public class Keydata {

    // the regular data
    public KeyCode keycode;
    public int count;

    // longest hold data
    public float longestHold;
    float[] holdTime = new float[2];
    bool held = false;

    public Keydata(KeyCode _keycode, int _count = 0) {
        keycode = _keycode;
        count = _count;
        longestHold = 0f;
    }

    public void Inc(int by = 1) {
        count += by;
    }

    public void Dec(int by = 1) {
        count -= by;
    }

    public void setStartTime (float startTime) {
        Debug.Log("pressed... " + keycode);
        holdTime[0] = startTime;
        held = true;
    }

    public void setEndTime(float endTime) {
        Debug.Log("let go of... " + keycode);
        holdTime[1] = endTime;
        if (held) {
            held = false;

            float newHoldTime = holdTime[1] - holdTime[0];
            Debug.Log(newHoldTime + " vs. " + longestHold);
            if (newHoldTime > longestHold) {
                longestHold = newHoldTime;
            }
        }
    }
}