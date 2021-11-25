using System.Collections.Generic;
using UnityEngine;

public abstract class Subject: MonoBehaviour
{
    private List<Observer> observers = new List<Observer>();

    public void RegisterObserver(Observer observer)
    {
        observers.Add(observer);
    }

    public void Notify(int ID)
    {
        foreach (var observer in observers)
        {
            observer.OnNotify(ID);
        }
    }
}