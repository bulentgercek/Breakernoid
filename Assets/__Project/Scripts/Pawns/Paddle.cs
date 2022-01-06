using UnityEngine;

public class Paddle : MonoBehaviour
{
    private GameManager _gameManager;
    private Vector2 _startPosition;

    private void Awake()
    {
        // Cache paddle start position
        // BUG : When game resolution changes this value need to be resetted too!
        _startPosition = transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameObject.FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // Stop if level not loaded or game paused
        if (!_gameManager.levelLoaded || _gameManager.gamePaused) return;

        // Create a Vector2 variable to store the paddle's position per frame
        Vector2 paddlePosition = new Vector2(transform.position.x, transform.position.y);

        // Clamp and set the stored position of the paddle in x per frame
        paddlePosition.x = Mathf.Clamp(GetXPos(), _gameManager._paddleMinX, _gameManager._paddleMaxX);

        // Set the current position of the paddle with calculated new Vector2 per frame
        transform.position = paddlePosition;
    }
    public void ResetPosition()
    {
        transform.position = _startPosition;
    }

    private float GetXPos()
    {
        if (_gameManager.autoPlay)
        {
            return _gameManager._ball.transform.position.x;
        }
        else
        {
            // Return a float variable to store mouse x position in units with
            // calculation of screen width per frame
            return Input.mousePosition.x / Screen.width * _gameManager._screnWidthInUnit;
        }
    }
}
