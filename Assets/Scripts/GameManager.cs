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
    public bool isGameOver;
    public bool pausedGame;
    public bool nextLevel;

    public Transform[] prefabLevels;

    private int lives;
    private int score;

    // Start is called before the first frame update
    void Start()
    {

        Debug.Log(string.Format("Check random pref that does not exit{0}", PlayerPrefs.GetFloat("Random")));

        gameLevel = START_LEVEL_FROM;
        lives = MAX_INTIAL_LIVES;
        score = 0;

        nextLevel = false;

        livesText.text = string.Format("Lives: {0}", lives);
        scoreText.text = string.Format("Score: {0}", score);
        levelText.text = string.Format("Level: {0}", gameLevel);

        ball.GetComponent<BallScript>().ResetBall();
        Instantiate(prefabLevels[gameLevel - 1], new Vector2(-4f, 2f), Quaternion.identity);
        numberOfBricks = CountBricks();

    }

    /*
        /// <summary>
        /// Loads the next level, assumns that that next levels exist
        /// </summary>
        /// <param name="gameLevel"></param>
        public void LoadLevel(int level)
        {

    #if UNITY_EDITOR
            Debug.Log(string.Format("LoadLevel with level:{0}", level));
            Debug.Log(string.Format("prefab level {0}: {1}", level-1, prefabLevels[level-2].name));
    #endif

            ball.GetComponent<BallScript>().ResetBall();
            Instantiate(prefabLevels[level - 1], new Vector2(-4f, 2f), Quaternion.identity);
            levelText.text = string.Format("Level: {0}", gameLevel);
            numberOfBricks = CountBricks();
        }
    */

    /// <summary>
    /// Counts the number of Bricks using the "brick" tag
    /// </summary>
    /// <returns>number of bricks</returns>
    private int CountBricks()
    {

        Debug.Log(string.Format("Number of Bricks: {0}", GameObject.FindGameObjectsWithTag("brick").Length));
        return GameObject.FindGameObjectsWithTag("brick").Length;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pausedGame)
            {
                PausedMenuPanel.SetActive(false);
                pausedGame = false;
            }
            else
            {
                pausedGame = true;
                PausedMenuPanel.SetActive(true);
            }
            
        }

    }

    public void RemoveLife()
    {
        if (lives > 1)
        {
            SetLives(lives - 1);
        }
        else if (lives == 1)
        {
            SetLives(0);
            GameOver();
        }
        else // lives < 1
        {
            Debug.LogError("lives should not be less than zero");
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
        isGameOver = true;
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
            pausedGame = true;
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
            nextLevel = true;
        }
    }

    // BUTTON METHODS

    public void OnClickNextLevel()
    {
        DestroyAllObjectsByTag("level-prefab");
        DestroyAllObjectsByTag("brick");
        pausedGame = false;
        LoadLevelPanel.SetActive(false);
        Invoke("LoadLevel", 0.01f);
        //LoadLevel();
    }

    public void OnClickReplayLevel()
    {
        pausedGame = false;
        isGameOver = false;
        LoadLevelPanel.SetActive(false);
        gameLevel--;

        Invoke("LoadLevel", 0.01f);
        //LoadLevel();
    }

    public void OnContGameClick()
    {
        pausedGame = false;
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
