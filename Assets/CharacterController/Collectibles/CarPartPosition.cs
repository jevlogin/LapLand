using UnityEngine;


public class CarPartPosition : PartPosition
{
    [SerializeField] private CarCollectibleType _type;

    public CarCollectibleType Type => _type;
}