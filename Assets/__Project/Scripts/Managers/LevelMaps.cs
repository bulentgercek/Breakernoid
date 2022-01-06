using System.Collections.Generic;
using UnityEngine;

public class LevelMaps : MonoBehaviour
{
    private GameManager _gameManager;

    [SerializeField]
    private TextAsset _jsonFile;

    [SerializeField]
    [Tooltip("Current Level Scriptable Object")]
    private Level _level;

    [System.Serializable]
    private struct LevelData
    {
        public string levelName;
        public string l19, l18, l17, l16, l15, l14, l13, l12, l11, l10, l09, l08, l07, l06, l05, l04, l03, l02, l01;
    }

    [System.Serializable]
    private class LevelList
    {
        public LevelData[] levels;
    }

    [Tooltip("Level Map Data From Json")]
    private LevelList _levelList = new LevelList();

    private Level[] _levels; // All level SO

    /**
     * Get/Set
     */
    public Level level
    {
        get { return _level; }
        set { _level = value; }
    }

    public Level[] levels
    {
        get { return _levels; }
    }

    private void Start()
    {
        _gameManager = GetComponent<GameManager>();
    }

    /// <summary>
    /// Read all level SO
    /// </summary>
    /// <returns>Level[]</returns>
    public void ReadLevels()
    {
        _levels = Resources.LoadAll<Level>("Levels");
    }

    /// <summary>
    /// Parse and get level no
    /// </summary>
    /// <param name="level"></param>
    /// <returns>int</returns>
    public int GetLevelNo(Level level)
    {
        return int.Parse(level.levelName.Split('l')[1]);
    }

    [ContextMenu("ReadJsonFile()")]
    public void ReadJsonFile()
    {
        if (!_jsonFile) return;

        _levelList = JsonUtility.FromJson<LevelList>(_jsonFile.text);
    }

    /// <summary>
    /// Convert class field values to a List
    /// </summary>
    private List<string> GetLevelDataList(LevelData data)
    {
        // Create new list
        List<string> newData = new List<string>();

        // Get class fields to Reflection.FieldInfo array
        System.Reflection.FieldInfo[] array = data.GetType().GetFields();

        // With using GetValue add the values* of the class fields to the list
        // *Except _levelName field
        foreach (var arrayData in array)
        {
            if (arrayData.Name != "levelName")
            {
                newData.Add(arrayData.GetValue(data).ToString());
            }
        }

        return newData;
    }

    /// <summary>
    /// Set all converted LevelData field list values to the selected level's SO
    /// </summary>
    [ContextMenu("SendDataToLevel()")]
    public void SendDataToLevel()
    {
        if (_levelList.levels == null)
        {
            Debug.LogError("No Json file on LevelMaps script. Add and  try again.");
            _gameManager.QuitGame();
            return;
        }
        foreach (LevelData leveldata in _levelList.levels)
        {
            if (leveldata.levelName == _level.levelName)
            {
                _level._levelData = GetLevelDataList(leveldata);
            }
        }
    }
}
