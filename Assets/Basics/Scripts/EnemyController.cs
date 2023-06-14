using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    public float flyingSpeed = 5f;
    public float circleRadius = 5f;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float shootingInterval = 10f;
    public float bulletSpeed = 10f;
    public float bulletLifetime = 10f;

    private Transform playerTransform;
    private Vector3 centerPosition;

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        centerPosition = playerTransform.position;

        // Start shooting bullets
        StartCoroutine(ShootBullets());
    }

    private void Update()
    {
        FlyInCircle();
    }

    private void FlyInCircle()
    {
        // Calculate the target position on the circle
        Vector3 targetPosition = centerPosition + Quaternion.Euler(0f, Time.time * flyingSpeed, 0f) * Vector3.forward * circleRadius;

        // Calculate direction towards the target position
        Vector3 direction = (targetPosition - transform.position).normalized;

        // Calculate rotation to look at the target position
        Quaternion lookRotation = Quaternion.LookRotation(direction);

        // Rotate enemy towards the target position
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime);

        // Move enemy forward
        transform.Translate(Vector3.forward * flyingSpeed * Time.deltaTime);
    }

    private IEnumerator ShootBullets()
    {
        while (true)
        {
            yield return new WaitForSeconds(shootingInterval);

            // Create a bullet at the fire point position and rotation
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();

            // Calculate direction towards the player
            Vector3 direction = (playerTransform.position - bullet.transform.position).normalized;

            // Apply force to the bullet towards the player
            bulletRb.velocity = direction * bulletSpeed;

            // Rotate the bullet to face the player
            bullet.transform.rotation = Quaternion.LookRotation(direction);

            // Destroy the bullet after the specified lifetime
            Destroy(bullet, bulletLifetime);
        }
    }
}




