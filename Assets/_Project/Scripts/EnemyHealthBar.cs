using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    public Image fillImage;
    public Canvas canvas;
    
    private Transform enemy;
    private Camera mainCamera;
    
    void Start()
    {
        mainCamera = Camera.main;
        enemy = transform.parent;
        
        // Make sure canvas faces camera
        if (canvas != null)
        {
            canvas.renderMode = RenderMode.WorldSpace;
        }
    }
    
    void LateUpdate()
    {
        // Always face the camera
        if (mainCamera != null)
        {
            transform.rotation = mainCamera.transform.rotation;
        }
        
        // Position above enemy
        if (enemy != null)
        {
            Vector3 offset = new Vector3(0, 1.5f, 0); // Adjust height here
            transform.position = enemy.position + offset;
        }
    }
    
    public void UpdateHealth(float currentHealth, float maxHealth)
    {
        if (fillImage != null)
        {
            fillImage.fillAmount = currentHealth / maxHealth;
        }
    }
}