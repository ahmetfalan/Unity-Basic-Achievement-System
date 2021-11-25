# Unity Basic Achievement System

I made an achievement system using observer pattern inspiration on Jason Weimann tutorial: [Observer Pattern](https://www.youtube.com/watch?v=Yy7Dt2usGy0&t=312s)

>I'm not very good at explaining like this things, sorry for mistakes.

### Observer Pattern:
>There is two side in Observer Pattern. One side the Subject(Observable) other one is the Observer. 
>The most important benefit of the Observer Pattern is that the Subject notifies the Observer when a situation occurs, instead of the Observer constantly observing. 

[Achievement Sprites](https://duct-team.itch.io/black-and-white-achievements-pack?download)

## PopUp
![PopUp](https://github.com/ahmetfalan/Unity-Basic-Achievement-System/blob/main/imgs/PopUp.png)

## Scriptable Object
![Saving Scriptable Object](https://github.com/ahmetfalan/Unity-Basic-Achievement-System/blob/main/imgs/ScriptableAchievement.png)

## Editor
![Editor](https://github.com/ahmetfalan/Unity-Basic-Achievement-System/blob/main/imgs/Editor.png)

Observer abstract class looks like this:
```c#
public abstract class Observer: MonoBehaviour
{
    public abstract void OnNotify(int ID); //Making abstract method for overriding, id parameter for to indicate which object it is
}
```
# Explanin Codes:
Subject abstract class looks like this:
```c#
public abstract class Subject: MonoBehaviour
{
    private List<Observer> observers = new List<Observer>(); //Creating observer list

    public void RegisterObserver(Observer observer) //Register observer list
    {
        observers.Add(observer); //Track the all observers
    }

    public void Notify(int ID)
    {
        foreach (var observer in observers)
        {
            observer.OnNotify(ID); //Wake up the observer. We say to observer "Hey do your thing"
        }
    }
}
```

AchievementManager.cs looks like this:
```c#

public class AchievementManager : Observer
{
    public static AchievementManager Instance;

    public AchievementList achievementList;

    public GameObject achievementPanel; //Content
    public GameObject achievementImage; //Achievement Sprite

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this; //Get instance
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        foreach (var collectableAchievement in FindObjectsOfType<CollectableAchievements>())
        {
            collectableAchievement.RegisterObserver(this); //Register each Collectable Achievement
        }

        foreach (var otherAchievement in FindObjectsOfType<OtherAchievements>())
        {
            otherAchievement.RegisterObserver(this); //Register each Other Achievement
        }

        LoadData(); //Load data for achievement panel
    }

    GameObject gameObject;
    public void LoadData() //Instantiate each achievement
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

    public void UpdateAlp(int ID) //Update achievement alpha when unlock the achievement 
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

    public bool UnlockAchievement(int ID) //Unlock achievement
    {
        achievementList.achievements[ID].Unlocked = true;
        UpdateAlp(ID);
        return true;
    }

    public bool CanAchievementBeUnlocked(int ID) //Check the achievement is unlocked
    {
        bool canUnlock = true;
        if (achievementList.achievements[ID].Unlocked)
        {
            canUnlock = false;
        }
        return canUnlock;
    }

    public override void OnNotify(int ID) //Do this wwhen achievement unlocked
    {
        if (CanAchievementBeUnlocked(ID))
        {
            PopUpManager.Instance.Create(achievementList.achievements[ID].Tittle, achievementList.achievements[ID].Description, 3f);
            UnlockAchievement(ID);
        }
    }
}
```

CollectableAchievements.cs looks like this:
```c#
public class CollectableAchievements: Subject //Inheritance from Subject abstract class because the overriding
{
    [SerializeField]
    private int ID; Send the achievement id for unlocked

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Notify(ID); //Notify this achievement
        Destroy(this.gameObject); //Destroy the taken object
    }
}
```

OtherAchievements.cs looks like this:
```c#
public class OtherAchievements : Subject //Inheritance from subject
{
    void Update()
    {
        if (PlayerControl.Instance.transform.position.y > 4.5f) //If height greater than 4.5
        {
            Notify(0); //Notify this achievement
        }
    }
}
```

PopUp.cs looks like this:
```c#
public class PopUp : MonoBehaviour
{
    public Text Tittle; //Popup tittle
    public Text Description; //Popup description
    public Image Img; //Popup image
    public Image BackgroundImg; //Popup background image
    public float Durability = 5f; //Popup durability
    public float CurrentTime = 0f; //Popup timer

    private void Awake()
    {
        StartCoroutine(OpenAndClose()); //While object creating start timer
    }

    private void Open()
    {
        this.transform.DOMoveY(500.0f, 0.5f).SetEase(Ease.InBack); //Open the popup window
    }

    private void Close() //Close the popup window
    {
        this.transform.DOMoveY(800.0f, 0.5f).SetEase(Ease.InBack);
    }

    IEnumerator OpenAndClose() //Wait on the screen
    {
        yield return new WaitForSeconds(0.1f);
        Open();
        yield return new WaitForSeconds(Durability);
        Close();
        yield return new WaitForSeconds(0.3f);
        Destroy(this.gameObject);
    }
}
```

PopUpManager.cs looks like this:
```c#
public class PopUpManager : MonoBehaviour
{
    public static PopUpManager Instance; //Get instance popup manager

    public PopUp popUp; //The popup object
    public GameObject canvas;

 
    private void Awake()
    {
        Instance = this;
    }

    public void Create(string tittle, string description, float durability) //Create the popup window and equal the all values
    {
        GameObject panel = Instantiate(popUp.gameObject, new Vector2(transform.position.x, 800), Quaternion.identity);
        panel.transform.SetParent(canvas.transform);
        panel.transform.GetComponent<PopUp>().Tittle.text = tittle;
        panel.transform.GetComponent<PopUp>().Description.text = description;
        panel.transform.GetComponent<PopUp>().Durability = durability;
    }
}
```
