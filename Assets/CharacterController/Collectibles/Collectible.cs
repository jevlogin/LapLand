using UnityEngine;


public class Collectible : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] protected ItemRoomTypes _roomType;

    public Rigidbody Rigidbody => _rigidbody;
    public ParticleSystem ParticleSystem => _particleSystem;
    public ItemRoomTypes RoomType => _roomType;
    
    public bool IsPickedUp { get; set; }
    public bool IsInstalled { get; set; }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerController playerController))
        {
            playerController.EnteredCollectibleTrigger(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out PlayerController playerController))
        {
            playerController.ExitedCollectibleTrigger(this);
        }
    }
}
