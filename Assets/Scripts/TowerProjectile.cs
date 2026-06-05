using UnityEngine;

public class TowerProjectile : MonoBehaviour
{
    public float speed = 15f;
    [HideInInspector] public float damage = 1f; 
    
    private Transform targetEnemy;

    public void InitializeProjectile(Transform target, float towerDamageValue)
    {
        targetEnemy = target;
        damage = towerDamageValue;
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