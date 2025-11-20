using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    
    [Header("Health Bar")]
    public Image healthBarFill;
    public Text healthText;
    
    [Header("Other UI")]
    public Text levelText;
    public Text xpText;
    public Text gemsText;
    [Header("Game Screens")]
public GameObject victoryScreen;
public GameObject gameOverScreen;
    void Awake()
    {
        // Singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        if (healthBarFill != null)
        {
            healthBarFill.fillAmount = currentHealth / maxHealth;
        }
        
        if (healthText != null)
        {
            healthText.text = Mathf.Round(currentHealth) + " / " + Mathf.Round(maxHealth);
        }
    }
    
    public void UpdateLevel(int level)
    {
        if (levelText != null)
        {
            levelText.text = "Level: " + level;
        }
    }
    
    public void UpdateXP(int currentXP, int xpNeeded)
    {
        if (xpText != null)
        {
            xpText.text = "XP: " + currentXP + " / " + xpNeeded;
        }
    }
    
    public void UpdateGems(int gems)
    {
        if (gemsText != null)
        {
            gemsText.text = "Gems: " + gems;
        }
    }

    public void UpdateXPBar(int currentXP, int maxXP)
{
    // Update XP bar fill
    Image xpBarFill = GameObject.Find("XPBarFill")?.GetComponent<Image>();
    if (xpBarFill != null)
    {
        xpBarFill.fillAmount = (float)currentXP / maxXP;
    }
    
    // Update XP text
    if (xpText != null)
    {
        xpText.text = currentXP + " / " + maxXP;
    }
}
public void ShowVictoryScreen(int enemiesKilled, int gems, int level)
{
    if (victoryScreen != null)
    {
        victoryScreen.SetActive(true);
        
        // Update stats text
        Text statsText = victoryScreen.transform.Find("VictoryStatsText")?.GetComponent<Text>();
        if (statsText != null)
        {
            statsText.text = "Enemies Defeated: " + enemiesKilled + "\n" +
                           "Gems Collected: " + gems + "\n" +
                           "Final Level: " + level;
        }
        
        // Pause game
        Time.timeScale = 0f;
    }
}

public void ShowGameOverScreen()
{
    if (gameOverScreen != null)
    {
        gameOverScreen.SetActive(true);
        
        // Pause game
        Time.timeScale = 0f;
    }
}

public void RestartGame()
{
    // Unpause
    Time.timeScale = 1f;
    
    // Reload scene
    UnityEngine.SceneManagement.SceneManager.LoadScene(
        UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
    );
}
}