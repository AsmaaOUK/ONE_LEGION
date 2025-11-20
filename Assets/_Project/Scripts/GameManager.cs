using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    [Header("Game Stats")]
    public int totalGems = 0;
    public int enemiesKilled = 0;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void AddGems(int amount)
    {
        totalGems += amount;
        
        // Update UI
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateGems(totalGems);
        }
        
        Debug.Log("💎 Total Gems: " + totalGems);
    }
    
    public void EnemyKilled()
    {
        enemiesKilled++;
        Debug.Log("💀 Enemies killed: " + enemiesKilled);
    }
}