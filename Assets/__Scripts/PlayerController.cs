using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : MonoBehaviour
{
    public float velocitatNau = 2.5f; // Velocitat de la nau
    public float velocitatBala = 10f; // Velocitat de la bala
    private Rigidbody cosRigid; // Referència al Rigidbody de l'objecte
    public GameObject prefabBala; // Prefab de la bala
    public Transform puntDispar; // Punt de dispar (on s'instanciarà la bala)
    public Transform turret; // Referència al cilindre que ha de girar

    void Start()
    {
        // Obtenir la referència al Rigidbody
        cosRigid = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Obtenir les entrades de moviment horitzontal i vertical
        float horitzontal = CrossPlatformInputManager.GetAxis("Horizontal");
        float vertical = CrossPlatformInputManager.GetAxis("Vertical");

        // Crear un vector per a la direcció del moviment
        Vector2 direccioMoviment = new Vector2(horitzontal, vertical);

        // Aplicar la velocitat al Rigidbody per moure l'objecte
        cosRigid.velocity = direccioMoviment * velocitatNau;

        // Detectar si el jugador prem el botó de dispar
        if (CrossPlatformInputManager.GetButtonDown("Fire1"))
        {
            Disparar();
        }

        // Control de la rotació de la torre cap al ratolí
        GirarTurretCapRatoli();
    }

    void Disparar()
    {
        Debug.Log("Dispar realitzat");

        // Instanciar la bala en el punt de dispar
        GameObject bala = Instantiate(prefabBala, puntDispar.position, puntDispar.rotation);

        // Obtenir el Rigidbody de la bala i donar-li velocitat en la direcció en què està mirant la torre
        Rigidbody cosRigidBala = bala.GetComponent<Rigidbody>();
        cosRigidBala.velocity = puntDispar.up * velocitatBala; // Ajusta la velocitat segons sigui necessari
    }

    void GirarTurretCapRatoli()
    {
        // Obtenir la posició del ratolí en l'espai de la càmera
        Vector3 posRatoli = Input.mousePosition;
        posRatoli.z = 10f; // Ajustar distància depenent de la càmera

        // Convertir la posició del ratolí de la pantalla a coordenades del món
        Vector3 posicioMonRatoli = Camera.main.ScreenToWorldPoint(posRatoli);

        // Calcular la direcció des de la torre cap al ratolí
        Vector3 direccio = posicioMonRatoli - turret.position;
        direccio.z = 0; // Mantenir la rotació en 2D

        // Calcular la rotació en el pla 2D
        float angle = Mathf.Atan2(direccio.y, direccio.x) * Mathf.Rad2Deg;
        turret.rotation = Quaternion.Euler(-angle, 90, -90);
    }
}
