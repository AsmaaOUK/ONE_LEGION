using UnityEngine;

public class BossAI : MonoBehaviour
{
    [Header("Boss Stats")]
    public float maxHealth = 300f;
    public float currentHealth;
    public float moveSpeed = 1.5f; // Slower but scarier
    public float attackDamage = 25f; // Hits harder!
    public float detectionRange = 15f; // Sees player from far
    public float attackRange = 3f;
    
    [Header("Boss Abilities")]
    public bool isEnraged = false;
    public float enrageHealthPercent = 0.5f; // Gets angry at 50% HP
    
    [Header("Rewards")]
    public int gemsDropped = 50;
    public int xpReward = 200;

    [Header("Health Bar")] 
    public GameObject healthBarPrefab;  
    private EnemyHealthBar healthBar; 
    
[Header("Gem Drop")]  
public GameObject gemPrefab;  
    private Transform player;

    [Header("Victory Character")]
public GameObject victoryCharacter;
    private Rigidbody2D rb;
    private float attackCooldown = 2f; // Slower attacks
    private float lastAttackTime = 0f;
    private SpriteRenderer spriteRenderer;
    
    void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        // Find the player
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
         // Create health bar
    if (healthBarPrefab != null)
    {
        GameObject hbInstance = Instantiate(healthBarPrefab, transform);
        healthBar = hbInstance.GetComponent<EnemyHealthBar>();
        if (healthBar != null)
        {
            healthBar.UpdateHealth(currentHealth, maxHealth);
        }
    }
    }
    
    void Update()
    {
        if (player == null) return;
        
        // Check for enrage mode
        float healthPercent = currentHealth / maxHealth;
        if (healthPercent <= enrageHealthPercent && !isEnraged)
        {
            EnterEnrageMode();
        }
        
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        
        // Chase player if in range
        if (distanceToPlayer <= detectionRange && distanceToPlayer > attackRange)
        {
            ChasePlayer();
        }
        
        // Attack if close enough
        if (distanceToPlayer <= attackRange)
        {
            AttackPlayer();
        }
    }
    
    void ChasePlayer()
    {
        // Move toward player
        Vector2 direction = (player.position - transform.position).normalized;
        float currentSpeed = isEnraged ? moveSpeed * 1.5f : moveSpeed;
        
        transform.position = Vector2.MoveTowards(
            transform.position, 
            new Vector2(player.position.x, transform.position.y), 
            currentSpeed * Time.deltaTime
        );
    }
    
   void AttackPlayer()
{
    // Attack on cooldown
    float currentCooldown = isEnraged ? attackCooldown * 0.5f : attackCooldown;
    
    if (Time.time >= lastAttackTime + currentCooldown)
    {
        // Actually damage the player HERE!
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            float damage = isEnraged ? attackDamage * 1.5f : attackDamage;
            playerHealth.TakeDamage(damage);
            Debug.Log("💀 BOSS attacked player for " + damage + " damage!");
        }
        
        // Flash red when attacking
        StartCoroutine(FlashRed());
        
        lastAttackTime = Time.time;
    }
}
    
    void EnterEnrageMode()
    {
        isEnraged = true;
        Debug.Log("🔥 BOSS IS ENRAGED! 🔥");
        
        // Change color to show rage
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.red;
        }
        
        // Boss gets faster and attacks more
        attackDamage *= 1.5f;
    }
    
   System.Collections.IEnumerator FlashRed()
{
    Color originalColor = spriteRenderer.color;
    spriteRenderer.color = Color.white; // Flash white
    yield return new WaitForSeconds(0.1f);
    spriteRenderer.color = originalColor; // Back to original
}
    
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log("💀 BOSS took " + damage + " damage! HP: " + currentHealth);
        
          // Update health bar
    if (healthBar != null)
    {
        healthBar.UpdateHealth(currentHealth, maxHealth);
    }
        // Flash when hit
        StartCoroutine(FlashRed());
        
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    
 void Die()
{
    Debug.Log("💥💥 BOSS DEFEATED! 💥💥");
    
    // Drop gems
    if (gemPrefab != null)
    {
        int numberOfGems = 5;
        
        for (int i = 0; i < numberOfGems; i++)
        {
            Vector3 randomOffset = new Vector3(
                Random.Range(-1f, 1f), 
                Random.Range(0.5f, 1.5f), 
                0
            );
            Vector3 dropPosition = transform.position + randomOffset;
            
            GameObject droppedGem = Instantiate(gemPrefab, dropPosition, Quaternion.identity);
            
            Rigidbody2D gemRb = droppedGem.GetComponent<Rigidbody2D>();
            if (gemRb == null)
            {
                gemRb = droppedGem.AddComponent<Rigidbody2D>();
            }
            gemRb.gravityScale = 1f;
            Vector2 randomForce = new Vector2(
                Random.Range(-2f, 2f), 
                Random.Range(5f, 8f)
            );
            gemRb.AddForce(randomForce, ForceMode2D.Impulse);
            
            Gem gemScript = droppedGem.GetComponent<Gem>();
            if (gemScript != null)
            {
                gemScript.gemValue = gemsDropped / numberOfGems;
                gemScript.xpValue = xpReward / numberOfGems;
            }
        }
    }
    
    // Track boss killed
    if (GameManager.Instance != null)
    {
        GameManager.Instance.EnemyKilled();
    }
    
    // Disable components but don't destroy yet
    GetComponent<BossAI>().enabled = false;
    GetComponent<BoxCollider2D>().enabled = false;
    GetComponent<Rigidbody2D>().simulated = false;
    
    // Hide sprite
    if (spriteRenderer != null)
    {
        spriteRenderer.enabled = false;
    }
    
    // Show victory screen after delay, THEN destroy
    StartCoroutine(ShowVictoryAfterDelay());
}
System.Collections.IEnumerator ShowVictoryAfterDelay()
{
    Debug.Log("✅ Boss defeated! Starting victory sequence...");
    
    // Wait for gems to finish dropping
    yield return new WaitForSeconds(3f);
    
    // Wait for gems to be collected (max 10 seconds)
    float waitTime = 0f;
    float maxWaitTime = 10f;
    
    while (waitTime < maxWaitTime)
    {
        GameObject[] gemsLeft = GameObject.FindGameObjectsWithTag("Gem");
        
        if (gemsLeft.Length == 0)
        {
            Debug.Log("✅ All gems collected!");
            break;
        }
        
        yield return new WaitForSeconds(0.5f);
        waitTime += 0.5f;
    }
    
    // === HIDE THE PLAYER ===
    GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
    if (playerObj != null)
    {
        playerObj.SetActive(false);
        Debug.Log("✅ Player hidden!");
    }
    
    // === SHOW VICTORY CHARACTER ===
    if (victoryCharacter != null)
    {
        victoryCharacter.SetActive(true);
        Debug.Log("✅ Victory character shown!");
        
        // Wait for animation to play
        yield return new WaitForSeconds(3f);
    }
    else
    {
        Debug.LogError("❌ Victory Character not assigned!");
    }
    
    // Show victory UI
    if (UIManager.Instance != null && GameManager.Instance != null)
    {
        PlayerHealth playerHealth = playerObj?.GetComponent<PlayerHealth>();
        int level = playerHealth != null ? playerHealth.currentLevel : 1;
        
        UIManager.Instance.ShowVictoryScreen(
            GameManager.Instance.enemiesKilled,
            GameManager.Instance.totalGems,
            level
        );
    }
    
    // Destroy boss
    Destroy(gameObject);
}
}