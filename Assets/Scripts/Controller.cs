using Unity.Hierarchy;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public float moveSpeed;
    public float rotationSpeed = 720f;

    public Rigidbody2D rigidBody;
    public Vector2 moveDirection;

    void Update()
    {
        ProcessInputs();
    }
    private void FixedUpdate()
    {
        Move();
    }

    void ProcessInputs()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        moveDirection = new Vector2(moveX, moveY).normalized;
    }

    void Move()
    {
        rigidBody.linearVelocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);

        if (moveDirection != Vector2.zero)
        {
            float targetAngle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle); // Adjust for sprite orientation

            // Smoothly rotate towards the target angle
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
