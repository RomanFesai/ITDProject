using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameBehaviour : MonoBehaviour, IManager
{
    public string labelText = "Collect all 4 items and win your freedom!";
    public int maxItems = 4;
    private int _itemsCollected = 0;
    public bool showWinScreen = false;
    public bool isGameOver = false;
    private int _playerLives = 10;
    private int currentHealth;
    private string _state;
    private GUIStyle guiStyle = new GUIStyle();
    public bool pause = false;
    public Camera fpsCam;
    public GameObject LossScreen;
    public HealthBar healthBar;
    public Text KeysCollected;
    public MouseLook PlayerCam;

    /*private bool mFaded = false;
    public float Duration = 0.4f;*/

    public string State
    {
        get { return _state; }
        set { _state = value; }
    }
    public int Items
    {
        get
        {
            return _itemsCollected;
        }
        set
        {
            _itemsCollected = value;
            KeysCollected.text = _itemsCollected.ToString("0");
            Debug.LogFormat("Items: {0}", _itemsCollected);
            if (_itemsCollected >= maxItems)
            {
                labelText = "You've found all the items!";
                
            }
            else
            {
                labelText = "You've found, only " + (maxItems - _itemsCollected) + " more to go!";
            }
        }
    } 

    public int Lives
    {
        get
        {
            return currentHealth;
        }
        set
        {
            currentHealth = value;
            healthBar.SetHealth(currentHealth);
            Debug.LogFormat("Lives: {0}", currentHealth);
            if (currentHealth <= 0)
            {
                UnityEngine.Cursor.visible = true;
                ShowLossScreen();
            }
        }
    }
    void RestartLevel()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1.0f;
    }

    void Start()
    {
        Initialize();
        currentHealth = _playerLives;
        healthBar.SetMaxHealth(_playerLives);
        UnityEngine.Cursor.visible = false;
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        PlayerCam = GameObject.Find("Main Camera").GetComponent<MouseLook>();
    }

    public void Initialize()
    {
        _state = "Manager initialized..";
        Debug.Log(_state);
    }

   /* public void Fade()
    {
        var canvGroup = GameObject.Find("GameOver").GetComponent<CanvasGroup>();

        StartCoroutine(DoFade(canvGroup, canvGroup.alpha, mFaded ? 1 : 0));

        mFaded = !mFaded;
    }

    public IEnumerator DoFade(CanvasGroup canvGroup, float start, float end) 
    {
        float counter = 0f;

        while(counter < Duration)
        {
            counter += Time.deltaTime;
            canvGroup.alpha = Mathf.Lerp(start,end,counter/Duration);

            yield return null;
        }
    }*/

    void OnGUI()
    {
        guiStyle.fontSize = 25;
        guiStyle.normal.textColor = Color.white;
        GUI.contentColor = Color.white;
        GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height - 50, 600, 700), labelText, guiStyle);
        if (showWinScreen)
        {
            UnityEngine.Cursor.visible = true;
            UnityEngine.Cursor.lockState = CursorLockMode.None;
            if (GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 50, 200, 100), "YOU ESCAPED!"))
            {                
                Utilities.RestartLevel(0);
            }            
        }
    }

    public void ShowLossScreen() 
    {
        fpsCam.transform.rotation = new Quaternion(0, 0, 90, -90);
        GameObject.Find("Sword_0").GetComponent<SpriteRenderer>().enabled = false;
        Time.timeScale = 0;
        LossScreen.SetActive(true);
        isGameOver = true;
        UnityEngine.Cursor.lockState = CursorLockMode.Confined;
        PlayerCam.enabled = false;
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Menu");
        isGameOver = false;
    }

    public void Retry()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("SampleScene");
        isGameOver = false;
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (_itemsCollected >= maxItems)
        {
            showWinScreen = true;
            Time.timeScale = 0f;
        }
    }
}
