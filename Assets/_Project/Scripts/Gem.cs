using UnityEngine;

public class Gem : MonoBehaviour
{
    [Header("Gem Settings")]
    public int gemValue = 10;
    public int xpValue = 5;
    
    [Header("Visual")]
    public float rotateSpeed = 100f;
    public float bobSpeed = 2f;
    public float bobHeight = 0.2f;
    
    private Vector3 startPosition;
    
    void Start()
    {
        startPosition = transform.position;
    }
    
    void Update()
    {
        // Rotate gem for visual effect
        transform.Rotate(0, 0, rotateSpeed * Time.deltaTime);
        
        // Bob up and down
        float newY = startPosition.y + Mathf.Sin(Time.time * bobSpeed) * bobHeight;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
    
    void OnTriggerEnter2D(Collider2D other)
{
    if (other.CompareTag("Player"))
    {
        // Add gems to counter
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddGems(gemValue);
        }
        
        // Give XP
        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.AddXP(xpValue);
        }
        
        Debug.Log("💎 Collected gem! +" + gemValue + " gems, +" + xpValue + " XP");
        
        // Destroy gem
        Destroy(gameObject);
    }
}
}