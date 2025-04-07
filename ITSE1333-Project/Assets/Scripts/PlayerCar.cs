using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections;

public class PlayerCar : MonoBehaviour {

    [SerializeField] private GameObject cameraSocket;

    [SerializeField] private float speed;
    [SerializeField] private float acceleration;
    [SerializeField] private float turnSpeed;


    [HideInInspector]
    public bool canMove = false;

    private Rigidbody rb;
    private Animator anim;
    private Vector2 moveDirection;

    private CarControls carControls;


    public Vector3 accelerationData = Vector3.zero;


    // Assigned in editor
    public TextMeshProUGUI AccelDataField;
    private Accelerometer accel;

    private bool timerRunning = false;
    private float timeElapsed = 0;
    // Assigned in editor
    public TextMeshProUGUI TimeElapsedField;

    protected void OnEnable()
    {
        // All sensors start out disabled so they have to manually be enabled first.
        InputSystem.EnableDevice(Accelerometer.current);
    }

    protected void OnDisable()
    {
        InputSystem.DisableDevice(Accelerometer.current);
    }

    private void Start() {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        carControls = new CarControls();
        carControls.Player.Enable();

        StartCoroutine(DisableCarMovement(1f));
    }

    private void Update()
    {
        accel = Accelerometer.current;
        if (accel != null)
        {
            AccelDataField.text = accel.acceleration.ReadValue().ToString();
            accelerationData = accel.acceleration.ReadValue();
        }

        if (timerRunning)
        {
            timeElapsed += Time.deltaTime;
            TimeElapsedField.text = $"TIME: {timeElapsed}";
        }
    }

    private void FixedUpdate() {



        moveDirection.x = accelerationData.x;

        //moveDirection = carControls.Player.MovementVector.ReadValue<Vector2>();



        if (canMove)
        {
            // Forward and backwards movement
            if (moveDirection.y > 0)
            {
                rb.linearVelocity = Vector3.MoveTowards(rb.linearVelocity, transform.forward * speed, Time.fixedDeltaTime * acceleration);
            }
            else if (moveDirection.y < 0)
            {
                rb.linearVelocity = Vector3.MoveTowards(rb.linearVelocity, -transform.forward * speed, Time.fixedDeltaTime * acceleration);
            }
            else
            {
                rb.linearVelocity = Vector3.MoveTowards(rb.linearVelocity, Vector3.zero, Time.fixedDeltaTime * acceleration);
            }
            // Turning movement
            if (moveDirection.x > 0)
            {
                Quaternion deltaRotation = Quaternion.Euler(new Vector3(0, turnSpeed, 0) * Time.fixedDeltaTime);
                rb.MoveRotation(rb.rotation * deltaRotation);
            }
            else if (moveDirection.x < 0)
            {
                Quaternion deltaRotation = Quaternion.Euler(new Vector3(0, -turnSpeed, 0) * Time.fixedDeltaTime);
                rb.MoveRotation(rb.rotation * deltaRotation);
            }
        }
        else
        {
            rb.linearVelocity = Vector3.MoveTowards(rb.linearVelocity, Vector3.zero, Time.fixedDeltaTime * acceleration * .75f);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Oil"))
        {
            anim.Play("Spin");
            StartCoroutine(DisableCarMovement(1f));   
        }
        if (other.gameObject.layer == LayerMask.NameToLayer("Finish"))
        {
            if (timeElapsed > 20)
            {
                timerRunning = false;
            }
        }
    }

    private IEnumerator DisableCarMovement(float delay)
    {
        canMove = false;
        yield return new WaitForSeconds(delay);
        canMove = true;

        if (!timerRunning)
        {
            timerRunning = true;
        }
    }
}
