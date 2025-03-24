using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button levelButton;
    [SerializeField] private TextMeshProUGUI levelText;
    public int MaxLevel = 10;

    private void Awake()
    {
        int level = PlayerPrefs.GetInt("Level", 1); // starting level 1
        if (level > MaxLevel)
        {
            levelText.text = "Finished";
            levelButton.CancelInvoke();
        }
        else
        {
            levelText.text = "Level " + level;
            levelButton.onClick.AddListener(() =>
            {
                levelButton.transform.DOKill();
                GameManager.Instance.LoadLevelScene();
            });
        }
    }
}
