using UnityEngine;
using System.Collections; // Required for Coroutines
using UnityEngine.SceneManagement;

public class EndGameButton : MonoBehaviour
{
    [Header("Credit Setup")]
    public GameObject creditPanel; 
    public float creditDuration = 10f; 
    public string mainMenuSceneName = "MainMenuScene"; 
    
    // Flag to prevent double-ending
    private bool isEnding = false;

    // Public method called by Raycast_Interaction
    public void EndGame()
    {
        if (isEnding) return;
        isEnding = true;

        // 1. Ensure time is running for the coroutine to tick down
        Time.timeScale = 1f;
        
        // 2. Show the credit UI panel
        if (creditPanel != null)
        {
            creditPanel.SetActive(true);
        }
        
        // 3. Start the timed sequence
        StartCoroutine(CreditSequence(creditDuration));
    }

    IEnumerator CreditSequence(float duration)
    {
        // Wait for the specified duration
        yield return new WaitForSeconds(duration);

        // 4. After the wait, load the main menu scene
        LoadMainMenu();
    }

    private void LoadMainMenu()
    {
        // Ensure time is running before leaving the scene
        Time.timeScale = 1f; 
        
        // CRITICAL FIX: Unlock and show the cursor BEFORE loading the new scene!
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true; 
        
        // Load the main menu scene
        SceneManager.LoadScene(mainMenuSceneName);
    }
}