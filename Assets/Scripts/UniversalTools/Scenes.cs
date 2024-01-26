using UnityEngine;
using UnityEngine.SceneManagement;

namespace UniversalTools
{
    public class Scenes : MonoBehaviour
    {
        public void ChangeScene(int sceneIndx)
        {
            SceneManager.LoadScene(sceneIndx);
        }
    }
}