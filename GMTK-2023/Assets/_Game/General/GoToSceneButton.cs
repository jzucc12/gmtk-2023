using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToSceneButton : MonoBehaviour
{
    public void GoToScene(int index)
    {  
        SceneManager.LoadScene(index);
    }
}