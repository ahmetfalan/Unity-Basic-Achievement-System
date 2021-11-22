using UnityEngine;

public class OtherAchievements : Subject
{
    [SerializeField]
    private string achievenemtTittle;

    [SerializeField]
    private string achievenemtDescription;

    void Update()
    {
        if (PlayerControl.Instance.transform.position.y > 4.5f)
        {
            Notify(achievenemtTittle, achievenemtDescription, NotificationType.AchievementUnlocked);
        }
    }
}
