using UnityEngine;

public abstract class Observer: MonoBehaviour
{
    public abstract void OnNotify(int ID);
}