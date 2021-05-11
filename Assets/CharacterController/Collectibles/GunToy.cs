using System;
using UnityEngine;


public class GunToy : Toy
{
    [SerializeField] private GunPartPosition[] _partPositions;
    private int _takenPositions;

    public bool InstallPart(Collectible collectible)
    {
        if (collectible is GunCollectible gunCollectible)
        {
            foreach (var partPosition in _partPositions)
            {
                if (!partPosition.IsTaken)
                {
                    if (partPosition.Type == gunCollectible.Type)
                    {
                        _takenPositions++;
                        var partTransform = gunCollectible.transform;
                        var partPositionTransform = partPosition.transform;
                        partTransform.position = partPositionTransform.position;
                        partTransform.rotation = partPositionTransform.rotation;
                        partTransform.parent = partPositionTransform;

                        gunCollectible.Rigidbody.isKinematic = true;
                        gunCollectible.ParticleSystem.Stop();
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
                $"The horse toy needs a {nameof(GunCollectible)}. {collectible}'s type is {collectible.GetType()}");
        }
    }
}