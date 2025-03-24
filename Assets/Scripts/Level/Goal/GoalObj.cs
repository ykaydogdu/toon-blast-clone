using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GoalObj : MonoBehaviour
{
    [SerializeField] private Image goalImg;
    [SerializeField] private Image completedImage;
    [SerializeField] private TextMeshProUGUI goalCountText;

    private Goal goal;
    public Goal Goal => goal;

    int count;

    public void Initialize(Goal goal)
    {
        this.goal = goal;
        var sprite = ItemImageLibrary.Instance.GetSprite(goal.ItemType);
        goalImg.sprite = sprite;
        count = goal.Count;
        UpdateCountText();
    }

    public void DecreaseCount()
    {
        count--;
        
        if (count <= 0)
        {
            count = 0;
            goalCountText.gameObject.SetActive(false);
            completedImage.gameObject.SetActive(true);
            return;
        }

        UpdateCountText();
    }

    public bool IsCompleted()
    {
        return count == 0;
    }

    private void UpdateCountText()
    {
        goalCountText.text = count.ToString();
    }
}
