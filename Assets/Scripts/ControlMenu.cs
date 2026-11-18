using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlMenu : MonoBehaviour
{
    public void jugar()
    {
        SceneManager.LoadScene("Nivel1");
    }

    public void creditos()
    {
        SceneManager.LoadScene("Creditos");
    }

    public void volverAlMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void GameOver()
    {
        SceneManager.LoadScene("GameOver");
    }

    public void salirDelJuego()
    {
        #if UNITY_EDITOR
        EditorApplication.isPlaying = false;
        #else

        Application.Quit();

        #endif
    }
}
