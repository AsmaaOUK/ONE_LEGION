using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Player Stats")]
    public float maxHealth = 100f;
    public float currentHealth;
    
    [Header("Level System")]
    public int currentLevel = 1;
    public int currentXP = 0;
    public int xpToNextLevel = 100;
    
    void Start()
    {
        currentHealth = maxHealth;
        
        // Update UI at start
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateHealthBar(currentHealth, maxHealth);
        UIManager.Instance.UpdateLevel(currentLevel);  
        UIManager.Instance.UpdateXPBar(currentXP, xpToNextLevel); 
        }
    }
    
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log("Player took " + damage + " damage. Health: " + currentHealth);
        
        // Update health bar
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateHealthBar(currentHealth, maxHealth);
        }
        
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    
    public void Heal(float amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        Debug.Log("Player healed! Health: " + currentHealth);
        
        // Update health bar
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateHealthBar(currentHealth, maxHealth);
        }
    }
    
    public void AddXP(int xp)
{
    currentXP += xp;
    Debug.Log("Gained " + xp + " XP! Total: " + currentXP);
    
    // Update XP bar  ← ADD THIS
    if (UIManager.Instance != null)
    {
        UIManager.Instance.UpdateXPBar(currentXP, xpToNextLevel);
    }
    
    // Check for level up
    if (currentXP >= xpToNextLevel)
    {
        LevelUp();
    }
}
    
   void LevelUp()
{
    currentLevel++;
    currentXP = 0;
    xpToNextLevel = currentLevel * 100;
    
    maxHealth += 20;
    currentHealth = maxHealth;
    
    Debug.Log("LEVEL UP! Now level " + currentLevel);
    
    // Update UI
    if (UIManager.Instance != null)
    {
        UIManager.Instance.UpdateHealthBar(currentHealth, maxHealth);
        UIManager.Instance.UpdateLevel(currentLevel);
        UIManager.Instance.UpdateXPBar(currentXP, xpToNextLevel);  // ← ADD THIS
    }
    
    // Show level up effect
    StartCoroutine(ShowLevelUpEffect());  // ← ADD THIS
}
    
   void Die()
{
    Debug.Log("Player died! GAME OVER");
    
    // Show game over screen
    if (UIManager.Instance != null)
    {
        UIManager.Instance.ShowGameOverScreen();
    }
}
    
    // CHEAT CODE SYSTEM
    private string cheatInput = "";

    void Update()
    {
        // Listen for letter keys
        foreach (char c in Input.inputString)
        {
            if (char.IsLetter(c))
            {
                cheatInput += char.ToUpper(c);
                
                if (cheatInput.Length > 10)
                {
                    cheatInput = cheatInput.Substring(1);
                }
                
                CheckCheats();
            }
        }
    }

    void CheckCheats()
    {
        if (cheatInput.Contains("HEALTH"))
        {
            currentHealth = maxHealth;
            Debug.Log("🎮 CHEAT ACTIVATED: FULL HEALTH!");
            
            // Update UI
            if (UIManager.Instance != null)
            {
                UIManager.Instance.UpdateHealthBar(currentHealth, maxHealth);
            }
            
            cheatInput = "";
        }
        
        if (cheatInput.Contains("GOD"))
        {
            maxHealth = 9999;
            currentHealth = 9999;
            Debug.Log("🎮 CHEAT ACTIVATED: GOD MODE!");
            
            // Update UI
            if (UIManager.Instance != null)
            {
                UIManager.Instance.UpdateHealthBar(currentHealth, maxHealth);
            }
            
            cheatInput = "";
        }
        
        if (cheatInput.Contains("LEVEL"))
        {
            LevelUp();
            Debug.Log("🎮 CHEAT ACTIVATED: LEVEL UP!");
            cheatInput = "";
        }
        
        if (cheatInput.Contains("POWER"))
        {
            Debug.Log("🎮 CHEAT ACTIVATED: POWER BOOST!");
            cheatInput = "";
        }
        
        if (cheatInput.Contains("LEGION"))
        {
            currentHealth = maxHealth;
            LevelUp();
            Debug.Log("🎮 SECRET CHEAT: ONE LEGION POWER!");
            cheatInput = "";
        }
    }
    System.Collections.IEnumerator ShowLevelUpEffect()
{
    // Flash screen effect
    GameObject flash = GameObject.Find("LevelUpFlash");
    if (flash == null)
    {
        // Create flash overlay
        Canvas canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        if (canvas != null)
        {
            flash = new GameObject("LevelUpFlash");
            flash.transform.SetParent(canvas.transform);
            
            UnityEngine.UI.Image flashImage = flash.AddComponent<UnityEngine.UI.Image>();
            flashImage.color = new Color(1, 1, 0, 0.5f); // Yellow flash
            
            RectTransform rt = flash.GetComponent<RectTransform>();
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;
        }
    }
    
    // Show flash
    if (flash != null)
    {
        flash.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        flash.SetActive(false);
    }
    
    Debug.Log("⭐ LEVEL UP EFFECT! ⭐");
}
}