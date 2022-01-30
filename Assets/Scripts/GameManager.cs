using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int MAX_INTIAL_LIVES = 3;
    public int START_LEVEL_FROM = 1;
    public int gameLevel;
    public int numberOfBricks;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI livesText;
    public TextMeshProUGUI levelText;
    public Transform ball;
    public GameObject menuPanel;
    public GameObject LoadLevelPanel;
    public GameObject PausedMenuPanel;
    public GameState gameState;

    public Transform[] prefabLevels;

    private int lives;
    private int score;

    // Start is called before the first frame update
    void Start()
    {

        Debug.Log(string.Format("Check random pref that does not exit{0}", PlayerPrefs.GetFloat("Random")));

        gameState = GameState.firstTime;

        gameLevel = START_LEVEL_FROM;
        lives = MAX_INTIAL_LIVES;
        score = 0;


        livesText.text = string.Format("Lives: {0}", lives);
        scoreText.text = string.Format("Score: {0}", score);
        levelText.text = string.Format("Level: {0}", gameLevel);

        ball.GetComponent<BallScript>().ResetBall();
        Instantiate(prefabLevels[gameLevel - 1], new Vector2(-4f, 2f), Quaternion.identity);
        numberOfBricks = CountBricks();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Escaped Pressed");
            switch (gameState)
            {
                case GameState.paused:
                    PausedMenuPanel.SetActive(false);
                    gameState = GameState.playing;
                    break;
                default:
                    gameState = GameState.paused;
                    PausedMenuPanel.SetActive(true);
                    break;
            }
        }
    }


    /// <summary>
    /// Counts the number of Bricks using the "brick" tag
    /// </summary>
    /// <returns>number of bricks</returns>
    private int CountBricks()
    {
        var count = GameObject.FindGameObjectsWithTag("brick").Length;
        Debug.Log(string.Format("Number of Bricks: {0}", count));
        return count;
    }

    public void RemoveLife() { 

        if (lives  < 1){
            Debug.LogError("lives should not be less than zero");
        }
        switch (lives)
        {
            case 1:
                SetLives(0);
                GameOver();
                break;
            default:
                SetLives(lives - 1);
                break;
        }
    }

    /// <summary>
    /// Adds a single life to the number of lives
    /// </summary>
    internal void addOneLife()
    {
        SetLives(lives + 1);
    }

    /// <summary>
    /// Sets the nember of lives a player has
    /// </summary>
    /// <param name="lives">the number of lives a Playe has in the game</param>
    private void SetLives(int lives)
    {
        this.lives = lives;
        livesText.text = string.Format("Lives: {0}", lives);
    }

    /// <summary>
    /// Called when a game has finished
    /// </summary>
    private void GameOver()
    {
        gameState = GameState.ended;
        menuPanel.SetActive(true);
        DestroyAllPowerUps();
    }

    /// <summary>
    /// Destroys all the power up that are existing
    /// </summary>
    internal void DestroyAllPowerUps()
    {
        // Destoying any powerups
        DestroyAllObjectsByTag("extraLife");
    }


    internal void DestroyAllObjectsByTag(string tag)
    {
        GameObject[] objects= GameObject.FindGameObjectsWithTag(tag);
        foreach (GameObject obj in objects)
        {
            Destroy(obj);
        }
    }

    internal void AddScore(int brickScore)
    {
        score += brickScore;
        scoreText.text = string.Format("Score: {0}", score);
    }

    internal void NextLevel()
    {
        gameLevel++;
        if (gameLevel > prefabLevels.Length)
        {
            GameOver();
        }
        else
        {
            LoadLevelPanel.SetActive(true);
            gameState = GameState.paused;
            LoadLevel();
            //Invoke("LoadLevel", 10f);
        }
    }

    internal void LoadLevel()
    {
        ball.GetComponent<BallScript>().ResetBall();
        Instantiate(prefabLevels[gameLevel - 1], new Vector2(-4f, 2f), Quaternion.identity);
        levelText.text = string.Format("Level: {0}", gameLevel);
        numberOfBricks = CountBricks();  // This is a dirty fix
    }

    internal void RemoveOneBricks()
    {
        numberOfBricks--;
        CountBricks();
        if (numberOfBricks <= 0)
        {
            gameState = GameState.nextLevel;
        }
    }

    // BUTTON METHODS

    public void OnClickNextLevel()
    {
        DestroyAllObjectsByTag("level-prefab");
        DestroyAllObjectsByTag("brick");
        gameState = GameState.paused;
        LoadLevelPanel.SetActive(false);
        Invoke("LoadLevel", 0.01f);
        //LoadLevel();
    }

    public void OnClickReplayLevel()
    {
        gameState = GameState.playing;
        LoadLevelPanel.SetActive(false);
        gameLevel--;

        Invoke("LoadLevel", 0.01f);
        //LoadLevel();
    }

    public void OnContGameClick()
    {
        gameState = GameState.playing;
        PausedMenuPanel.SetActive(false);
    }


    // NEW GAME AND QUIT GAME

    public void NewGame()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quitting");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("StartMenu");
    }


}

public enum GameState
{
    firstTime, playing, paused, ended, nextLevel
}
