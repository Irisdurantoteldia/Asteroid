using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelManager : MonoBehaviour
{
    [Header("Set in Inspector")]
    public GameObject titleScreenPanel;
    public GameObject levelAdvancePanel;
    public GameObject gameOverPanel;
    public Toggle pauseToggle;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI gameOverScoreText;
    public TextMeshProUGUI gameOverLevelText;
    public Button startButton;
    
    [Header("Set Dynamically")]
    public int currentLevel = 0;
    public int initialAsteroids = 3;
    public int childrenPerAsteroid = 2;
    
    private bool isPaused = false;
    
    void Awake()
    {
        Debug.Log("LevelManager Awake");
        // Subscribe to game state change event
        AsteraX.GAME_STATE_CHANGE_DELEGATE += OnGameStateChange;
        
        // Add click listener to start button
        startButton.onClick.AddListener(StartGame);
    }
    
    void Start()
    {
        Debug.Log("LevelManager Start");
        // Initialize the game in main menu state
        if (AsteraX.GAME_STATE == AsteraX.eGameState.none)
        {
            AsteraX.GAME_STATE = AsteraX.eGameState.mainMenu;
        }
    }
    
    void OnDestroy()
    {
        Debug.Log("LevelManager OnDestroy");
        // Unsubscribe from game state change event
        if (AsteraX.GAME_STATE_CHANGE_DELEGATE != null)
        {
            AsteraX.GAME_STATE_CHANGE_DELEGATE -= OnGameStateChange;
        }
    }
    
    void OnGameStateChange()
    {
        Debug.Log("Game State Changed to: " + AsteraX.GAME_STATE);
        // Handle state changes
        switch (AsteraX.GAME_STATE)
        {
            case AsteraX.eGameState.mainMenu:
                Debug.Log("Main Menu state");
                Time.timeScale = 1f;
                break;
                
            case AsteraX.eGameState.preLevel:
                Debug.Log("PreLevel state");
                UpdateLevelText();
                StartCoroutine(PreLevelDelay());
                break;
                
            case AsteraX.eGameState.level:
                Debug.Log("Level state");
                Time.timeScale = 1f;
                break;
                
            case AsteraX.eGameState.postLevel:
                Debug.Log("PostLevel state");
                // Mostrar el panel de avance de nivel
                if (levelAdvancePanel != null) {
                    levelAdvancePanel.SetActive(true);
                }
                // Clean up bullets and prepare for next level
                ClearBullets();
                StartCoroutine(PostLevelDelay());
                break;
                
            case AsteraX.eGameState.gameOver:
                Debug.Log("GameOver state");
                UpdateGameOverPanel();
                StartCoroutine(GameOverDelay());
                break;
        }
    }
    
    void UpdateLevelText()
    {
        if (levelText != null)
        {
            levelText.text = "LEVEL " + currentLevel;
        }
    }
    
    void UpdateGameOverPanel()
    {
        if (gameOverScoreText != null)
        {
            gameOverScoreText.text = "Final Score: " + AsteraX.SCORE;
        }
        
        if (gameOverLevelText != null)
        {
            gameOverLevelText.text = "Final Level: " + currentLevel;
        }
    }
    
    IEnumerator PreLevelDelay()
    {
        // Pause briefly at the start of a level
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(0.5f);
        
        // Only start level if we're still in preLevel state
        if (AsteraX.GAME_STATE == AsteraX.eGameState.preLevel)
        {
            // Establir les propietats abans de cridar StartLevel
            AsteraX.NumInitialAsteroids = initialAsteroids;
            AsteraX.NumAsteroidChildren = childrenPerAsteroid;
            
            // Cambiar al estado de nivel antes de iniciar el nivel
            AsteraX.GAME_STATE = AsteraX.eGameState.level;
            
            // Iniciar el nivell
            AsteraX.StartGame();
        }
    }
    
    IEnumerator PostLevelDelay()
    {
        // Pause between levels
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(2f);
        
        // Ocultar el panel de avance de nivel
        if (levelAdvancePanel != null) {
            levelAdvancePanel.SetActive(false);
        }
        
        // Advance to next level
        AdvanceToNextLevel();
    }
    
    IEnumerator GameOverDelay()
    {
        // Wait before returning to title screen
        yield return new WaitForSecondsRealtime(4f);
        
        // Return to main menu
        AsteraX.GAME_STATE = AsteraX.eGameState.mainMenu;
        currentLevel = 0;
    }
    
    public void StartGame()
    {
        currentLevel = 1;
        ParseLevelSettings();
        AsteraX.GAME_STATE = AsteraX.eGameState.preLevel;
    }
    
    public void AdvanceToNextLevel()
    {
        Debug.Log("Avanzando al siguiente nivel desde: " + currentLevel);
        currentLevel++;
        
        // If we've completed all levels, go to game over
        if (currentLevel > 10)
        {
            AsteraX.GAME_STATE = AsteraX.eGameState.gameOver;
            return;
        }
        
        // Otherwise parse the level settings and go to preLevel state
        ParseLevelSettings();
        AsteraX.GAME_STATE = AsteraX.eGameState.preLevel;
    }
    
    public void ParseLevelSettings()
    {
        // Get the level progression string from AsteraX
        string progression = AsteraX.LevelProgression;
        
        // Split by commas to get each level
        string[] levels = progression.Split(',');
        
        // Find settings for the current level
        foreach (string level in levels)
        {
            string[] parts = level.Split(':');
            if (parts.Length == 2 && int.Parse(parts[0]) == currentLevel)
            {
                string[] settings = parts[1].Split('/');
                if (settings.Length == 2)
                {
                    initialAsteroids = int.Parse(settings[0]);
                    childrenPerAsteroid = int.Parse(settings[1]);
                    Debug.Log("Configuración del nivel " + currentLevel + ": " + initialAsteroids + " asteroides, " + childrenPerAsteroid + " hijos");
                    return;
                }
            }
        }
        
        // Default values if level not found
        initialAsteroids = 3;
        childrenPerAsteroid = 2;
        Debug.Log("Usando valores por defecto para el nivel " + currentLevel);
    }
    
    void ClearBullets()
    {
        // Find and destroy all bullets
        Bullet[] bullets = FindObjectsOfType<Bullet>();
        foreach (Bullet bullet in bullets)
        {
            Destroy(bullet.gameObject);
        }
    }
    
    public void TogglePause(bool pause)
    {
        isPaused = pause;
        Time.timeScale = isPaused ? 0f : 1f;
    }
}