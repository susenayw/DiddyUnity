using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameButton : MonoBehaviour
{
    [Header("Credit Setup")]
    public GameObject creditPanel; // Drag your Credit UI Panel here
    public Animator creditAnimator; // Drag the Animator component from the Credit Panel here
    public string endCreditAnimationName = "CreditRoll"; // Name of the animation clip to play
    public string mainMenuSceneName = "MainMenuScene.unity"; // Name of the scene to load

    private bool isEnding = false;

    // Public method called by Raycast_Interaction
    public void EndGame()
    {
        if (isEnding) return;

        isEnding = true;
        
        // Ensure the pause menu is closed and time is running for the credits
        if (PauseMenu.GameIsPaused)
        {
            Time.timeScale = 1f;
        }

        // 1. Show the credit UI panel
        if (creditPanel != null)
        {
            creditPanel.SetActive(true);
        }

        // 2. Start the credit animation
        if (creditAnimator != null)
        {
            creditAnimator.Play(endCreditAnimationName);
            
            // 3. Calculate and invoke the scene load after the animation duration
            float clipLength = GetClipLength(endCreditAnimationName);
            Invoke("LoadMainMenu", clipLength);
        }
        else
        {
            // Fallback if no animator is found
            LoadMainMenu();
        }
    }

    private float GetClipLength(string clipName)
    {
        if (creditAnimator == null) return 0f;

        // Search the animator's clips for the specific animation duration
        RuntimeAnimatorController ac = creditAnimator.runtimeAnimatorController;
        foreach (AnimationClip clip in ac.animationClips)
        {
            if (clip.name == clipName)
            {
                return clip.length;
            }
        }
        Debug.LogError("Credit animation clip not found: " + clipName + ". Loading scene immediately.");
        return 0f;
    }

    private void LoadMainMenu()
    {
        // Unlock the cursor before leaving the scene
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        // Load the main menu scene
        SceneManager.LoadScene(mainMenuSceneName);
    }
}