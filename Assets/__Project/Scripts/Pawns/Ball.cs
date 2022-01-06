using UnityEngine;

public class Ball : MonoBehaviour
{
    private GameManager _gameManager;
    private Paddle _paddle1;
    private Vector2 _startPosition;
    private Vector2 _paddleToBallVector;
    private AudioSource _ballAudioSource;
    private Rigidbody2D _ballRigidbody2D;
    
    private void Awake()
    {
        // Cache paddle start position
        _startPosition = transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Get level scriptable object from LevelManager script by accesing game object tagged as GameController
        _gameManager = GameObject.FindObjectOfType<GameManager>();

        // Get Paddle object that has the Paddle script
        _paddle1 = _gameManager._paddle;

        // Store the difference between the paddle's position and the ball position
        _paddleToBallVector = transform.position - _paddle1.transform.position;
        
        _ballAudioSource = GetComponent<AudioSource>();
        _ballRigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Stop if level not loaded or game paused
        if (!_gameManager.levelLoaded || _gameManager.gamePaused) return;

        // Ball launched?
        if (_gameManager.ballLaunched) return;

        // Ball launched at start option selected in level SO?
        if (!_gameManager.ballLaunched)
        {
            LockToPaddle();
            LaunchOnMouseClick();
        }
        else
        {
            LockToPaddle();
            Launch();
        }
    }

    public void ResetPosition()
    {
        transform.position = _startPosition;
    }

    private void LaunchOnMouseClick()
    {
        if (Input.GetKeyDown(_gameManager.controllerSetup._launchWithMouseBut) || Input.GetKeyDown(_gameManager.controllerSetup._launchKey))
        {
            Launch();
        }
    }

    private void Launch()
    {
        _gameManager.ballLaunched = true;

        if (!_gameManager.levelMaps.level) return;

        // Give velocity to the ball
        _ballRigidbody2D.velocity = new Vector2(_gameManager.levelMaps.level.ballXPush, _gameManager.levelMaps.level.ballYPush);
    }

    private void LockToPaddle()
    {
        // Create a Vector2 to store the paddle's current position per frame
        Vector2 paddlePos = new Vector2(_paddle1.transform.position.x, _paddle1.transform.position.y);

        // Set the ball's position as paddle's current position + position difference between the ball and the paddle
        transform.position = paddlePos + _paddleToBallVector;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_gameManager.levelLoaded || _gameManager.ballLaunched)
        {
            PlayRandomSound();
            TweakBallVelocity();
        }
    }
     
    /// <summary>
    /// Adding some randomness to ball's bounce
    /// to get rid of that ball stuck on between colliders
    /// </summary>
    private void TweakBallVelocity()
    {
        float ballRandomFactor = _gameManager.ballRandomFactor;
        Vector2 velocityTweak = new Vector2
        (
            Random.Range(0, ballRandomFactor),
            Random.Range(0, ballRandomFactor)
        );

        _ballRigidbody2D.velocity += velocityTweak;
    }

    private void PlayRandomSound()
    {
        AudioClip clip = _gameManager._ballSounds[UnityEngine.Random.Range(0, _gameManager._ballSounds.Length)];
        _ballAudioSource.PlayOneShot(clip);
    }
}
