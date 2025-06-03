using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [Header("Set in Inspector")]
    public float lifetime = 5f; // Temps de vida del power-up
    
    protected float birthTime;
    
    protected virtual void Awake()
    {
        birthTime = Time.time;
        
        // Assegurar-se que el collider és prou gran i allargat cap endavant
        BoxCollider boxCollider = GetComponent<BoxCollider>();
        if (boxCollider != null)
        {
            // Fer el collider més gran i allargat cap endavant
            boxCollider.size = new Vector3(2f, 2f, 4f);
            boxCollider.center = new Vector3(0f, 0f, 2f); // Moure el centre cap endavant
        }
        else
        {
            Debug.LogWarning("No s'ha trobat un BoxCollider al PowerUp!");
        }
    }
    
    protected virtual void Update()
    {
        // Comprovar si ha passat el temps de vida
        if (Time.time - birthTime >= lifetime)
        {
            Destroy(gameObject);
        }
        
        // Comprovar si el jugador està a prop (depuració)
        if (PlayerShip.S != null)
        {
            float distancia = Vector3.Distance(transform.position, PlayerShip.S.transform.position);
            if (distancia < 5.0f)
            {                
                // Si el jugador està molt a prop, forçar la recollida
                if (distancia < 0.5f)
                {
                    ApplyPowerUp();
                    Destroy(gameObject);
                }
            }
        }
    }
    
    protected virtual void OnTriggerEnter(Collider other)
    {       
        // Comprovar si ha col·lisionat amb el jugador
        if (other.gameObject.tag == "Player")
        {
            ApplyPowerUp();
            Destroy(gameObject);
        }
    }
    
    protected virtual void ApplyPowerUp()
    {
        // Aquest mètode serà sobreescrit per les classes derivades
        Debug.Log("Mètode ApplyPowerUp cridat");
    }
}
