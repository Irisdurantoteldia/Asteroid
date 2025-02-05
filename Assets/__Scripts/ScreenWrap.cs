using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Math = System.Math;

public class ScreenWrap : MonoBehaviour
{
    private Camera camPrincipal; // Càmera principal
    private ScreenBounds limitsPantalla; // Límits de la pantalla

    void Start()
    {
        // Obtenir la referència a la càmera principal
        camPrincipal = Camera.main;
        limitsPantalla = GameObject.FindGameObjectWithTag("ScreenBound").GetComponent<ScreenBounds>();
        // Obtenir l'amplada i l'altura de la pantalla en unitats del món
    }

    void Update()
    {
        
    }

    void OnTriggerExit(Collider altre)
    {
        // InverseTransformPoint(transform.position); Relativa
        // TransformPoint(transform.position); Posició global
        Vector3 pos = limitsPantalla.transform.InverseTransformPoint(transform.position);
        if (Math.Abs(pos.x) > 0.5)
        {
            pos.x *= -1;
        }
        if (Math.Abs(pos.y) > 0.5)
        {
            pos.y *= -1;
        }
        
        gameObject.transform.position = limitsPantalla.transform.TransformPoint(pos);
    }
}
