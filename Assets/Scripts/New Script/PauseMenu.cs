using UnityEngine;
using UnityEngine.SceneManagement; // Essential for loading scenes

public class PauseMenu : MonoBehaviour
{
    [Header("UI Setup")]
    // Drag your UI Panel/Canvas container here
    public GameObject pauseMenuUI; 
    
    // Static flag to check pause state easily from other scripts (optional, but good practice)
    public static bool GameIsPaused = false; 

    void Start()
    {
        // Ensure the pause menu is hidden and time is running at startup
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(false);
        }
        Time.timeScale = 1f;
        GameIsPaused = false;
        
        // Ensure the cursor is locked and hidden at start
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Check for the Escape key input
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    // Public function called to resume the game (linked to the Resume button)
    public void Resume()
    {
        // 1. Hide the pause menu UI
        pauseMenuUI.SetActive(false);
        
        // 2. Restart game time (1 = normal speed)
        Time.timeScale = 1f;
        
        // 3. Update the static state flag
        GameIsPaused = false;
        
        // 4. Lock and hide the cursor for gameplay
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Private function called to pause the game
    void Pause()
    {
        // 1. Show the pause menu UI
        pauseMenuUI.SetActive(true);
        
        // 2. Stop game time (0 = paused - pauses physics, videos, and time-based updates)
        Time.timeScale = 0f;
        
        // 3. Update the static state flag
        GameIsPaused = true;
        
        // 4. Unlock and show the cursor so the player can click buttons
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Public function to load the main menu scene (linked to the Main Menu button)
    public void LoadMenu()
    {
        // Ensure time is running before loading a new scene
        Time.timeScale = 1f; 
        
        // NOTE: Make sure you have added the "MainMenuScene" to your Build Settings!
        SceneManager.LoadScene("MainMenuScene");
    }
}