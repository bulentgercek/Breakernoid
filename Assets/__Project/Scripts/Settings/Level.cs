using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_NewLevel", menuName = "Breakernoid SO/Level")]
public class Level : ScriptableObject
{
    /**
     * Level variables
     */
    [Header("Level Configuration")]
    [SerializeField]
    private string _levelName;

    [SerializeField]
    public List<string> _levelData = new List<string>();

    [SerializeField]
    [Range(1.0f, 32.0f)]
    [Tooltip("Ball's push value on X direction when it's launched")]
    private float _ballXPush = 2f;

    [SerializeField]
    [Range(1.0f, 32.0f)]
    [Tooltip("Ball's push value on Y direction when it's launched")]
    private float _ballYPush = 15f;

    /**
    * Level background
    */
    [Header("Background")]
    public Sprite _background;
    public Color _tintColor;

    /**
    * Get/Set
    */
    public string levelName
    {
        get { return _levelName; }
        set { _levelName = value; }
    }

    public float ballXPush
    {
        get { return _ballXPush; }
    }

    public float ballYPush
    {
        get { return _ballYPush; }
    }
}
