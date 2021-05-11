using System;
using UnityEngine;


public class CarToy : Toy
{
    [SerializeField] private CarPartPosition[] _partPositions;
    private int _takenPositions;

    public bool InstallPart(Collectible collectible)
    {
        if (collectible is CarCollectible carCollectible)
        {
            foreach (var partPosition in _partPositions)
            {
                if (!partPosition.IsTaken)
                {
                    if (partPosition.Type == carCollectible.Type)
                    {
                        _takenPositions++;
                        var partTransform = carCollectible.transform;
                        var partPositionTransform = partPosition.transform;
                        partTransform.position = partPositionTransform.position;
                        partTransform.rotation = partPositionTransform.rotation;
                        partTransform.parent = partPositionTransform;

                        carCollectible.Rigidbody.isKinematic = true;
                        carCollectible.ParticleSystem.Stop();
                        partPosition.IsTaken = true;
                        if (_takenPositions == _partPositions.Length)
                        {
                            OnAllPartsCollected();
                        }

                        _audioSource.Play();
                        return true;
                    }
                }
            }

            print("This part isn't needed.");
            return false;
        }
        else
        {
            throw new Exception(
                $"The horse toy needs a {nameof(CarCollectible)}. {collectible}'s type is {collectible.GetType()}");
        }
    }
}