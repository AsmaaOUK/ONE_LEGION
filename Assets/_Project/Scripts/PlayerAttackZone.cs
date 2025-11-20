using UnityEngine;

public class PlayerAttackZone : MonoBehaviour
{
    private PlayerMovement2D playerMovement;
    private Collider2D currentEnemy; // Track enemy in range
    
    void Start()
    {
        // Get parent's movement script
        playerMovement = GetComponentInParent<PlayerMovement2D>();
    }
    
    void Update()
    {
        // Check for F key press while enemy is in range
        if (currentEnemy != null && Input.GetKeyDown(KeyCode.F))
        {
            if (playerMovement != null && Time.time >= playerMovement.lastAttackTime + playerMovement.attackCooldown)
            {
                AttackEnemy(currentEnemy);
                playerMovement.lastAttackTime = Time.time;
            }
        }
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            currentEnemy = other;
        }
    }
    
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            currentEnemy = other;
        }
    }
    
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") && currentEnemy == other)
        {
            currentEnemy = null;
        }
    }
    
    void AttackEnemy(Collider2D enemyCollider)
    {
        // Try to damage normal enemy
        EnemyAI enemy = enemyCollider.GetComponent<EnemyAI>();
        if (enemy != null)
        {
            enemy.TakeDamage(playerMovement.attackDamage);
            Debug.Log("💥 Player attacked enemy! Enemy HP: " + enemy.currentHealth);
        }
        
        // Try to damage boss
        BossAI boss = enemyCollider.GetComponent<BossAI>();
        if (boss != null)
        {
            boss.TakeDamage(playerMovement.attackDamage);
            Debug.Log("💥 Player attacked BOSS! Boss HP: " + boss.currentHealth);
        }
    }
}