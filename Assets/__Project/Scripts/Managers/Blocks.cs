using UnityEngine;

public class Blocks : MonoBehaviour
{
    //public Drops _drops;
    /**
     * Blocks
     */
    [SerializeField]
    public Sprite[] _blockBreakSprites;

    [SerializeField]
    private BlockData[] _blockList;

    [System.Serializable]
    public struct BlockData
    {
        public Block pfBlock;
        public int score;
        public GameObject destroyVFX;
        public bool isBreakable;
        [Range(1, 3)]
        public int blockHealth;
        public DropType dropType;
    }

    public BlockData[] blockList
    {
        get { return _blockList; }
    }

    /// <summary>
    /// Get the prefab block from Blocks array
    /// </summary>
    /// <param name="inObject"></param>
    /// <returns>Blocks</returns>
    public BlockData GetBlock(Block inBlock)
    {
        BlockData foundBlockData = new BlockData();

        foreach (var blockData in _blockList)
        {
            if (blockData.pfBlock.name == inBlock.name)
            {
                foundBlockData = blockData;
                break;
            }
        }

        return foundBlockData;
    }
}
