using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementScript : MonoBehaviour
{
    [SerializeField] private float forceMagnitude;
    [SerializeField] private float maxVelocity;
    private Rigidbody rb;
    private Camera mainCamera;
    private Vector3 movementDirection;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if(Touchscreen.current.primaryTouch.press.isPressed){
            Vector2 touchPOS = Touchscreen.current.primaryTouch.position.ReadValue();
            Debug.Log(touchPOS);
            Vector3 worldPOS = mainCamera.ScreenToWorldPoint(touchPOS);
            // Debug.Log(worldPOS);
            movementDirection = transform.position - worldPOS; 
            movementDirection.z = 0f;
            movementDirection.Normalize();
        }else{
            movementDirection = Vector3.zero;
        }
    }

    void FixedUpdate(){
        if(movementDirection == Vector3.zero){return;}
        rb.AddForce(movementDirection * forceMagnitude * Time.deltaTime,ForceMode.Force);
        rb.linearVelocity =Vector3.ClampMagnitude(rb.linearVelocity, maxVelocity);
    }
}
