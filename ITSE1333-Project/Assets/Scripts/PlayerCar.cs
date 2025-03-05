using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class PlayerCar : MonoBehaviour {

    [SerializeField] private GameObject cameraSocket;

    [SerializeField] private float speed;
    [SerializeField] private float acceleration;
    [SerializeField] private float turnSpeed;

    private Rigidbody rb;
    private Vector2 moveDirection;

    private CarControls carControls;

    private void Start() {
        rb = GetComponent<Rigidbody>();
        carControls = new CarControls();
        carControls.Player.Enable();
    }


    private void FixedUpdate() {
        moveDirection = carControls.Player.MovementVector.ReadValue<Vector2>();



        // Forward and backwards movement
        if (moveDirection.y > 0) {
            rb.linearVelocity = Vector3.MoveTowards(rb.linearVelocity, transform.forward * speed, Time.fixedDeltaTime * acceleration);
        }
        else if(moveDirection.y < 0) {
            rb.linearVelocity = Vector3.MoveTowards(rb.linearVelocity, -transform.forward * speed, Time.fixedDeltaTime * acceleration);
        }
        else {
            rb.linearVelocity = Vector3.MoveTowards(rb.linearVelocity, Vector3.zero, Time.fixedDeltaTime * acceleration);
        }
        // Turning movement
        if (moveDirection.x > 0) {
            Quaternion deltaRotation = Quaternion.Euler(new Vector3(0, turnSpeed, 0) * Time.fixedDeltaTime);
            rb.MoveRotation(rb.rotation * deltaRotation);
        }
        else if (moveDirection.x < 0) {
            Quaternion deltaRotation = Quaternion.Euler(new Vector3(0, -turnSpeed, 0) * Time.fixedDeltaTime);
            rb.MoveRotation(rb.rotation * deltaRotation);
        }
    }


}
