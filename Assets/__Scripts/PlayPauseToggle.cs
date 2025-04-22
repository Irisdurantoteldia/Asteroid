using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class PlayPauseToggle : MonoBehaviour {

    private Toggle toggle;
    private bool isVisible = false;
    
    void Awake() {
        toggle = GetComponent<Toggle>();
        if (toggle != null) {
            toggle.onValueChanged.AddListener(OnToggleValueChanged);
        }
        
        // Registrar-se per rebre notificacions de canvi d'estat
        AsteraX.GAME_STATE_CHANGE_DELEGATE += OnGameStateChanged;
        
        // Comprovar l'estat actual
        OnGameStateChanged();
    }
    
    void OnDestroy() {
        if (toggle != null) {
            toggle.onValueChanged.RemoveListener(OnToggleValueChanged);
        }
        
        // Cancel·lar la subscripció
        AsteraX.GAME_STATE_CHANGE_DELEGATE -= OnGameStateChanged;
    }
    
    void OnToggleValueChanged(bool isPaused) {
        // Aplicar l'estat de pausa
        AsteraX.TogglePause(isPaused);
    }
    
    void OnGameStateChanged() {
        // Determinar si el toggle hauria d'estar actiu
        bool shouldBeActive = AsteraX.GAME_STATE == AsteraX.eGameState.level || 
                            AsteraX.GAME_STATE == AsteraX.eGameState.paused;
        
        if (shouldBeActive != isVisible) {
            isVisible = shouldBeActive;
            gameObject.SetActive(shouldBeActive);
            
            // Actualitzar l'estat del toggle segons l'estat del joc
            if (shouldBeActive) {
                toggle.isOn = AsteraX.GAME_STATE == AsteraX.eGameState.paused;
            }
        }
    }
}