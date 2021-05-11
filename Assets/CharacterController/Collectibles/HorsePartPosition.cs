using UnityEngine;


public class HorsePartPosition : PartPosition
{
    [SerializeField] private HorseCollectibleType _type;

    public HorseCollectibleType Type => _type;
}