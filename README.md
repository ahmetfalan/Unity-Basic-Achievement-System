# Unity Basic Achievement System

I made an achievement system using observer pattern inspiration on Jason Weimann tutorial: [Observer Pattern](https://www.youtube.com/watch?v=Yy7Dt2usGy0&t=312s)

>I'm not very good at explaining like this things, sorry for mistakes.

### Observer Pattern:
>There is two side in Observer Pattern. One side the Subject(Observable) other one is the Observer. 
>The most important benefit of the Observer Pattern is that the Subject notifies the Observer when a situation occurs, instead of the Observer constantly observing. 


Observer abstract class looks like this:
```c#
public abstract class Observer: MonoBehaviour
{
    public abstract void OnNotify(object v1, object v2, NotificationType notificationType); //Making abstract method for overriding calling OnNotify
    //there is 3 parameter => v1 = Tittle of the achievement, v2 = Description of the achievement, notificationType = There is one type:AchievementUnlocked
}
```

Subject abstract class looks like this:
```c#
public abstract class Subject: MonoBehaviour
{
    private List<Observer> observers = new List<Observer>(); //Creating observer list

    public void RegisterObserver(Observer observer) //Register observer list
    {
        observers.Add(observer); //Track the all observers
    }

    public void Notify(object v1, object v2, NotificationType notificationType)
    {
        foreach (var observer in observers)
        {
            observer.OnNotify(v1, v2, notificationType); //Wake up each observer. We say to observer "Hey do your thing"
        }
    }
}
```

AchievementManager.cs looks like this:
```c#
public class AchievementManager: Observer //Inheritance from Observer abstract class because the overriding
{
    public static AchievementManager Instance;

    private void Start()
    {
        Instance = this;
        PlayerPrefs.DeleteAll(); //Delete all prefs for testing

        foreach (var collectableAchievement in FindObjectsOfType<CollectableAchievements>())
        {
            collectableAchievement.RegisterObserver(this); //Register this object on collectable achievements
        }

        foreach (var otherAchievement in FindObjectsOfType<OtherAchievements>())
        {
            otherAchievement.RegisterObserver(this); //Register this object on other achievements
        }
    }
    public override void OnNotify(object v1, object v2, NotificationType notificationType) //Override on OnNotify method
    {
        if (notificationType == NotificationType.AchievementUnlocked) //If incoming type unlocked
        {
            string achievementKey = "Achievement => " + "Tittle: " + v1 + "----" + "Description: " + v2; //Unique key for achievement
            if (PlayerPrefs.GetInt(achievementKey) == 1) //If already unlocked
                return;
            else
            {
                PopUpManager.Instance.Create(v1.ToString(), v2.ToString(), 3f); //Show the popup
                PlayerPrefs.SetInt(achievementKey, 1); //Unlock in prefs
                Debug.Log(achievementKey); //Write the key in console
            }
        }
    }
}
```

CollectableAchievements.cs looks like this:
```c#
public class CollectableAchievements: Subject //Inheritance from Subject abstract class because the overriding
{
    [SerializeField]
    private string achievenemtTittle; //Tittle of achievement

    [SerializeField]
    private string achievenemtDescription; //Description of achievement

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Notify(achievenemtTittle, achievenemtDescription, NotificationType.AchievementUnlocked); //Notify this achievement
        Destroy(this.gameObject); //Destroy the taken object
    }
}
```

OtherAchievements.cs looks like this:
```c#
public class OtherAchievements : Subject //Inheritance from subject
{
    [SerializeField]
    private string achievenemtTittle; //Tittle of achievement

    [SerializeField]
    private string achievenemtDescription; //Description of achievement

    void Update()
    {
        if (PlayerControl.Instance.transform.position.y > 4.5f) //If height greater than 4.5
        {
            Notify(achievenemtTittle, achievenemtDescription, NotificationType.AchievementUnlocked); //Notify this achievement
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
