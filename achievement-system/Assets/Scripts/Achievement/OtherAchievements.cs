using UnityEngine;

public class OtherAchievements : Subject
{
    [SerializeField]
    private string achievenemtTittle;

    [SerializeField]
    private string achievenemtDescription;

    void Update()
    {
        if (PlayerControl.Instance.rb.velocity.y > 7.0f)
        {
            Notify(achievenemtTittle, achievenemtDescription, NotificationType.AchievementUnlocked);
        }
    }
}
