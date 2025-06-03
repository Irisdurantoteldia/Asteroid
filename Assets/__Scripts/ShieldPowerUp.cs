using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPowerUp : PowerUp
{
    [Header("Set in Inspector")]
    public GameObject shieldVisualPrefab;
    public int maxHits = 3;
    
    private static GameObject activeShield;
    private static int remainingHits;
    
    protected override void ApplyPowerUp()
    {
        // Si ja hi ha un escut actiu, destruir-lo
        if (activeShield != null)
        {
            Destroy(activeShield);
        }
        
        // Crear l'escut visual al voltant del jugador
        if (PlayerShip.S != null && shieldVisualPrefab != null)
        {
            activeShield = Instantiate(shieldVisualPrefab, PlayerShip.S.transform.position, Quaternion.identity);
            activeShield.transform.parent = PlayerShip.S.transform;
            remainingHits = maxHits;
            
            // Afegir un component per gestionar les col·lisions de l'escut
            ShieldController controller = activeShield.AddComponent<ShieldController>();
            controller.Initialize(maxHits);
            
            Debug.Log("Escut protector activat! Absorbirà " + maxHits + " impactes.");
        }
    }
}

// Classe auxiliar per gestionar l'escut
public class ShieldController : MonoBehaviour
{
    private int hitsRemaining;
    
    public void Initialize(int maxHits)
    {
        hitsRemaining = maxHits;
    }
    
    // Aquest mètode serà cridat des de PlayerShip quan rebi un impacte
    public bool AbsorbHit()
    {
        hitsRemaining--;
        Debug.Log("Escut ha absorbit un impacte! Queden: " + hitsRemaining);
        
        if (hitsRemaining <= 0)
        {
            Debug.Log("Escut destruït!");
            Destroy(gameObject);
            return false; // L'escut ja no pot absorbir més impactes
        }
        
        return true; // L'escut ha absorbit l'impacte
    }
}