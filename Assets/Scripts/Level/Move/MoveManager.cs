using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class MoveManager : Singleton<MoveManager>
{
    [SerializeField] private TextMeshProUGUI remainingMoveText;
    private int movesLeft;
    public int MovesLeft
    {
        get => movesLeft;
        set
        {
            movesLeft = value;
            remainingMoveText.text = movesLeft.ToString();
        }
    }
    public Action OnMovesFinished;

    public void Initialize(int moves)
    {
        MovesLeft = moves;
    }

    public async Task DecreaseMovesAsync()
    {
        movesLeft--;

        if (movesLeft <= 0)
        {
            TouchHandler.Instance.DisableTouch();
            movesLeft = 0;
            remainingMoveText.text = movesLeft.ToString();
            await Task.Delay(1000);
            OnMovesFinished?.Invoke();
        }

        remainingMoveText.text = movesLeft.ToString();
    }
}
