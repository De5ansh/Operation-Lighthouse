using UnityEngine;

public class TowerWeapon : MonoBehaviour
{
    [Header("Tower Stats")]
    public float baseDamage = 1f; 
    public float attackRange = 15f;
    public float fireRate = 1f; 
    public float rotationSpeed = 10f;
    [Range(1f, 45f)] public float fireAngleWindow = 5f; 

    [Header("Setup Fields")]
    public GameObject projectilePrefab; 
    public Transform firePoint;          
    public Transform turretRotatingPart; 

    private float nextFireTime;
    private Transform currentTarget;

    public AudioClip fireSoundClip;

    void Update()
    {
        FindTarget();

        if (currentTarget != null)
        {

            RotateTowardsTarget();

            Vector3 targetDir = currentTarget.position - turretRotatingPart.position;
            targetDir.y = 0; 
            
            float angleToEnemy = Vector3.Angle(turretRotatingPart.forward, targetDir);

            if (Time.time >= nextFireTime && angleToEnemy <= fireAngleWindow)
            {
                Shoot();
                nextFireTime = Time.time + 1f / fireRate;
            }
        }
    }

    void FindTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null && shortestDistance <= attackRange)
        {
            currentTarget = nearestEnemy.transform;
        }
        else
        {
            currentTarget = null; 
        }
    }

    void RotateTowardsTarget()
    {
        Vector3 direction = currentTarget.position - turretRotatingPart.position;
        direction.y = 0; 

        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            turretRotatingPart.rotation = Quaternion.Slerp(turretRotatingPart.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        }
    }

    void Shoot()
    {
        if (projectilePrefab == null || firePoint == null) return;

        if (fireSoundClip != null)
        {
            AudioSource.PlayClipAtPoint(fireSoundClip, firePoint.position);
        }
        GameObject bullet = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

        TowerProjectile projectileScript = bullet.GetComponent<TowerProjectile>();
        if (projectileScript != null)
        {
            projectileScript.InitializeProjectile(currentTarget, baseDamage);
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}