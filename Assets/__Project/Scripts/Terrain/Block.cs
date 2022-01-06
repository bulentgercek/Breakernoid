using UnityEngine;

public class Block : MonoBehaviour
{
    private GameManager _gameManager;
    private Library _library;
    private Blocks _blocks;
    private GameObject _dynamicObjectsParent;

    private int _blockCurrentHealth = 1;

    void Start()
    {
        _gameManager = GameObject.FindObjectOfType<GameManager>();
        _library = _gameManager.library;
        _blocks = _gameManager.blocks;
        _dynamicObjectsParent = _gameManager._dynamicObjectsParent;
        IncreaseBreakableBlockCount();
        GetBlockHealth();
    }

    private void GetBlockHealth()
    {
        Blocks.BlockData currentBlock = _blocks.GetBlock(this);

        if (currentBlock.isBreakable)
        {
            _blockCurrentHealth = currentBlock.blockHealth;
            gameObject.GetComponent<SpriteRenderer>().sprite = _blocks._blockBreakSprites[_blockCurrentHealth-1];
        }
    }

    /// <summary>
    /// If its breakable then add new block to the level's total block count
    /// </summary>
    private void IncreaseBreakableBlockCount()
    {
        if (_blocks.GetBlock(this).isBreakable)
        {
            _gameManager.IncreaseBlockCount();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        BlockDamageHandler();
    }

    private void BlockDamageHandler()
    {
        if (_blocks.GetBlock(this).isBreakable)
        {
            if (_blockCurrentHealth > 1)
            {
                _blockCurrentHealth--;
                gameObject.GetComponent<SpriteRenderer>().sprite = _blocks._blockBreakSprites[_blockCurrentHealth-1];
            }
            else
            {
                DestroyBlock();
            }
        }
    }

    // Play Destroy Block Clip and Destroy Block
    private void DestroyBlock()
    {
        // Notes : Normally you can use a line already exist in Unity Api like this
        // to create and destroy and audio :
        // AudioSource.PlayClipAtPoint(_destroyClip, Camera.main.transform.position);
        // Instead, I created this custom method below in Library class to add
        // the newly created object to a parent object
        // and it can also be null because it's optional;)
        _library.PlayClipAtPoint(_gameManager._destroyBlockClip, Camera.main.transform.position, _dynamicObjectsParent);
        
        _gameManager.SpawnParticle(this); // Spawn particle effect on its location
        _gameManager.SpawnDrop(this); // Spawn drop on its location if defined it Blocks
        _gameManager.AddToGameScore(this); // Add destroyed block score value to game score
        _gameManager.UpdateGameScoreUI(); // Update game score on UI
        _gameManager.DescreaseCurrentBlockCount(); // Decrease the block count
        Destroy(gameObject);

        // Check if all blocks destroyed
        if (_gameManager.currentBlockCount <= 0)
        {
            _gameManager.ballLaunched = false;
            _gameManager.EndLevelScreen();
        }
    }
}
