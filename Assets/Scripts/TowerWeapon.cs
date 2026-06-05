using UnityEngine;

public class TowerWeapon : MonoBehaviour
{
    [Header("Tower Stats")]
    public float baseDamage = 1f; // NEW: Each tower can now have an independent damage value!
    public float attackRange = 15f;
    public float fireRate = 1f; // Bullets per second
    public float rotationSpeed = 10f;
    [Range(1f, 45f)] public float fireAngleWindow = 5f; // Must be within 5 degrees to fire!

    [Header("Setup Fields")]
    public GameObject projectilePrefab; // Drag your bullet/cannonball prefab here
    public Transform firePoint;          // An empty child object at the tip of the gun barrel
    public Transform turretRotatingPart; // The child object of the tower that should spin around

    private float nextFireTime;
    private Transform currentTarget;

    public AudioClip fireSoundClip;

    void Update()
    {
        // 1. Find and lock onto the closest enemy
        FindTarget();

        if (currentTarget != null)
        {
            // 2. Smoothly rotate the turret toward the enemy on the horizontal plane
            RotateTowardsTarget();

            // 3. ANGLE CHECK: Calculate the angle between where the barrel points vs where the enemy is
            Vector3 targetDir = currentTarget.position - turretRotatingPart.position;
            targetDir.y = 0; // Keep it on the ground plane
            
            float angleToEnemy = Vector3.Angle(turretRotatingPart.forward, targetDir);

            // 4. Handle the automatic shooting timer ONLY if pointing directly at them
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

        // Lock target only if it's within our maximum radar radius
        if (nearestEnemy != null && shortestDistance <= attackRange)
        {
            currentTarget = nearestEnemy.transform;
        }
        else
        {
            currentTarget = null; // No enemy or lost out of range
        }
    }

    void RotateTowardsTarget()
    {
        // Calculate direction vector ignoring height differences (Y axis)
        Vector3 direction = currentTarget.position - turretRotatingPart.position;
        direction.y = 0; 

        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            // Smoothly lerp/slerp the turret's rotation over time
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
        // Instantiate the bullet at our barrel's tip
        GameObject bullet = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        
        // Pass the targeted enemy reference AND this specific tower's damage value
        TowerProjectile projectileScript = bullet.GetComponent<TowerProjectile>();
        if (projectileScript != null)
        {
            // NEW LOGIC: We call our updated initialization function here!
            projectileScript.InitializeProjectile(currentTarget, baseDamage);
        }
    }

    // Visualizes the attack radius inside the Unity editor scene view
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}