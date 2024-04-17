using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void Play() => SceneManager.LoadScene(1);

    public void Quit() => Application.Quit();

    public void Menu() => SceneManager.LoadScene(0);
}
