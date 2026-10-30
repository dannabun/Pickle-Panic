using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject pickleballPrefab;
    public GameObject fireBallPrefab;
    public GameObject bombBallPrefab;
    public GameObject gameOverPanel;
    public GameObject startPanel;
    public GameObject lifeIcon1;
    public GameObject lifeIcon2;
    public GameObject lifeIcon3;

    public TMP_Text scoreText;
    public TMP_Text livesText;
    public TMP_Text levelText;
    public TMP_Text gameOverText;
    public TMP_Text restartText;
    public TMP_Text finalScoreText;

    public float timeBetweenBalls = 1.5f;

    public int score = 0;
    public int lives = 3;
    public int level = 1;
    public bool gameOver = false;
    public TMP_Text highScoreText;

    private float timer = 0f;
    private int highScore;

    public AudioClip pickleballHitSound;
    public AudioClip fireballHitSound;
    public AudioClip levelUpSound;
    public AudioClip lifeLostSound;
    public AudioClip gameOverSound;
    private AudioSource audioSource;
    private int nextLevelScore = 1000;

    void Start()
    {
        Time.timeScale = 0f;
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        audioSource = GetComponent<AudioSource>();

        UpdateText();
        
        gameOverText.gameObject.SetActive(false);
        restartText.gameObject.SetActive(false);
        gameOverPanel.SetActive(false);
        levelText.text = "Level: 1";
        finalScoreText.gameObject.SetActive(false);
    }

    public void StartGame()
    {
        startPanel.SetActive(false);
        Time.timeScale = 1f;
        SpawnBall();
    }

        void UpdateLifeIcons()
    {
        if (lifeIcon1 != null)
        {
            lifeIcon1.SetActive(lives >= 1);
        }

        if (lifeIcon2 != null)
        {
            lifeIcon2.SetActive(lives >= 2);
        }

        if (lifeIcon3 != null)
        {
            lifeIcon3.SetActive(lives >= 3);
        }
    }

    void Update()
    {
        if (gameOver)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }

            return;
        }

        if (FindAnyObjectByType<BallMovement>() == null)
        {
            timer += Time.deltaTime;

            if (timer >= timeBetweenBalls)
            {
                SpawnBall();
                timer = 0f;
            }
        }
    }

    void SpawnBall()
    {
        float chance = Random.value;
        GameObject ballToSpawn = pickleballPrefab;

        if (chance < 0.20f)
        {
            ballToSpawn = fireBallPrefab;
        }
        else if (chance < 0.35f)
        {
            ballToSpawn = bombBallPrefab;
        }

        Instantiate(ballToSpawn);
    }

    public void AddScore(int points)
    {
        score += points;

        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();
        }

        if (score >= nextLevelScore)
        {
            level++;
            levelText.text = "Level: " + level;

            PlayGameSound(levelUpSound);
            nextLevelScore += 1000;
        }

        UpdateText();
    }

    public void LoseLife()
    {
        if (gameOver)
        {
            return;
        }

        PlayGameSound(lifeLostSound);

        lives = Mathf.Max(lives - 1, 0);
        UpdateText();

        if (lives == 0)
        {
            gameOver = true;
            Time.timeScale = 0f;

            gameOverPanel.SetActive(true);
            gameOverText.gameObject.SetActive(true);
            restartText.gameObject.SetActive(true);

            finalScoreText.text = "Score: " + score;
            finalScoreText.gameObject.SetActive(true);

            highScoreText.text = "High Score: " + highScore;
            highScoreText.gameObject.SetActive(true);

            PlayGameSound(gameOverSound);
        }
    }

    public void PlayGameSound(AudioClip sound)
    {
        if (audioSource != null && sound != null)
        {
            audioSource.PlayOneShot(sound);
        }
    }

    void UpdateText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }

        if (livesText != null)
        {
            livesText.text = "Lives:";
            UpdateLifeIcons();
        }
    }
}