using UnityEngine;

public class LoseCollider : MonoBehaviour
{
    private GameManager _gameManager;

    private void Start()
    {
        _gameManager = GameObject.FindObjectOfType<GameManager>();
    }

    // Monobehaviour's virtual function
    void OnTriggerEnter2D(Collider2D collision)
    {
        _gameManager.ResetBlockCount();
        _gameManager.GameOverScreen();
    }
}
