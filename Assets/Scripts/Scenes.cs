using UnityEngine;
using UnityEngine.SceneManagement;

public class Scenes : MonoBehaviour
{
    public void ChangeScene(int sceneIndx)
    {
        SceneManager.LoadScene(sceneIndx);
    }
}
