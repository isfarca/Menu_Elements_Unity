using UnityEngine;
using UnityEngine.UI;

public class StartMenu : Menu
{
    // Elements.
    [SerializeField] private Button startButton;
    [SerializeField] private Button quitButton;

    /// <summary>
    /// Adds.
    /// </summary>
    private void OnEnable()
    {
        // Add listeners.
        startButton.onClick.AddListener(OnStartClick);
        quitButton.onClick.AddListener(OnQuitClick);
    }

    /// <summary>
    /// Removes.
    /// </summary>
    private void OnDisable()
    {
        // Remove listeners.
        startButton.onClick.RemoveListener(OnStartClick);
        quitButton.onClick.RemoveListener(OnQuitClick);
    }

    /// <summary>
    /// Open the game.
    /// </summary>
    private void OnStartClick()
    {
        // Show the finish menu for testing.
        manager.Show<FinishMenu>();
    }

    /// <summary>
    /// Quit the application
    /// </summary>
    private void OnQuitClick()
    {
        Application.Quit();
    }
}