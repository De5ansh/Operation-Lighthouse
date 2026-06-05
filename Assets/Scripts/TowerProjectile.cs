using UnityEngine;

public class TowerProjectile : MonoBehaviour
{
    public float speed = 15f;
    [HideInInspector] public float damage = 1f; // Hidden because the tower will inject this value dynamically!
    
    private Transform targetEnemy;

    // Modified to receive both the Target AND the specific Tower's damage stat!
    public void InitializeProjectile(Transform target, float towerDamageValue)
    {
        targetEnemy = target;
        damage = towerDamageValue; // Set this bullet's damage to match the tower that shot it
    }

    void Update()
    {
        if (targetEnemy == null)
        {
            Destroy(gameObject); 
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, targetEnemy.position, speed * Time.deltaTime);
        transform.LookAt(targetEnemy.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyAI enemyScript = other.GetComponent<EnemyAI>();
            if (enemyScript != null)
            {
                enemyScript.TakeDamage(damage); 
            }

            Destroy(gameObject); 
        }
    }
}