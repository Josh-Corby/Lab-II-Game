using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCardSpot : GameBehaviour<PlayerCardSpot>
{
    public bool isOccupied;
    void Start()
    {
        isOccupied = false;
    }
}
