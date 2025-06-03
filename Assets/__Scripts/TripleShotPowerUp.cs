using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TripleShotPowerUp : PowerUp
{
    [Header("Set in Inspector")]
    public int maxShots = 20;
    public float sideAngle = 15f; // Angle en graus per als dispars laterals
    
    protected override void ApplyPowerUp()
    {
        if (PlayerShip.S != null)
        {
            // Afegir el component TripleShotController al jugador
            TripleShotController controller = PlayerShip.S.gameObject.AddComponent<TripleShotController>();
            controller.Initialize(maxShots, sideAngle);
            Debug.Log("Triple dispar activat! Durarà " + maxShots + " dispars.");
        }
    }
}

// Classe auxiliar per gestionar el triple dispar
public class TripleShotController : MonoBehaviour
{
    private int shotsRemaining;
    private float sideAngle;
    private PlayerShip playerShip;
    
    public void Initialize(int maxShots, float angle)
    {
        shotsRemaining = maxShots;
        sideAngle = angle;
        playerShip = GetComponent<PlayerShip>();
        
        // Subscriure's a l'event de dispar del jugador
        // Nota: Caldrà modificar PlayerShip per afegir aquest event
    }
    
    // Aquest mètode serà cridat quan el jugador dispari
    public void OnPlayerFire(Vector3 direction)
    {
        if (shotsRemaining <= 0)
        {
            // Eliminar aquest component quan s'acabin els dispars
            Destroy(this);
            return;
        }
        
        // Crear els dos dispars addicionals
        CreateSideShot(direction, sideAngle);
        CreateSideShot(direction, -sideAngle);
        
        shotsRemaining--;
        Debug.Log("Triple dispar! Queden: " + shotsRemaining);
    }
    
    private void CreateSideShot(Vector3 direction, float angle)
    {
        // Rotar la direcció segons l'angle
        Quaternion rotation = Quaternion.Euler(0, 0, angle);
        Vector3 rotatedDirection = rotation * direction;
        
        // Crear la bala
        GameObject bullet = Instantiate(playerShip.bulletPrefab, transform.position, Quaternion.identity);
        bullet.transform.LookAt(transform.position + rotatedDirection);
    }
}