using System.Collections.Generic;
using UnityEngine;
using Malee;

public class MenuManager : MonoBehaviour
{
    // Menus.
    private readonly Stack<Menu> activeMenuStack = new Stack<Menu>();
    private readonly Stack<GameObject> prefabMenuStack = new Stack<GameObject>();

    [Reorderable] public MenuList menus;
    private Menu currentMenu;

    /// <summary>
    /// Show the first menu.
    /// </summary>
    private void Start()
    {
        Show(menus[0].prefab);
    }

    /// <summary>
    /// Check is the back pressed button called.
    /// </summary>
    private void Update()
    {
        // Back button pressed?
        if (!Input.GetKeyDown(KeyCode.B)) // No.
        {
            // Exit this function.
            return;
        }
            
        // Back pressed.
        OnBackPressed();
    }

    /// <summary>
    /// Open last menu.
    /// </summary>
    private void OnBackPressed()
    {
        // Have a prefab in a prefab menu stack?
        if (prefabMenuStack.Count <= 1) // No, one or fever, so that the first menu is displayed.
            return; // Exit this function.
            
        // Close the current opened menu.
        CloseTopMenu();
            
        // How many menu's are active?
        if (activeMenuStack.Count <= 0) // No => Menus are instantiated!
        {
            // Pop the opened menu from prefab menu stack.
            prefabMenuStack.Pop();
                
            // Open the last popped menu from prefab menu stack.
            Show(prefabMenuStack.Pop());
        }
        else if (activeMenuStack.Count >= 1) // One or more => Menus are activated!
        {
            // Open the last activated menu.
            Show(activeMenuStack.Peek().gameObject);
        }
    }

    /// <summary>
    /// Reset.
    /// </summary>
    private void OnDisable()
    {
        // Is a current menu opened?
        if (currentMenu != null) // Yes.
            ClosedMenu(currentMenu);

        // Open the first menu.
        Show(menus[0].prefab);
    }

    /// <summary>
    /// Overload for showing a menu via generic parameter.
    /// </summary>
    /// <typeparam name="T">Menu type.</typeparam>
    public void Show<T>() where T : Menu
    {
        if (currentMenu != null)
            ClosedMenu(currentMenu);

        var prefab = menus.GetPrefab<T>();

        // By doesn't menu prefabs, output the error.
        if (prefab == null)
            throw new MissingReferenceException("No prefab of type " + typeof(T) + " found!");

        Show(prefab);
    }

    /// <summary>
    /// Show the menu with the given prefab.
    /// </summary>
    /// <param name="prefab"></param>
    private void Show(GameObject prefab)
    {
        if (currentMenu == null)
        {
            var go = Instantiate(prefab, transform);
            currentMenu = go.GetComponent<Menu>();
            currentMenu.manager = this;

            // Push the prefab to stack.
            if (!currentMenu.excludeBackbutton)
                prefabMenuStack.Push(prefab);
        }
        else
        {
            currentMenu.gameObject.SetActive(true);
        }

        OpenMenu(currentMenu);
    }

    /// <summary>
    /// Open the menu by instance.
    /// </summary>
    /// <param name="instance">The instance of the menu.</param>
    private void OpenMenu(Menu instance)
    {
        // Opening menu.
        if (activeMenuStack.Count > 0)
        {
            if (instance.disableMenuUnderneath)
            {
                foreach (var menu in activeMenuStack)
                {
                    var canvasGroup = menu.GetComponent<CanvasGroup>();

                    if (canvasGroup != null)
                    {
                        canvasGroup.interactable = false;
                    }
                    else
                    {
                        menu.gameObject.SetActive(false);
                    }

                    if (menu.disableMenuUnderneath)
                    {
                        break;
                    }
                }
            }

            // Get the first and last menu.
            var topCanvas = instance.GetComponent<Canvas>();
            var prevCanvas = activeMenuStack.Peek().GetComponent<Canvas>();

            // Sorting all menus.
            topCanvas.sortingOrder = prevCanvas.sortingOrder + 1;
        }

        // Push the above instance.
        activeMenuStack.Push(instance);
    }

    /// <summary>
    /// Close the menu by instance.
    /// </summary>
    /// <param name="instance">The instance of the menu.</param>
    private void ClosedMenu(Menu instance)
    {
        // Close the menu by instance, else throw exception.
        if (instance == null)
        {
            Debug.LogErrorFormat("No menu of type {0} is currently open.", typeof(Menu));

            return;
        }
            
        // By no menus, output the error.
        if (activeMenuStack.Count <= 0)
        {
            Debug.LogErrorFormat("Stack is empty! {0} cannot be closed.", instance.GetType());

            return;
        }

        // When instance unequal the top of the stack, than output the error.
        if (activeMenuStack.Peek() != instance)
        {
            Debug.LogErrorFormat("{0} is not the top menu.", instance.GetType());

            return;
        }

        // Else, closing the menu.
        CloseTopMenu();
    }

    /// <summary>
    /// Closing the menu of the top of the stack.
    /// </summary>
    private void CloseTopMenu()
    {
        // Get top menu instance.
        var instance = activeMenuStack.Pop();

        // Destroy the menu.
        if (instance.destroyWhenClosed)
        {
            Destroy(instance.gameObject);
            currentMenu = null;
        }
        else
        {
            instance.gameObject.SetActive(false);
        }

        // Activate all canvas.
        foreach (var menu in activeMenuStack)
        {
            var canvasGroup = menu.GetComponent<CanvasGroup>();

            if (canvasGroup != null)
            {
                canvasGroup.interactable = true;
            }
            else
            {
                menu.gameObject.SetActive(true);
            }

            if (menu.disableMenuUnderneath)
            {
                break;
            }
        }
    }
}