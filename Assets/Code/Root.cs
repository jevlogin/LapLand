using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


namespace JevLogin
{
    internal sealed class Root : MonoBehaviour
    {
        [SerializeField] private Button _buttonStart;


        private void Awake()
        {
            _buttonStart.onClick.AddListener(StartGame);
        }

        private void OnDestroy()
        {
            _buttonStart.onClick.RemoveListener(StartGame);
        }

        private void StartGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}