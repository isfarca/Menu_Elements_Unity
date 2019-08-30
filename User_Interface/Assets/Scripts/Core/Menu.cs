using UnityEngine;
    
public abstract class Menu : MonoBehaviour
{
    // Type of closes.
    public bool destroyWhenClosed = true;
    public bool disableMenuUnderneath = true;
    public bool excludeBackbutton = false;

    [HideInInspector] public MenuManager manager;
}