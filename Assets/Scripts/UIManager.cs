using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private GameObject levelCompletePanel;
    [SerializeField] private GameObject levelFailedPanel;
    [SerializeField] private TextMeshProUGUI levelCompleteNumberText;
    [SerializeField] private TextMeshProUGUI levelFailedNumberText;
    [SerializeField] private Button exitButton;
    [SerializeField] private Button retryButton;
    [SerializeField] private Image celebrationImage;
    [SerializeField] private GameObject celebrationParticlePrefab;

    private float timer = 0f;
    private float spawnTimer = 0f;
    private float screenBottom;
    private float screenWidth;

    void Start()
    {
        // bottom of the screen in world coordinates
        screenBottom = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).y;
        screenWidth = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x;
    }

    public IEnumerator SpawnComets()
    {
        float spawnDuration = 5f;
        float spawnRate = 2f;
        timer = 0f;
        while (timer < spawnDuration)
        { 
            spawnTimer += Time.deltaTime;
            if (spawnTimer >= 1f / spawnRate)
            {
                SpawnComet();
                spawnTimer = 0f;
                spawnRate = Random.Range(1f, 3f);
            }

            timer += Time.deltaTime;
            yield return null;
        }
    }

    void SpawnComet()
    {
        if (celebrationParticlePrefab == null)
        {
            Debug.LogError("Comet Particle Prefab is not assigned!");
            return;
        }

        // random X position at the bottom of the screen
        float randomX = Random.Range(-screenWidth / 2, screenWidth / 2);
        Vector3 spawnPosition = new Vector3(randomX, screenBottom, 0);

        GameObject comet = Instantiate(celebrationParticlePrefab, spawnPosition, Quaternion.identity);

        // direction of comet
        Vector3 direction = new Vector3(Random.Range(-0.5f, 0.5f), 1f, 0).normalized;

        // make the rotation of the particle system correct
        var particleSystem = comet.GetComponentInChildren<ParticleSystem>();
        Quaternion q = Quaternion.LookRotation(direction, Vector3.forward);
        particleSystem.transform.rotation = q * Quaternion.Euler(Vector3.right * 180);

        comet.GetComponent<Transform>().DOMove(spawnPosition + direction * 30f, 2f).SetEase(Ease.Linear).OnComplete(() =>
        {
            Destroy(comet);
        });
    }

    private void OnEnable()
    {
        MoveManager.Instance.OnMovesFinished += MovesFinished;
    }

    private void OnDisable()
    {
        if (MoveManager.Instance != null)
        {
            MoveManager.Instance.OnMovesFinished -= MovesFinished;
        }
    }

    private void MovesFinished()
    {
        if (GoalManager.Instance.CheckAllGoalsCompleted())
        {
            ShowLevelComplete();
        }
        else
        {
            ShowLevelFailed();
        }
    }

    public void ShowLevelComplete()
    {
        StartCoroutine(SpawnComets());
        Invoke("ShowLevelCompletePanel", 2f);
    }

    void ShowLevelCompletePanel()
    {
        levelCompleteNumberText.text = $"Level {LevelManager.Instance.CurrentLevel} Complete!";
        levelCompletePanel.SetActive(true);
        
        celebrationImage.transform.DOScale(Vector3.one, 2f).SetEase(Ease.OutBounce);
        celebrationImage.transform.DORotate(Vector3.back * 360, 2f, RotateMode.FastBeyond360).SetEase(Ease.OutBounce);

        if (GameManager.Instance == null)
        {
            // means that game has started from the level scene
            throw new System.Exception("Please load the game from MainScene");
        }


        // wait for a few seconds
        Invoke("NextLevel", 3.5f);
    }

    private void NextLevel()
    {
        StopAllCoroutines();
        GameManager.Instance.NextLevel();
    }

    public void ShowLevelFailed()
    {
        levelFailedNumberText.text = $"Level {LevelManager.Instance.CurrentLevel}";
        levelFailedPanel.SetActive(true);

        // TODO: remaining goals display


        if (GameManager.Instance == null)
        {
            // means that game has started from the level scene
            throw new System.Exception("Please load the game from MainScene");
        }
        // retry button
        retryButton.onClick.AddListener(() =>
        {
            retryButton.transform.DOKill();
            GameManager.Instance.LoadLevelScene();
        });

        // exit button
        exitButton.onClick.AddListener(() =>
        {
            exitButton.transform.DOKill();
            GameManager.Instance.LoadMainMenu();
        });
    }
}
