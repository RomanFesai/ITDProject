using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject PauseWindow;
    public GameBehaviour IsLoss;
    public MouseLook PlayerCam;

    void Start()
    {
        PlayerCam = GameObject.Find("Main Camera").GetComponent<MouseLook>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }else if (IsLoss.isGameOver) { }
            else
            { Pause(); }
        }
    }
    public void ReturnToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Menu");
    }

    public void Resume()
    {
        PlayerCam.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
        GameIsPaused = false;
        Cursor.visible = false;
        PauseWindow.SetActive(false);
    }

    public void Pause()
    {
        PlayerCam.enabled=false;
        Cursor.lockState = CursorLockMode.Confined;
        Time.timeScale = 0;
        GameIsPaused = true;
        Cursor.visible = true;
        PauseWindow.SetActive(true);
    }
}
