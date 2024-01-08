using UnityEngine;

public class Arici : MonoBehaviour
{
    public GameObject player;
    public float maxSpeed = 100.0f; // Maximum speed when far from the player
    public float minSpeed = 10.0f; // Minimum speed when close to the player
    public float detectionRange = 5.0f; // Range at which the enemy starts slowing down
    public float chargeDistance = 1.0f; // Distance at which the charge attack begins
    public float chargeSpeed = 5.0f; // Speed of the charge attack
    public float chargeCooldown = 1.0f; // Delay between charge attacks
    public float postChargeDistance = 5.0f; // Distance to move away from the player after charging

    private bool isCharging = false;
    private bool isMovingAway = false;
    private Vector3 initialPosition;
    private Vector3 targetPosition;
    private float chargeTimer = 0.0f;
    private float moveAwayTimer = 0.0f;

    private void Start()
    {
        initialPosition = transform.position;
        targetPosition = initialPosition;
    }

    void Update()
    {
        Vector3 playerPos = new Vector3(player.transform.position.x, player.transform.position.y + 2f,
            player.transform.position.z);
        Vector3 moveDirection = playerPos - transform.position;

        float distanceToPlayer = moveDirection.magnitude;

        // Normalize the direction vector to ensure consistent speed.
        moveDirection.Normalize();

        if (chargeTimer <= 0.0f && moveAwayTimer <= 0.0f)
        {
            if (!isCharging && !isMovingAway)
            {
                // Calculate the speed based on the distance to the player.
                float speed = Mathf.Lerp(minSpeed, maxSpeed, distanceToPlayer / detectionRange);

                // Move the enemy towards the player's position.
                transform.position += moveDirection * (speed * Time.deltaTime);

                // Check if the enemy should start charging.
                if (distanceToPlayer <= chargeDistance)
                {
                    isCharging = true;
                }
            }
            else if (isCharging)
            {
                // Perform the charge attack.
                transform.position += moveDirection * (chargeSpeed * Time.deltaTime);

                // Check if the enemy has reached or passed the player's position.
                if (Vector3.Dot(playerPos - initialPosition, playerPos - transform.position) <= 0)
                {
                    // Stop charging when the enemy reaches or passes the player's position.
                    isCharging = false;
                    transform.position = playerPos - moveDirection * chargeDistance; // Ensure the enemy stops exactly 1 meter away.
                    
                    // Start the charge cooldown timer.
                    chargeTimer = chargeCooldown;

                    // Determine a random target position around 5 meters away from the player.
                    float randomAngle = Random.Range(0, 360);
                    Vector3 randomDirection = Quaternion.Euler(0, randomAngle, 0) * Vector3.forward;
                    targetPosition = playerPos + randomDirection * postChargeDistance;

                    // Start moving away from the player.
                    isMovingAway = true;
                }
            }
            else if (isMovingAway)
            {
                // Move away from the player.
                Vector3 moveAwayDirection = targetPosition - transform.position;
                float moveAwayDistance = moveAwayDirection.magnitude;
                moveAwayDirection.Normalize();

                float moveAwaySpeed = Mathf.Lerp(minSpeed, maxSpeed, moveAwayDistance / postChargeDistance);

                transform.position += moveAwayDirection * (moveAwaySpeed * Time.deltaTime);

                // Check if the enemy has reached the target position.
                if (moveAwayDistance <= 0.1f)
                {
                    isMovingAway = false;
                    // Reset the timer to prevent immediate charging.
                    chargeTimer = chargeCooldown;
                }
            }
        }
        else
        {
            // Charge cooldown and move away timer are counting down.
            chargeTimer -= Time.deltaTime;
            moveAwayTimer -= Time.deltaTime;
        }
    }
}