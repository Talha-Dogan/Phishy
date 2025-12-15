using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public void Start()
    {

    }

    public void loadLevel(string levelName)
    {
        Debug.Log("Clicked " + levelName);
        SceneManager.LoadScene(levelName, LoadSceneMode.Single);
    }
}