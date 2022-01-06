using System;
using UnityEngine;
using TMPro;

/// <summary>
/// Main Game Controller (Main Options and Configurations)
/// </summary>
public partial class GameManager : MonoBehaviour
{
    private Library _library;
    private LevelMaps _levelMaps;
    private Blocks _blocks;
    private Screens _screens;
    private Drops _drops;
    private ControllerSetup _controllerSetup;

    /**
     * Game Options
     */
    [Header("Main Options")]

    [SerializeField]
    [Range(0.0f, 5.0f)]
    private float _gameSpeed;

    private float _gameSpeedBackup; // Save the current gamespeed value to get back to it

    [SerializeField]
    private bool _autoPlay;

    [SerializeField]
    private bool _gamePaused;

    [SerializeField]
    public GameObject _dynamicObjectsParent;

    [NonSerialized]
    private ScreenType _activeScreen; // Screen type object define whenever you want to change screen

    /**
     * Game Scores
     */
    [Header("Game Score Setup")]
    [SerializeField]
    private TextMeshProUGUI _scoreText;

    private int _gameScore;

    /**
     * Blocks setup
     */
    [Header("Block Setup")]

    [SerializeField]
    public GameObject _blocksParent;

    [SerializeField]
    [Tooltip("Audio clip for block destroy")]
    public AudioClip _destroyBlockClip;

    private int _totalBlockCount;

    private int _currentBlockCount;

    /**
     * Background setup
     */
    [Header("Background Setup")]

    [SerializeField]
    [Tooltip("Background Prefab Object")]
    private GameObject _pfBackground;

    /**
     * Paddle setup
     */
    [Header("Paddle Setup")]

    [SerializeField]
    public Paddle _paddle;

    [SerializeField]
    [Range(1.0f, 32.0f)]
    [Tooltip("Screen width in unit for calculating mouse x position")]
    public float _screnWidthInUnit = 8f;

    [SerializeField]
    [Range(1.0f, 32.0f)]
    public float _paddleMinX = 1.16f;

    [SerializeField]
    [Range(1.0f, 32.0f)]
    public float _paddleMaxX = 7.834f;

    /**
     * Drops Setup
     */
    [Header("Drops Setup")]

    [SerializeField]
    public Vector2 _dropFallSpeed;

    [SerializeField]
    public float _dropMaxLifeInSeconds;

    [SerializeField]
    private bool _lazerActive;

    [SerializeField]
    public int _lazerAmmoCount;

    [SerializeField]
    private Lazer _pfLazer;

    [SerializeField]
    public Vector2 _lazerSpeed;

    [SerializeField]
    public float _lazerYPaddingWithPaddle;

    /**
     * Ball setup
     */
    [Header("Ball Setup")]

    [SerializeField]
    public Ball _ball;

    [SerializeField]
    [Tooltip("Add some randomness to Ball's bounce")]
    private float _ballRandomFactor;

    [SerializeField]
    [Tooltip("Ball's sound clips for colliders")]
    public AudioClip[] _ballSounds;

    private bool _ballLaunched = false;

    /**
     * Level setup
     */
    private bool _levelLoaded;

    /**
     * Get/Set
     */
    public Library library
    {
        get { return _library; }
    }

    public LevelMaps levelMaps
    {
        get { return _levelMaps; }
    }

    public Blocks blocks
    {
        get { return _blocks; }
    }

    public ControllerSetup controllerSetup
    {
        get { return _controllerSetup; }
    }

    public Drops drops
    {
        get { return _drops; }
    }

    public bool gamePaused
    {
        get { return _gamePaused; }
    }

    public bool autoPlay
    {
        get { return _autoPlay; }
        set { _autoPlay = value; }
    }

    public bool levelLoaded
    {
        get { return _levelLoaded; }
    }

    public int totalBlockCount
    {
        get { return _totalBlockCount; }
    }

    public int currentBlockCount
    {
        get { return _currentBlockCount; }
    }

    public bool lazerActive
    {
        get { return _lazerActive; }
        set { _lazerActive = value; }
    }

    public int lazerAmmoCount
    {
        get { return _lazerAmmoCount; }
        set { _lazerAmmoCount = value; }
    }

    public Lazer pfLazer
    {
        get { return _pfLazer; }
    }

    public float ballRandomFactor
    {
        get { return _ballRandomFactor; }
    }

    public bool ballLaunched
    {
        get { return _ballLaunched; }
        set { _ballLaunched = value; }
    }
}

/// <summary>
/// GameManager : Methods
/// </summary>
public partial class GameManager
{
    private void Start()
    {
        // Include Library
        _library = GetComponent<Library>();

        // Include LevelMaps
        _levelMaps = GetComponent<LevelMaps>();

        // Include LevelMaps
        _blocks = GetComponent<Blocks>();

        // Include Screens
        _screens = GetComponent<Screens>();

        // Include Screens
        _drops = GetComponent<Drops>();

        _controllerSetup = GetComponent<ControllerSetup>();

        // Set first screen as MainMenu
        MainMenuScreen();
    }

    private void Update()
    {
        ChangeTimeScale();

        if (Input.GetKeyDown(_controllerSetup._quitKey))
        {
            _library.QuitGame();
        }

        if (Input.GetKeyDown(_controllerSetup._pauseKey))
        {
            /**
             * ^ is the XOR cond operator, it is like a normal OR except 
             * if both are true it returns false instead of true, which means 
             * that you only get these two cases: 0^1=1 and 1^1=0 
             * which indeed inverts the bool ;)
             */
            _gamePaused ^= true;

            PauseUnpauseGame();
        }

        if (Input.GetKeyDown(_controllerSetup._autPlayKey))
        {
            autoPlay ^= true;
        }

        if (Input.GetKeyDown(_controllerSetup._endLevelKey))
        {
            _currentBlockCount = 0;
            EndLevelScreen();
        }

        if (Input.GetKeyDown(_controllerSetup._gameSpeedUpKey))
        {
            _gameSpeed += 0.5f;
        }

        if (Input.GetKeyDown(_controllerSetup._gameSpeedDownKey))
        {
            _gameSpeed -= 0.5f;
        }

        // Spawn lazer when boolean true
        if (_lazerActive && Input.GetKeyDown(_controllerSetup._lazerKey))
        {
            if (_lazerAmmoCount <= 0)
            {
                _lazerActive = false;
                return;
            }

            _lazerAmmoCount--;
            SpawnLazer();
        }

        if (Input.GetKeyDown(_controllerSetup._activateLazerKey))
        {
            _drops.GetDrop(DropType.Lazer).jobList.Invoke();
        }
    }

    /// <summary>
    /// Change screen with given type
    /// </summary>
    private void ChangeScreen()
    {
        int currentLevelNo; // Current level no cache variable

        // Activate the game objects for selected screen type from Screens
        foreach (Screens.ScreenData ScreenData in _screens._screenDataList)
        {
            if (ScreenData.screenType == _activeScreen)
            {
                foreach (GameObject GameObject in ScreenData.gameObjects)
                {
                    GameObject.SetActive(true);
                }
            }
            else
            {
                foreach (GameObject GameObject in ScreenData.gameObjects)
                {
                    GameObject.SetActive(false);
                }
            }
        }

        switch (_activeScreen)
        {
            case ScreenType.MainMenu:
                Cursor.visible = true;
                break;

            case ScreenType.InGame:
                Cursor.visible = false;
                ballLaunched = false;
                //ResetGame(); // Reset the game score saved in GameScore field
                _levelMaps.ReadLevels(); // Read all level SO

                // If no level assigned to LevelMaps then assign Level 1
                // Otherwise we will assign the next level at EndLevel screen
                if (!_levelMaps.level)
                {
                    _levelMaps.level = _levelMaps.levels[0];
                }

                // Remove "SO_" from the level name
                _levelMaps.level.levelName = _levelMaps.level.name.Split('_')[1];

                _levelMaps.ReadJsonFile();
                _levelMaps.SendDataToLevel();

                ResetBlockCount(); // Reset the level's block counts
                UpdateBackground(); // Update background with the data in level SO
                GenerateLevelMap(); // Generate level map from the leveldata in current level SO

                // Reset ball and paddle positions
                _ball.ResetPosition();
                _paddle.ResetPosition();

                _levelLoaded = true; // Set as game loaded and ready to play for loader feature purposes
                UpdateGameScoreUI(); // Update game score with the previous score
                break;

            case ScreenType.EndLevel:
                Cursor.visible = true;
                ballLaunched = false;
                _levelLoaded = false; // Set as game loaded false for loader feature purposes

                currentLevelNo = _levelMaps.GetLevelNo(_levelMaps.level);

                // Increase the level if its not last level then set
                if (currentLevelNo < _levelMaps.levels.Length)
                {
                    _levelMaps.level = _levelMaps.levels[currentLevelNo];
                }
                else
                {
                    GameCompletedScreen();
                }
                break;

            case ScreenType.GameCompleted:
                Cursor.visible = true;
                ballLaunched = false;
                _levelMaps.level = null; // Remove the level
                _levelLoaded = false;
                DestroyBlocks();
                break;

            case ScreenType.GameOver:
                Cursor.visible = true;
                ballLaunched = false;
                _levelMaps.level = null; // Remove the level
                _levelLoaded = false;
                DestroyBlocks();
                break;
        }
    }

    private void PauseUnpauseGame()
    {
        if (_gameSpeed > 0)
        {
            _gameSpeedBackup = _gameSpeed;
            _gameSpeed = 0;
        }
        else
        {
            _gameSpeed = _gameSpeedBackup;
        }
    }

    /// <summary>
    /// Create the blocks using prefabs with 
    /// location and block type data from level data
    /// </summary>
    [ContextMenu("GenerateLevelMap()")]
    public void GenerateLevelMap()
    {
        int pfBlockCount = _blocks.blockList.Length; // Count the Blocks

        // Set maximum Line and Column limits
        int maxLine = _levelMaps.level._levelData.Count;
        int maxCol = (int)_screnWidthInUnit;

        // Loop for lines
        for (int line = 0; line < maxLine; line++)
        {
#if UNITY_EDITOR
            _library = GetComponent<Library>();
#endif
            int[] intLine = _library.StringToIntArray(_levelMaps.level._levelData[line]);

            for (int col = 0; col < maxCol; col++) // Loop for columns : Create PF_Blocks
            {
                // Define and clamp the column maximum value to available block count
                int clampedColValue = intLine[col] > pfBlockCount ? 0 : intLine[col];

                // if column value is not 0 Instantiate PF_Block
                if (clampedColValue >= 1)
                {
                    // Create a Vector3 for new block position
                    float currentLinePos = (maxLine * 0.5f) - (line * 0.5f) + 0.25f;
                    Vector3 blockPos = new Vector3(col + 0.5f, currentLinePos, 0f);
                    // 0.5, 9.75, 0
                    // 0.5, 9.25, 0
                    //
                    // 0.5, 0.75, 0
                    // 0.5, 0.25, 0
                    // Create new block gameobject and set its name with trim and parent it
                    Block newBlock = Instantiate(_blocks.blockList[clampedColValue - 1].pfBlock, _blocksParent.transform.position + blockPos, transform.rotation, _blocksParent.transform);
                    newBlock.name = newBlock.name.Replace("(Clone)", "").Trim();
                }
            }
        }
    }

    /// <summary>
    /// Update prefab background with the data on level OS
    /// </summary>
    private void UpdateBackground()
    {
        SpriteRenderer _spriteRenderer = _pfBackground.GetComponent<SpriteRenderer>();

        _spriteRenderer.sprite = _levelMaps.level._background;
        _spriteRenderer.color = _levelMaps.level._tintColor;
    }

    /// <summary>
    /// Change the time scale of the game
    /// </summary>
    private void ChangeTimeScale()
    {
        Time.timeScale = _gameSpeed;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }

    public void AddToGameScore(Block inBlock)
    {
        Blocks.BlockData pfBlock = _blocks.GetBlock(inBlock); // Get the prefab block from Blocks array
        _gameScore += pfBlock.score; // Add found prefab's score to the game score
    }

    public void SpawnParticle(Block inBlock)
    {
        // Get the prefab block from Blocks array
        Blocks.BlockData pfBlock = _blocks.GetBlock(inBlock);

        // Instantiate the new particle prefab from pfBlock object and parent to the _DynamicOjects
        GameObject newPfParticle = Instantiate(pfBlock.destroyVFX, inBlock.transform.position,
            inBlock.transform.rotation, _dynamicObjectsParent.transform);

        Destroy(newPfParticle, 1.0f);
    }

    public void SpawnLazer()
    {
        Vector2 lazerStartPos = new Vector2(_paddle.transform.position.x, _paddle.transform.position.y + _lazerYPaddingWithPaddle);

        Lazer newLazer = Instantiate<Lazer>(pfLazer, lazerStartPos, pfLazer.transform.rotation,
            _dynamicObjectsParent.transform);
    }

    public void SpawnDrop(Block inBlock)
    {
        // Get the prefab block from Blocks array
        Blocks.BlockData pfBlock = _blocks.GetBlock(inBlock);

        // Return if its None
        if (pfBlock.dropType == DropType.None) return;

        // Get the prefab block from Blocks array
        Drops.DropData dropData = _drops.GetDrop(pfBlock.dropType);

        switch (dropData.dropType)
        {
            case DropType.Lazer:
                Vector2 dropStartPos = inBlock.transform.position;
                Quaternion dropStartRotation = new Quaternion();

                Drop newDropLazer = Instantiate<Drop>(dropData.pfDrop, dropStartPos, dropStartRotation, _dynamicObjectsParent.transform);

                newDropLazer.dropType = dropData.dropType;
                break;
        }
    }

    public void UpdateGameScoreUI()
    {
        _scoreText.text = _gameScore.ToString();
    }
    public void ResetGame()
    {
        _gameScore = 0;
        _lazerAmmoCount = 0;
        _lazerActive = false;
    }

    public void DestroyBlocks()
    {
        foreach (Transform child in _blocksParent.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void IncreaseBlockCount()
    {
        _currentBlockCount = ++_totalBlockCount;
    }

    public void DescreaseCurrentBlockCount()
    {
        _currentBlockCount--;
    }

    public void ResetBlockCount()
    {
        _currentBlockCount = _totalBlockCount = 0;
    }

    public void MainMenuScreen()
    {
        _activeScreen = ScreenType.MainMenu;
        ChangeScreen();
    }

    public void InGameScreen()
    {
        _activeScreen = ScreenType.InGame; // Set active screen to InGame
        ChangeScreen();
    }

    public void EndLevelScreen()
    {
        _activeScreen = ScreenType.EndLevel; // Set active screen to InGame
        ChangeScreen();
    }

    public void GameCompletedScreen()
    {
        _activeScreen = ScreenType.GameCompleted; // Set active screen to InGame
        ChangeScreen();
    }

    public void GameOverScreen()
    {
        _activeScreen = ScreenType.GameOver; // Set active screen to InGame
        ChangeScreen();
    }

    public void QuitGame()
    {
        _library.QuitGame();
    }
}
