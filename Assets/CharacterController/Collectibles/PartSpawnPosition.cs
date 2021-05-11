using UnityEngine;


public class PartSpawnPosition : MonoBehaviour
{
    [SerializeField] private ItemRoomTypes _roomType;

    public ItemRoomTypes RoomType => _roomType;
    
    public bool IsTaken { get; set; }
}