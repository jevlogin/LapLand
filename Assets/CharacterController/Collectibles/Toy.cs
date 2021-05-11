using UnityEngine;


public class Toy : MonoBehaviour
{
    [SerializeField] protected AudioSource _audioSource;
    private GameController _gameController;
    
    private void Start()
    {
        _gameController = FindObjectOfType<GameController>();
    }

    protected void OnAllPartsCollected()
    {
        _gameController.CollectedAllParts();
    }
    
    protected void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerController playerController))
        {
            playerController.EnteredToyTrigger();
        }
    }

    protected void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out PlayerController playerController))
        {
            playerController.ExitedToyTrigger();
        }
    }
}
