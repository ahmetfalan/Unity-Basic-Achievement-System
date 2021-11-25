using UnityEngine;

public class CollectableAchievements: Subject
{
    [SerializeField]
    private int ID;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Notify(ID);
        Destroy(this.gameObject);
    }
}