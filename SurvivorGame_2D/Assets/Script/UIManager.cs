using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using EasyJoystick;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public TMP_Text enemyDead;
    public int enemyDeadCount = 0;

    public TMP_Text level;
    public int levelCount = 1;

    [SerializeField] TMP_Text timerText;
    [SerializeField] float remainingTime;

    public Joystick joystick;

    [SerializeField] private UpgradePopup upgradePopup;
    [SerializeField] private GameObject gameOver;
    [SerializeField] private GameObject menu;
    [SerializeField] private Slider expSlider;
    private void Awake()
    {
        gameOver.SetActive(false);
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        
    }
    public void Update()
    {
        enemyDead.text = enemyDeadCount.ToString();
        level.text = "Lv." + levelCount;
        Timer();


    }
    public void Timer()
    {
        if (remainingTime <= 0)
        {
            remainingTime = 0;
            GameOverPopup();
            timerText.color = Color.red;
        }
        else if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
        }
        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int second = Mathf.FloorToInt(remainingTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, second);


    }
    public void OpenUpgradePopup()
    {
        upgradePopup.gameObject.SetActive(true);
        upgradePopup.SetupPopup();
        Time.timeScale = 0;
    }
    public void GameOverPopup()
    {
        gameOver.SetActive(true);
        Time.timeScale = 0;
    }
    public void UpdateXP(float expPercent)
    {
        expSlider.value = expPercent;
    }
    public void Menu()
    {
        menu.SetActive(true);
        Time.timeScale = 0; // Pause Time
    }
    public void Resume()
    {
        menu.SetActive(false);
        Time.timeScale = 1.0f;
    }
    public void TryAgain()
    {
        gameOver.SetActive(false);
        menu.SetActive(false);
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(1);
        remainingTime = 60;
        timerText.color = Color.white;
        expSlider.value=0;
        levelCount = 1;
        enemyDeadCount = 0;
    }
}
