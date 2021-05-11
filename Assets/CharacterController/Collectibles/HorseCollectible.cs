using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorseCollectible : Collectible
{
    [SerializeField] private HorseCollectibleType _type;

    public HorseCollectibleType Type => _type;
}
