using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCardSpot : GameBehaviour<EnemyCardSpot>
{
    public bool isOccupied;
    void Start()
    {
        isOccupied = false;
    }
}
