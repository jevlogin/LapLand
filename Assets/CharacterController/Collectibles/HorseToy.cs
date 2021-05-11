using System;
using UnityEngine;


public class HorseToy : Toy
{
    [SerializeField] private HorsePartPosition[] _partPositions;
    private int _takenPositions;

    /// <summary>
    /// Used to install a part on a free space on the horse.
    /// </summary>
    /// <param name="collectible">The collectible that you want to install.</param>
    /// <exception cref="Exception">Is thrown if the part's type is wrong.</exception>
    public bool InstallPart(Collectible collectible)
    {
        if (collectible is HorseCollectible horseCollectible)
        {
            
            foreach (var partPosition in _partPositions)
            {
                if (!partPosition.IsTaken)
                {
                    if (partPosition.Type == horseCollectible.Type)
                    {
                        _takenPositions++;
                        var partTransform = horseCollectible.transform;
                        var partPositionTransform = partPosition.transform;
                        partTransform.position = partPositionTransform.position;
                        partTransform.rotation = partPositionTransform.rotation;
                        partTransform.parent = partPositionTransform;

                        horseCollectible.Rigidbody.isKinematic = true;
                        horseCollectible.ParticleSystem.Stop();
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
                $"The horse toy needs a {nameof(HorseCollectible)}. {collectible}'s type is {collectible.GetType()}");
        }
    }
}