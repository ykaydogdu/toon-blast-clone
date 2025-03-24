using UnityEngine;

public class LevelComplete : MonoBehaviour
{
    [SerializeField] private GameBoard board;

    private void OnEnable()
    {
        GoalManager.Instance.OnGoalsCompleted += ShowLevelComplete;
    }

    private void OnDisable()
    {
        GoalManager.Instance.OnGoalsCompleted -= ShowLevelComplete;
    }

    private void ShowLevelComplete()
    {
        UIManager.Instance.ShowLevelComplete();
    }
}
