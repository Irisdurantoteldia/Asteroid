using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameOverPanel : MonoBehaviour {

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI levelText;
    private bool isVisible = false;
    
    void Awake() {
        // Registrar-se per rebre notificacions de canvi d'estat
        AsteraX.GAME_STATE_CHANGE_DELEGATE += OnGameStateChanged;
        
        // Comprovar l'estat actual
        OnGameStateChanged();
    }
    
    void OnDestroy() {
        // Cancel·lar la subscripció
        AsteraX.GAME_STATE_CHANGE_DELEGATE -= OnGameStateChanged;
    }
    
    void OnGameStateChanged() {
        bool shouldBeActive = AsteraX.GAME_STATE == AsteraX.eGameState.gameOver;
        
        if (shouldBeActive != isVisible) {
            isVisible = shouldBeActive;
            gameObject.SetActive(shouldBeActive);
            
            if (shouldBeActive) {
                // Actualitzar els textos
                if (scoreText != null) {
                    scoreText.text = "FINAL SCORE: " + AsteraX.SCORE;
                }
                
                if (levelText != null) {
                    levelText.text = "FINAL LEVEL: " + AsteraX.LEVEL;
                }
                
                // Iniciar la corrutina per tornar al menú principal
                StartCoroutine(ReturnToTitleScreen());
            }
        }
    }
    
    IEnumerator ReturnToTitleScreen() {
        yield return new WaitForSecondsRealtime(4f);
        AsteraX.GAME_STATE = AsteraX.eGameState.mainMenu;
    }
}