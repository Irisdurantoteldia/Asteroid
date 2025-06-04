using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TripleShotPowerUp : PowerUp
{
    [Header("Set in Inspector")]
    public int maxShots = 20;
    public float sideAngle = 45f; // Ángulo en grados para los disparos laterales
    
    protected override void ApplyPowerUp()
    {
        if (PlayerShip.S != null)
        {
            // Añadir el componente TripleShotController al jugador
            TripleShotController controller = PlayerShip.S.gameObject.AddComponent<TripleShotController>();
            controller.Initialize(maxShots, sideAngle);
            Debug.Log("Triple disparo activado! Durará " + maxShots + " disparos.");
        }
    }
}

// Clase auxiliar para gestionar el triple disparo
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
        
        // Suscribirse al evento de disparo del jugador
        if (playerShip != null)
        {
            playerShip.OnFire += OnPlayerFire;
            Debug.Log("TripleShotController suscrito al evento OnFire");
        }
    }
    
    private void OnDestroy()
    {
        // Desuscribirse del evento cuando se destruya el componente
        if (playerShip != null)
        {
            playerShip.OnFire -= OnPlayerFire;
        }
    }
    
    // Este método será llamado cuando el jugador dispare
    public void OnPlayerFire(Vector3 direction)
    {
        if (shotsRemaining <= 0)
        {
            // Eliminar este componente cuando se acaben los disparos
            Destroy(this);
            return;
        }
        
        // Crear los dos disparos adicionales
        CreateSideShot(direction, sideAngle);
        CreateSideShot(direction, -sideAngle);
        
        shotsRemaining--;
        Debug.Log("Triple disparo! Quedan: " + shotsRemaining);
    }
    
    private void CreateSideShot(Vector3 direction, float angle)
    {
        // Obtener la posición del mouse en 3D (igual que en PlayerShip.Fire())
        Vector3 mPos = Input.mousePosition;
        mPos.z = -Camera.main.transform.position.z;
        Vector3 mPos3D = Camera.main.ScreenToWorldPoint(mPos);
        
        // Crear una rotación basada en el ángulo alrededor del eje Z
        Quaternion rotation = Quaternion.Euler(0, 0, angle);
        
        // Calcular la nueva posición objetivo rotando la dirección original
        Vector3 rotatedDirection = rotation * direction;
        Vector3 targetPosition = transform.position + rotatedDirection;
        
        // Crear la bala
        GameObject bullet = Instantiate(playerShip.bulletPrefab, transform.position, Quaternion.identity);
        
        // Hacer que la bala mire hacia la dirección rotada
        bullet.transform.LookAt(targetPosition, Vector3.back);
        
        // No necesitamos establecer la velocidad manualmente, ya que el script Bullet lo hace en su método Start
        // basándose en la dirección hacia donde mira la bala (transform.forward)
    }
}