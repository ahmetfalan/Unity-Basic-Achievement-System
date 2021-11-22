using UnityEngine;

public abstract class Observer: MonoBehaviour
{
    public abstract void OnNotify(object v1, object v2, NotificationType notificationType);
}