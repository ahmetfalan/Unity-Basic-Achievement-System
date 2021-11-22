using UnityEngine;

public class CollectableAchievements: Subject
{
    [SerializeField]
    private string achievenemtTittle;

    [SerializeField]
    private string achievenemtDescription;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Notify(achievenemtTittle, achievenemtDescription, NotificationType.AchievementUnlocked);
        Destroy(this.gameObject);
    }
}