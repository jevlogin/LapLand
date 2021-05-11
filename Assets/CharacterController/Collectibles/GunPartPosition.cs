using UnityEngine;


public class GunPartPosition : PartPosition
{
    [SerializeField] private GunCollectibleType _type;

    public GunCollectibleType Type => _type;
}