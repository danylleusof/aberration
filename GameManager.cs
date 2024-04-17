using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region Singleton;

    public static GameManager instance;

    private void Awake()
    {
        instance = this;
    }

    #endregion

    [Header("References")]
    public GameObject player;

    public int killsToWin;

    [HideInInspector]
    public int enemiesKilled;
    
    // Start is called before the first frame update
    void Start()
    {
        enemiesKilled = 0;
    }

    // Update is called once per frame
    void Update()
    { 
        if (enemiesKilled == killsToWin)
            Invoke("Win", 1f);
    }

    void Win() => SceneManager.LoadScene(3);
}
