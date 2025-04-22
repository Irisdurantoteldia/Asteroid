using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelAdvancePanel : MonoBehaviour {

    public TextMeshProUGUI levelText;
    private bool isVisible = false;
    
    void Awake() {
        // Asegurarse de que el panel esté inicialmente oculto
        gameObject.SetActive(false);
        isVisible = false;
        
        // Registrarse para recibir notificaciones de cambio de estado
        AsteraX.GAME_STATE_CHANGE_DELEGATE += OnGameStateChanged;
        
        // Comprobar el estado actual
        OnGameStateChanged();
    }
    
    void OnDestroy() {
        // Cancelar la subscripción
        AsteraX.GAME_STATE_CHANGE_DELEGATE -= OnGameStateChanged;
    }
    
    void OnGameStateChanged() {
        // Mostrar el panel solo en estado postLevel
        bool shouldBeActive = AsteraX.GAME_STATE == AsteraX.eGameState.postLevel;
        
        if (shouldBeActive != isVisible) {
            isVisible = shouldBeActive;
            gameObject.SetActive(shouldBeActive);
            
            // Actualizar el texto del nivel si está activo
            if (shouldBeActive && levelText != null) {
                // Mostrar el nivel que acaba de completar
                int completedLevel = AsteraX.LEVEL;
                levelText.text = "NIVEL " + completedLevel + " COMPLETADO";
                Debug.Log("Panel de avance de nivel activado para el nivel: " + completedLevel);
            }
        }
    }
    
    IEnumerator AdvanceToNextLevel() {
        yield return new WaitForSecondsRealtime(2f); // Aumentado a 2 segundos para que se vea el panel
        LevelManager levelManager = FindObjectOfType<LevelManager>();
        if (levelManager != null) {
            levelManager.AdvanceToNextLevel();
        }
    }
}