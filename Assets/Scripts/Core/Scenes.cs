using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core
{
    public class Scenes : MonoBehaviour
    {
        public void ChangeScene(int sceneIndx)
        {
            SceneManager.LoadScene(sceneIndx);
        }
    }
}