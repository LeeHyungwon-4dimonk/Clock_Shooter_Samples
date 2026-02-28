using UnityEngine;

public enum UIType
{
    Screen, Popup
}

public abstract class UIBase : MonoBehaviour
{
    public virtual void Open()
    {
        gameObject.SetActive(true);
    }

    public virtual void Close()
    {
        gameObject.SetActive(false);
    }
}