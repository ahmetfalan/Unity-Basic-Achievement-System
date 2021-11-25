using UnityEngine;
using UnityEngine.UI;

public class AchievementManager : Observer
{
    public static AchievementManager Instance;

    string path = "Assets/Scripts/Data/Achievements.json";
    public AchievementList achievementList;

    public GameObject achievementPanel;
    public GameObject achievementImage;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        PlayerPrefs.DeleteAll();
        foreach (var collectableAchievement in FindObjectsOfType<CollectableAchievements>())
        {
            collectableAchievement.RegisterObserver(this);
        }

        foreach (var otherAchievement in FindObjectsOfType<OtherAchievements>())
        {
            otherAchievement.RegisterObserver(this);
        }

        LoadData();
    }

    GameObject gameObject;
    public void LoadData()
    {
        foreach (var achievement in achievementList.achievements)
        {
            gameObject = Instantiate(achievementImage, achievementPanel.transform, achievementPanel.transform);
            gameObject.transform.SetParent(achievementPanel.transform);
            gameObject.transform.GetChild(0).GetComponent<Image>().sprite = achievement.BackgroundImg;
            gameObject.transform.GetChild(1).GetComponent<Image>().sprite = achievement.Img;

            gameObject.GetComponent<Achievement>().ID = achievement.ID;

            Color alp = gameObject.transform.GetChild(1).GetComponent<Image>().color;
            alp.a = 0.1f;

            if (!achievement.Unlocked)
                gameObject.transform.GetChild(1).GetComponent<Image>().color = alp;
        }
    }

    public void UpdateAlp(int ID)
    {
        foreach (var achievenent in FindObjectsOfType<Achievement>())
        {
            if (ID == achievenent.ID)
            {
                Color alp = achievenent.gameObject.transform.GetChild(1).GetComponent<Image>().color;
                alp.a = 1f;
                achievenent.gameObject.transform.GetChild(1).GetComponent<Image>().color = alp;
            }
        }
    }

    public bool UnlockAchievement(int ID)
    {
        achievementList.achievements[ID].Unlocked = true;
        UpdateAlp(ID);
        return true;
    }
    public bool CanAchievementBeUnlocked(int ID)
    {
        bool canUnlock = true;
        if (achievementList.achievements[ID].Unlocked)
        {
            canUnlock = false;
        }
        return canUnlock;
    }
    public override void OnNotify(int ID)
    {
        if (CanAchievementBeUnlocked(ID))
        {
            PopUpManager.Instance.Create(achievementList.achievements[ID].Tittle, achievementList.achievements[ID].Description, 3f);
            UnlockAchievement(ID);
        }
    }
}
