using System.Collections.Generic;
using UnityEngine;

public abstract class Subject: MonoBehaviour
{
    private List<Observer> observers = new List<Observer>();

    public void RegisterObserver(Observer observer)
    {
        observers.Add(observer);
    }

    public void Notify(object v1, object v2, NotificationType notificationType)
    {
        foreach (var observer in observers)
        {
            observer.OnNotify(v1, v2, notificationType);
        }
    }
}