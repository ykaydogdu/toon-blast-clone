using UnityEngine;
using System.IO;

/// <summary>
/// Manages the game levels.
/// </summary>
public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] private GameBoard gameBoard;

    private LevelData levelData;
    private Level level;

    public int CurrentLevel;

    public void Initialize()
    {
        int currentLevel = PlayerPrefs.GetInt("Level", 1);
        levelData = LoadLevelData(currentLevel);
        CurrentLevel = levelData.level_number;
        level = new(levelData);
    }

    public Level GetLevel()
    {
        return level;
    }

    public LevelData LoadLevelData(int levelNumber)
    {
        string levelIndex = levelNumber.ToString("D2");
        string filePath = Path.Combine(Application.dataPath, "Levels", "level_" + levelIndex + ".json");
        if (File.Exists(filePath))
        {
            string jsonStr = File.ReadAllText(filePath);
            LevelData levelData = JsonUtility.FromJson<LevelData>(jsonStr);
            return levelData;
        }
        else
        {
            Debug.LogError("Level file not found: " + filePath);
            return null;
        }
    }
}