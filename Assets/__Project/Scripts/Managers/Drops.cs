using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Drop types for different type of Drops
/// </summary>
public enum DropType
{
    None,
    Lazer
}

public class Drops : MonoBehaviour
{
    [SerializeField]
    public DropData[] _dropList;

    [System.Serializable]
    public struct DropData
    {
        public DropType dropType;
        public Drop pfDrop;
        
        [System.Serializable]
        public class JobList : UnityEvent { }

        public JobList jobList;
    }

    public DropData[] dropList
    {
        get { return _dropList; }
    }

    /// <summary>
    /// Get the DropType dropdata from DropList
    /// </summary>
    /// <param name="inDropType"></param>
    /// <returns>Blocks</returns>
    public DropData GetDrop(DropType inDropType)
    {
        DropData foundDropData = new DropData();

        foreach (var dropData in _dropList)
        {
            if (dropData.dropType == inDropType)
            {
                foundDropData = dropData;
                break;
            }
        }

        return foundDropData;
    }
}
