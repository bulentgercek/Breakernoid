using UnityEngine;

public class Lazer : MonoBehaviour
{
    GameManager _gameManager;
    Rigidbody2D _rigidBody2D;

    private void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _rigidBody2D = GetComponent<Rigidbody2D>();
        _rigidBody2D.velocity = _gameManager._lazerSpeed;

        // Destroy lazer after given seconds
        Destroy(gameObject, _gameManager._dropMaxLifeInSeconds);

        /* Helpful Comment : Ignore collision with the Ball (without Layer Collision Matrix)
        Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), _gameManager._ball.GetComponent<Collider2D>());*/
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        /* Helpful Comment : If it's an instance of a prefab check its name and don't destroy it
        string colliderName = collision.collider.gameObject.name.Replace("(Clone)", "").Trim();
        if (colliderName != _gameManager.pfLazer.name) Destroy(gameObject);*/

        Destroy(gameObject);
    }
}
