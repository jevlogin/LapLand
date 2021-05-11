using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarCollectible : Collectible
{
    [SerializeField] private CarCollectibleType _type;

    public CarCollectibleType Type => _type;
}
