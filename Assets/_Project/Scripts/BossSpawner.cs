using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    public GameObject bossPrefab;
    public Transform bossSpawnPoint;
    public int enemiesRequiredToKill = 3;
    
    private bool bossSpawned = false;
    
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("🟢 SOMETHING HIT THE TRIGGER!");
        Debug.Log("Hit by: " + other.gameObject.name);
        
        if (other.CompareTag("Player") && !bossSpawned)
        {
            Debug.Log("🟢 PLAYER HIT THE TRIGGER!");
            
            if (GameManager.Instance != null)
            {
                Debug.Log("Enemies killed: " + GameManager.Instance.enemiesKilled);
                
                if (GameManager.Instance.enemiesKilled >= enemiesRequiredToKill)
                {
                    Debug.Log("🔥 SPAWNING BOSS!");
                    SpawnBoss();
                }
                else
                {
                    Debug.Log("❌ Need to kill more enemies!");
                }
            }
            else
            {
                Debug.Log("⚠️ No GameManager - spawning anyway!");
                SpawnBoss();
            }
        }
    }
    
    void SpawnBoss()
    {
        if (bossPrefab != null && bossSpawnPoint != null)
        {
            Debug.Log("✅ Creating boss at: " + bossSpawnPoint.position);
            GameObject boss = Instantiate(bossPrefab, bossSpawnPoint.position, Quaternion.identity);
            Debug.Log("✅ Boss created: " + boss.name);
            bossSpawned = true;
        }
        else
        {
            Debug.LogError("❌ Boss Prefab or Spawn Point is NULL!");
        }
    }
}