using UnityEngine;
using UnityEngine.UI;

public class FinishMenu : Menu
{
    // Elements.
    [SerializeField] private Button backButton;

    /// <summary>
    /// Adds.
    /// </summary>
    private void OnEnable()
    {
        // Add listeners.
        backButton.onClick.AddListener(OnBackClick);
    }

    /// <summary>
    /// Removes.
    /// </summary>
    private void OnDisable()
    {
        // Remove listeners.
        backButton.onClick.RemoveListener(OnBackClick);
    }

    /// <summary>
    /// Go to last menu.
    /// </summary>
    private void OnBackClick()
    {
        // Open start menu.
        manager.Show<StartMenu>();
    }
}