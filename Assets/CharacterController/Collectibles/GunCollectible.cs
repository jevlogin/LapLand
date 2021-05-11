using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunCollectible : Collectible
{
    [SerializeField] private GunCollectibleType _type;
    public GunCollectibleType Type => _type;
}
