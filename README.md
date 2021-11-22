# Unity Basic Achievement System

I made an achievement system using observer pattern inspiration on Jason Weimann tutorial: [Observer Pattern](https://www.youtube.com/watch?v=Yy7Dt2usGy0&t=312s)

>I'm not very good at explaining like this things, sorry for mistakes.



Observer.cs looks like this:
```c#
public abstract class Observer: MonoBehaviour
{
    public abstract void OnNotify(object v1, object v2, NotificationType notificationType); //making abstract method for overriding calling OnNotify
    //there is 3 parameter => v1 = tittle of the achievement, v2 = description of the achievement, notificationType = there is one type:AchievementUnlocked
}
```

Subject.cs looks like this:
```c#
public abstract class Subject: MonoBehaviour
{
    private List<Observer> observers = new List<Observer>(); //creating observer list

    public void RegisterObserver(Observer observer) //register observer list
    {
        observers.Add(observer);
    }

    public void Notify(object v1, object v2, NotificationType notificationType)
    {
        foreach (var observer in observers)
        {
            observer.OnNotify(v1, v2, notificationType); //wake up each observer
        }
    }
}
```

AchievementManager.cs looks like this:
```c#
public class AchievementManager: Observer //inheritance from observer
{
    public static AchievementManager Instance;

    private void Start()
    {
        Instance = this;
        PlayerPrefs.DeleteAll(); //delete all prefs for testing

        foreach (var collectableAchievement in FindObjectsOfType<CollectableAchievements>())
        {
            collectableAchievement.RegisterObserver(this); //register this object on collectable achievement
        }

        foreach (var otherAchievement in FindObjectsOfType<OtherAchievements>())
        {
            otherAchievement.RegisterObserver(this); //register this object on other achievement
        }
    }
    public override void OnNotify(object v1, object v2, NotificationType notificationType)
    {
        if (notificationType == NotificationType.AchievementUnlocked) //if incoming type unlocked
        {
            string achievementKey = "Achievement => " + "Tittle: " + v1 + "----" + "Description: " + v2; //unique key for achievement
            if (PlayerPrefs.GetInt(achievementKey) == 1) //if already unlocked
                return;
            else
            {
                PopUpManager.Instance.Create(v1.ToString(), v2.ToString(), 3f); //show the popup
                PlayerPrefs.SetInt(achievementKey, 1); //unlock in prefs
                Debug.Log(achievementKey); //write the key
            }
        }
    }
}
```

CollectableAchievements.cs looks like this:
```c#
public class CollectableAchievements: Subject //inheritance from observer
{
    [SerializeField]
    private string achievenemtTittle; //tittle of achievement

    [SerializeField]
    private string achievenemtDescription; //description of achievement

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Notify(achievenemtTittle, achievenemtDescription, NotificationType.AchievementUnlocked); //notify this achievement
        Destroy(this.gameObject); //destroy taken object
    }
}
```

CollectableAchievements.cs looks like this:
```c#
public class OtherAchievements : Subject //inheritance from observer
{
    [SerializeField]
    private string achievenemtTittle; //tittle of achievement

    [SerializeField]
    private string achievenemtDescription; //description of achievement

    void Update()
    {
        if (PlayerControl.Instance.transform.position.y > 4.5f) //if height greater than 4.5
        {
            Notify(achievenemtTittle, achievenemtDescription, NotificationType.AchievementUnlocked); //notify this achievement
        }
    }
}
```

PopUp.cs looks like this:
```c#
public class PopUp : MonoBehaviour
{
    public Text Tittle; //popup tittle
    public Text Description; //popup description
    public Image Img; //popup image
    public Image BackgroundImg; //popup background image
    public float Durability = 5f; //popup durability
    public float CurrentTime = 0f; //popup timer

    private void Awake()
    {
        StartCoroutine(OpenAndClose()); //while object creating start timer
    }

    private void Open()
    {
        this.transform.DOMoveY(500.0f, 0.5f).SetEase(Ease.InBack); //open the popup window
    }

    private void Close() //close the popup window
    {
        this.transform.DOMoveY(800.0f, 0.5f).SetEase(Ease.InBack);
    }

    IEnumerator OpenAndClose() //wait on the screen
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

PopUp.cs looks like this:
```c#
public class PopUpManager : MonoBehaviour
{
    public static PopUpManager Instance; //get instance popup manager

    public PopUp popUp; //
    public GameObject canvas;

 
    private void Awake()
    {
        Instance = this;
    }

    public void Create(string tittle, string description, float durability) //create the popup window and equal the all values
    {
        GameObject panel = Instantiate(popUp.gameObject, new Vector2(transform.position.x, 800), Quaternion.identity);
        panel.transform.SetParent(canvas.transform);
        panel.transform.GetComponent<PopUp>().Tittle.text = tittle;
        panel.transform.GetComponent<PopUp>().Description.text = description;
        panel.transform.GetComponent<PopUp>().Durability = durability;
    }
}
```
