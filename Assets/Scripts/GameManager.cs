using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("UI References")]
    public Animator animator;
    public TextMeshProUGUI gameStartText;
    public TextMeshProUGUI gameOverText;
    public Button retryButton;
    public TextMeshProUGUI coinText;

    [Header("Gameplay Settings")]
    public float initialGameSpeed = 5f;
    public float gameSpeedIncrease = 2f;
    public float gameSpeed { get; private set; }

    [Header("Coin System")]
    public int coinCount = 0;
    public int coinsToBoss = 3; // syarat coin pindah boss

    private PlayerController2D player;
    private ObstacleGenerator spawner;

    public bool isGameStarted = false;
    public bool isGameOver = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else DestroyImmediate(gameObject);
    }

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        player = FindObjectOfType<PlayerController2D>();
        spawner = FindObjectOfType<ObstacleGenerator>();

        animator.SetBool("isGameStarted", false);
        gameSpeedIncrease = 0;
        if (spawner != null) spawner.gameObject.SetActive(false);

        // reset coin
        coinCount = 0;
        if (coinText != null) coinText.text = "Coin: 0";
    }

    public void NewGame()
    {
        gameSpeedIncrease = 0.1f;
        player.gameObject.SetActive(false);

        ObstacleController[] obstacles = FindObjectsOfType<ObstacleController>();
        foreach (var obstacle in obstacles)
        {
            Destroy(obstacle.gameObject);
        }

        gameSpeed = initialGameSpeed;
        enabled = true;

        player.gameObject.SetActive(true);
        if (spawner != null) spawner.gameObject.SetActive(true);

        gameOverText.gameObject.SetActive(false);
        retryButton.gameObject.SetActive(false);

        isGameOver = false;
        animator.SetBool("isGameStarted", true);

        // reset coin
        coinCount = 0;
        if (coinText != null) coinText.text = "Coin: 0";

        // reset boss health kalau ada boss
        BossHealth boss = FindObjectOfType<BossHealth>();
        if (boss != null) boss.ResetHealth();
    }

    public void GameStart()
    {
        gameOverText.gameObject.SetActive(false);
        retryButton.gameObject.SetActive(false);

        if (Input.GetKeyDown(KeyCode.Space) && !isGameStarted)
        {
            isGameStarted = true;
            NewGame();
            gameStartText.gameObject.SetActive(false);
            animator.SetBool("isGameStarted", true);
        }
    }

    private void Update()
    {
        if (!isGameOver)
        {
            GameStart();

            // 🔹 update game speed tiap frame
            gameSpeed += gameSpeedIncrease * Time.deltaTime;

            // 🔹 Scene Boss → cek boss mati
            if (SceneManager.GetActiveScene().name == "boss")
            {
                GameObject boss = GameObject.FindWithTag("Boss");
                if (boss == null) SceneManager.LoadScene("play");
            }
        }
    }

    public void GameOver()
    {
        gameSpeed = 0f;
        enabled = false;

        animator.SetBool("isGameOver", true);
        if (spawner != null) spawner.gameObject.SetActive(false);

        gameOverText.gameObject.SetActive(true);
        retryButton.gameObject.SetActive(true);

        isGameOver = true;

        // reset boss health kalau ada
        BossHealth boss = FindObjectOfType<BossHealth>();
        if (boss != null) boss.ResetHealth();
    }

    // --- Coin ---
    public void AddCoin(int amount)
    {
        coinCount += amount;
        if (coinText != null) coinText.text = "Coin: " + coinCount;

        if (coinCount >= coinsToBoss && SceneManager.GetActiveScene().name != "boss")
        {
            SceneManager.LoadScene("boss");
        }
    }
}
