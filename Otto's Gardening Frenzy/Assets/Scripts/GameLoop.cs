using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class GameLoop : MonoBehaviour
{
    // Game screens
    [SerializeField] private GameObject startMenu;
    [SerializeField] private GameObject tutorial_screen;
    [SerializeField] private GameObject endScreen;


    public float totalTime = 120f; // Total time in seconds (2 minutes)
    private float timeRemaining;

    public TextMeshProUGUI timerText; // Assign a UI Text element in the Inspector

    private bool timerRunning = false;

    private PlayerMovement player; // Variable for player movemnt script

    private void Start()
    {
        timeRemaining = totalTime; // Initialize timeRemaining
        player = FindObjectOfType<PlayerMovement>(); // Sets variable equal to the object found in the scene
        // Disables all screens not in use
        tutorial_screen.SetActive(false);
        endScreen.SetActive(false);
    }

    // This method starts the game
    public void StartGame()
    {
        tutorial_screen.SetActive(false);
        timerRunning = true; // Start the timer
    }

    // This method displays the tutorial screen
    public void NextScreen()
    {
        startMenu.SetActive(false);
        tutorial_screen.SetActive(true);
    }

    // This method restarts the game
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reloads the current scene
    }

    private void Update()
    {
        if (timerRunning) // Checks if the timer is running
        {
            if (timeRemaining > 0) // Checks if there's time remaining
            {
                // Reduces the timer based on Time.deltaTime and updates the display
                timeRemaining -= Time.deltaTime;
                UpdateTimerDisplay(timeRemaining);
            }
            else
            {
                timeRemaining = 0;
                timerRunning = false;
                TimerEnded();
            }
        }
    }

    // This function updates the timer display
    void UpdateTimerDisplay(float timeToDisplay)
    {
        // Ensure no negative values
        if (timeToDisplay < 0) timeToDisplay = 0;

        // Convert time to minutes and seconds
        int minutes = Mathf.FloorToInt(timeToDisplay / 60);
        int seconds = Mathf.FloorToInt(timeToDisplay % 60);

        // Update the Text UI
        timerText.text = $"Time remaining: {minutes:00}:{seconds:00}";
    }

    // This function acts as a timer for the game, and ends the game once the timer is up.
    private void TimerEnded()
    {
        endScreen.SetActive(true); // Enables the end screen
        player.movementEnabled = false; // Disables player movement
    }

}
