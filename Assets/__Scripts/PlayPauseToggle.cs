using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class PlayPauseToggle : MonoBehaviour {
    [Header("UI Elements")]
    public Image playIcon;    
    public Image pauseIcon;   
    
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
        
        // Canviar les icones
        if (playIcon != null) playIcon.gameObject.SetActive(!isPaused);
        if (pauseIcon != null) pauseIcon.gameObject.SetActive(isPaused);
        
        // Obtener la referencia al PlayerShip
        PlayerShip playerShip = FindObjectOfType<PlayerShip>();
        if (playerShip != null) {
            // Desactivar el component sencer per evitar qualsevol interacció
            playerShip.enabled = !isPaused;
            
            // També desactivar el Rigidbody per evitar moviments físics
            Rigidbody rb = playerShip.GetComponent<Rigidbody>();
            if (rb != null) {
                rb.isKinematic = isPaused;
                if (!isPaused) {
                    // Reiniciar la velocitat quan es reprèn el joc
                    rb.velocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                }
            }
        }
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