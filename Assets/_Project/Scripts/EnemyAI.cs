using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Enemy Stats")]
    public float maxHealth = 50f;
    public float currentHealth;
    public float moveSpeed = 2f;
    public float attackDamage = 10f;
    public float detectionRange = 8f;
    public float attackRange = 2f;
    
    [Header("Rewards")]
    public int gemsDropped = 5;
    public int xpReward = 25;
    
    [Header("Health Bar")]
public GameObject healthBarPrefab;
private EnemyHealthBar healthBar;

[Header("Gem Drop")]
public GameObject gemPrefab;  
    private Transform player;
    private Rigidbody2D rb;
    private float attackCooldown = 1f;
    private float lastAttackTime = 0f;
    
    void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        
        // Find the player
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
         // ADD THIS SECTION:
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
    // Move toward player using Transform instead of physics
    Vector2 direction = (player.position - transform.position).normalized;
    transform.position = Vector2.MoveTowards(
        transform.position, 
        new Vector2(player.position.x, transform.position.y), 
        moveSpeed * Time.deltaTime
    );
}
    
 void AttackPlayer()
{
    // Don't stop moving completely, just slow down
    Vector2 direction = (player.position - transform.position).normalized;
    rb.linearVelocity = new Vector2(direction.x * moveSpeed * 0.3f, rb.linearVelocity.y);
    
    // Attack on cooldown
    if (Time.time >= lastAttackTime + attackCooldown)
    {
            // Deal damage to player
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(attackDamage);
                Debug.Log("Enemy attacked player!");
            }
            
            lastAttackTime = Time.time;
        }
    }
    
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log("Enemy took " + damage + " damage. Health: " + currentHealth);
        // Update health bar
    if (healthBar != null)
    {
        healthBar.UpdateHealth(currentHealth, maxHealth);
    }
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    
   void Die()
{
    Debug.Log("Enemy died!");
        if (healthBar != null)
    {
        Destroy(healthBar.gameObject);
    }
    // Spawn gem drop
    if (gemPrefab != null)
    {
        // Create gem at enemy position
        Vector3 dropPosition = transform.position + new Vector3(0, 0.5f, 0);
        GameObject droppedGem = Instantiate(gemPrefab, dropPosition, Quaternion.identity);
        
        // Add upward force (makes it pop up!)
        Rigidbody2D gemRb = droppedGem.GetComponent<Rigidbody2D>();
        if (gemRb == null)
        {
            gemRb = droppedGem.AddComponent<Rigidbody2D>();
        }
        gemRb.gravityScale = 1f;
        gemRb.AddForce(Vector2.up * 5f, ForceMode2D.Impulse);
        
        // Set gem value
        Gem gemScript = droppedGem.GetComponent<Gem>();
        if (gemScript != null)
        {
            gemScript.gemValue = gemsDropped;
            gemScript.xpValue = xpReward;
        }
    }
    
    // Track enemy killed
    if (GameManager.Instance != null)
    {
        GameManager.Instance.EnemyKilled();
    }
    
    Destroy(gameObject);
}

    // Handle trigger collisions (when Is Trigger is checked)
void OnTriggerStay2D(Collider2D other)
{
    if (other.CompareTag("Player"))
    {
        // Attack on cooldown
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(attackDamage);
                Debug.Log("💀 Enemy attacked player for " + attackDamage + " damage!");
            }
            
            lastAttackTime = Time.time;
        }
    }
}
}