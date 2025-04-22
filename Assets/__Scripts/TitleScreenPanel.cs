using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TitleScreenPanel : MonoBehaviour {
    [Header("Set in Inspector")]
    public Button startButton;

    void Start() {
        Debug.Log("TitleScreenPanel Start");
        
        // Configurar el botón
        if (startButton != null) {
            startButton.onClick.RemoveAllListeners();
            startButton.onClick.AddListener(StartGame);
            Debug.Log("Start button configured");
        } else {
            Debug.LogError("Start button not assigned!");
        }
    }

    public void StartGame() {
        Debug.Log("StartGame called - Intentando ocultar el panel");
        
        // Ocultar el panel de título (usando el objeto actual)
        gameObject.SetActive(false);
        
        // Cambiar el estado del juego a preLevel
        Debug.Log("Cambiando estado del juego a preLevel");
        AsteraX.GAME_STATE = AsteraX.eGameState.preLevel;
        
        // Iniciar el primer nivel inmediatamente
        LevelManager levelManager = FindObjectOfType<LevelManager>();
        if (levelManager != null) {
            Debug.Log("Configurando nivel 1");
            levelManager.currentLevel = 1;
            levelManager.ParseLevelSettings(); // Asegurar que los settings están cargados
            AsteraX.NumInitialAsteroids = levelManager.initialAsteroids;
            AsteraX.NumAsteroidChildren = levelManager.childrenPerAsteroid;
        } else {
            Debug.LogError("No se encontró el LevelManager!");
        }
    }
}