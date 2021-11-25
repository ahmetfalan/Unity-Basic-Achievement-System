public class OtherAchievements : Subject
{
    void Update()
    {
        if (PlayerControl.Instance.transform.position.y > 4.5f)
        {
            Notify(0);
        }
    }
}
