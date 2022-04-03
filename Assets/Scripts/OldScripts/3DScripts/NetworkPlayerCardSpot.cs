using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkPlayerCardSpot : GameBehaviour<NetworkPlayerCardSpot>
{
    public bool isOccupied;
    void Start()
    {
        isOccupied = false;
    }
}
