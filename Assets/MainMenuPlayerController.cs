using UnityEngine;
using Random = UnityEngine.Random;


public class MainMenuPlayerController : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    private void Start()
    {
        int danceNum = Random.Range(0, 4);

        switch (danceNum)
        {
            case 0:
                _animator.SetBool("BreakDance", true);
                break;
            case 1:
                _animator.SetBool("ShuffleDance", true);
                break;
            case 2:
                _animator.SetBool("HipHopDance", true);
                break;
            case 3:
                _animator.SetBool("RumbaDance", true);
                break;
        }
    }
}
