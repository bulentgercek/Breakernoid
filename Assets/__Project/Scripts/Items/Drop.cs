using UnityEngine;

public class Drop : MonoBehaviour
{
    private GameManager _gameManager;
    private Drops _drops;
    private GameObject _dynamicObjectsParent;
    private Rigidbody2D _rigidBody2D;
    private DropType _dropType;

    public DropType dropType
    {
        get { return _dropType; }
        set { _dropType = value; }
    }

    void Start()
    {
        _gameManager = GameObject.FindObjectOfType<GameManager>();
        _drops = _gameManager.drops;
        _dynamicObjectsParent = _gameManager._dynamicObjectsParent;
        _rigidBody2D = GetComponent<Rigidbody2D>();
        _rigidBody2D.velocity = _gameManager._dropFallSpeed;

        // Destroy drop after given seconds
        Destroy(gameObject, _gameManager._dropMaxLifeInSeconds);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Invoke all the events in job list on Drops
        _drops.GetDrop(_dropType).jobList.Invoke();
        Destroy(gameObject);
    }
}
