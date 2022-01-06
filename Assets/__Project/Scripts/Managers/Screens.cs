using System;
using UnityEngine;

/// <summary>
/// Screen types for different game screens
/// </summary>
public enum ScreenType
{
    MainMenu,
    InGame,
    EndLevel,
    GameCompleted,
    GameOver
}

/// <summary>
/// Fill this class with selection arrays that
/// point to which objects will be enabled 
/// when certain screen types activated in the GameManager
/// </summary>
public class Screens : MonoBehaviour
{
    [Serializable]
    public struct ScreenData
    {
        public ScreenType screenType;
        public GameObject[] gameObjects;
    }

    [SerializeField]
    public ScreenData[] _screenDataList;
}
