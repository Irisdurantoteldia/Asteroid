using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody rb; // Referència al Rigidbody2D de la bala
    private Vector2 direction; // Angle de la direcció de la bala

    // S'executa en iniciar el joc
    void Start()
    {
        // Destrueix la bala després d'un temps (per exemple, 2 segons)
        Invoke("DestroyBullet", 2f);
    }

    // Mètode per destruir la bala
    public void DestroyBullet()
    {
        Destroy(gameObject);
    }
}