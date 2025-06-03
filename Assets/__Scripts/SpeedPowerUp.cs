using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedPowerUp : PowerUp
{
    [Header("Set in Inspector")]
    public float speedMultiplier = 1.5f;
    public float duration = 10f; // Durada en segons, 0 per durar fins perdre una vida
    
    protected override void ApplyPowerUp()
    {
        // Guardar la velocitat original
        float originalSpeed = PlayerShip.S.shipSpeed;
        
        // Aplicar l'increment de velocitat
        PlayerShip.S.shipSpeed *= speedMultiplier;
        Debug.Log("Velocitat incrementada! Nova velocitat: " + PlayerShip.S.shipSpeed);
        
        // Si té durada temporal, iniciar coroutine per restaurar
        if (duration > 0)
        {
            StartCoroutine(RestoreSpeedAfterDelay(originalSpeed));
        }
    }
    
    private IEnumerator RestoreSpeedAfterDelay(float originalSpeed)
    {
        yield return new WaitForSeconds(duration);
        
        // Restaurar velocitat original si el jugador encara existeix
        if (PlayerShip.S != null)
        {
            PlayerShip.S.shipSpeed = originalSpeed;
            Debug.Log("Velocitat restaurada a: " + originalSpeed);
        }
    }
}