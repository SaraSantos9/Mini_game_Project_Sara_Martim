using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private bool faceMoveDirection;
    [SerializeField] private float Speed = 5f;


     private Animator animator;

    private Rigidbody rb;
    private Vector2 moveInput;
    
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
    }

    public void Move(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        

    }



    void UpdateAnimator()
    {
        if (animator == null) return;

        //Convert movement input into a speed value (0 when idle, >0 when moving)
        float currentSpeed = new Vector3(moveInput.x, 0f, moveInput.y).magnitude;

        animator.SetFloat("Speed", currentSpeed);
    }


    void FixedUpdate()
    {
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        Vector3 movement = forward * moveInput.y + right * moveInput.x;

        Vector3 velocity = rb.linearVelocity;

        velocity.x = movement.x * Speed;
        velocity.z = movement.z * Speed;

        rb.linearVelocity = velocity;

        if (faceMoveDirection && movement.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement, Vector3.up);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, 10f * Time.fixedDeltaTime));
        }
        UpdateAnimator();

        if (moveInput != Vector2.zero)
    Debug.Log($"movement: {movement}, velocity: {rb.linearVelocity}");
    }
   
}
