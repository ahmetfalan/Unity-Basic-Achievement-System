using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementManager: Observer
{
    public static AchievementManager Instance;

    private void Start()
    {
        Instance = this;
        PlayerPrefs.DeleteAll();

        foreach (var collectableAchievement in FindObjectsOfType<CollectableAchievements>())
        {
            collectableAchievement.RegisterObserver(this);
        }

        foreach (var otherAchievement in FindObjectsOfType<OtherAchievements>())
        {
            otherAchievement.RegisterObserver(this);
        }
    }
    public override void OnNotify(object v1, object v2, NotificationType notificationType)
    {
        if (notificationType == NotificationType.AchievementUnlocked)
        {
            string achievementKey = "Achievement => " + "Tittle: " + v1 + "----" + "Description: " + v2;
            if (PlayerPrefs.GetInt(achievementKey) == 1)
                return;
            else
            {
                PopUpManager.Instance.Open(v1.ToString(), v2.ToString(), 3f);
                PlayerPrefs.SetInt(achievementKey, 1);
                Debug.Log(achievementKey);
            }
        }
    }
}
