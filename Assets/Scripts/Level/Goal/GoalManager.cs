using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The GoalManager class is a Singleton MonoBehaviour that manages the goals in a level.
/// When all goals are completed, OnGoalsCompleted event is invoked.
/// </summary>
public class GoalManager : Singleton<GoalManager>
{
    [SerializeField] private GoalObj goalPrefab;
    [SerializeField] private Transform goalPanel;
    private List<GoalObj> goalObjects = new List<GoalObj>();

    public Action OnGoalsCompleted;
    private bool allGoalsCompleted = false;
    public void Initialize(List<Goal> goals)
    {
        foreach (Goal goal in goals)
        {
            GoalObj goalObject = Instantiate(goalPrefab, goalPanel);
            goalObject.Initialize(goal);
            goalObjects.Add(goalObject);
        }
    }

    public void UpdateLevelGoal(ItemType itemType)
    {
        if (allGoalsCompleted) return;

        var goalObject = goalObjects.Find(goalObj => goalObj.Goal.ItemType.Equals(itemType));

        if (goalObject != null)
        {
            goalObject.DecreaseCount();
            CheckAllGoalsCompleted();
        }

    }

    public bool CheckAllGoalsCompleted()
    {
        foreach (GoalObj goal in goalObjects)
        {
            if (!goal.IsCompleted())
                return false;
        }

        allGoalsCompleted = true;
        OnGoalsCompleted?.Invoke();
        return true;
    }
}