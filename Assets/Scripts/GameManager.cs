using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Singleton class for managing scene transitions.
/// </summary>
public class GameManager : Singleton<GameManager>
{
    [SerializeField] private TextMeshProUGUI levelText;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void LoadLevelScene()
    { 
        StartCoroutine(LoadLevelSceneAsync("LevelScene"));
    }

    IEnumerator LoadLevelSceneAsync(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        var levelTextObj = GameObject.Find("LevelText");
        if (levelTextObj != null) levelText = levelTextObj.GetComponent<TextMeshProUGUI>();
        if (levelText != null) levelText.text = "Loading...";

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        yield return new WaitForSeconds(0.25f);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
    }

    public void NextLevel()
    {
        int currentLevel = PlayerPrefs.GetInt("Level", 1);
        PlayerPrefs.SetInt("Level", currentLevel + 1);
        LoadMainMenu();
    }
}
