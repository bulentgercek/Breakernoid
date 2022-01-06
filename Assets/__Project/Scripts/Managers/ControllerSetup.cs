using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerSetup : MonoBehaviour
{
    /**
     * Keyboard Setup
     */
    [Header("Keyboard Setup")]
    [SerializeField]
    public KeyCode _launchKey = KeyCode.Space;
    public KeyCode _launchWithMouseBut = KeyCode.Mouse0;
    public KeyCode _lazerKey = KeyCode.Space;
    public KeyCode _activateLazerKey = KeyCode.L;
    public KeyCode _pauseKey = KeyCode.P;
    public KeyCode _quitKey = KeyCode.Escape;
    public KeyCode _autPlayKey = KeyCode.C;
    public KeyCode _gameSpeedUpKey = KeyCode.W;
    public KeyCode _gameSpeedDownKey = KeyCode.S;
    public KeyCode _endLevelKey = KeyCode.N;
}
