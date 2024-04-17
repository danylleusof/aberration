using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using TMPro;

public class GameInterface : MonoBehaviour
{
    [Header("References")]
    public GameObject player;
    FirstPersonCamera firstPersonCamera;
    PlayerController playerController;
    Weapon weapon;

    [Header("Settings Menu")]
    public AudioMixer audioMixer;
    public TMPro.TMP_Dropdown resolutionDropdown;
    Resolution[] resolutions;
    public static bool GameIsPaused = false;

    [Header("Pause and Objective Menu")]
    public GameObject pauseMenu;
    public GameObject objective;

    [Header("Ammo and Magazine Display")]
    public TextMeshProUGUI ammoDisplay;
    public WeaponData weaponData;

    [Header("Mouse Sensitivity Settings")]
    public Slider sensitivitySlider;
    float currentSensitivity;
    public float maxSensitivity = 5f, minSensitivity = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
                currentResolutionIndex = i;
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        firstPersonCamera = player.GetComponentInChildren<FirstPersonCamera>();
        playerController = player.GetComponent<PlayerController>();
        weapon = player.GetComponentInChildren<Weapon>();

        Resume();

        currentSensitivity = firstPersonCamera.mouseSensitivity;
        sensitivitySlider.maxValue = maxSensitivity;
        sensitivitySlider.minValue = minSensitivity;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            if (GameIsPaused)
            {
                pauseMenu.SetActive(false);
                Resume();
            }

            else
            {
                pauseMenu.SetActive(true);
                Pause();
            }

        if (Input.GetKeyDown(KeyCode.Tab))
            if (GameIsPaused)
            {
                objective.SetActive(false);
                Resume();
            }
            else
            {
                objective.SetActive(true);
                Pause();
            }

        ammoDisplay.text = weaponData.currentAmmo.ToString();

        sensitivitySlider.value = currentSensitivity;
        firstPersonCamera.mouseSensitivity = currentSensitivity;
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        GameIsPaused = false;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        firstPersonCamera.enabled = true;
        playerController.enabled = true;
        weapon.enabled = true;
    }

    void Pause()
    {
        Time.timeScale = 0f;
        GameIsPaused = true;

        AudioManager.instance.PlaySFX("Paper");
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        firstPersonCamera.enabled = false;
        playerController.enabled = false;
        weapon.enabled = false;
    }

    public void ChangeSensitivity() => currentSensitivity = sensitivitySlider.value;

    public void Restart() => SceneManager.LoadScene(1);

    public void MainMenu() => SceneManager.LoadScene(0);

    public void Quit() => Application.Quit();
}
