using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraLifePowerUp : PowerUp
{
    protected override void ApplyPowerUp()
    {
        // Incrementar el nombre de vides del jugador
        PlayerShip.JUMPS++;
        Debug.Log("Vida extra obtinguda! Vides restants: " + PlayerShip.JUMPS);
    }
}