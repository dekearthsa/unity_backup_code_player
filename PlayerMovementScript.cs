using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementScript : MonoBehaviour
{   
    [SerializeField] private Transform childVisual; 
    [SerializeField] private float forceMagnitude;
    [SerializeField] private float maxVelocity;
    [SerializeField] private float rotationSpeed = 100f;
    // [SerializeField] private float rotationSpeedr;
    private Rigidbody rb;
    private Camera mainCamera;
    private Vector3 movementDirection;
    
    private string moveRotaionStatus;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        mainCamera = Camera.main;
        childVisual.localEulerAngles = new Vector3(-90, 0, 0);
    }
    // Update is called once per frame
    void Update()
    {
        ProcessInput();
        KeepPlayerOnScreen();
        RotateToFaceVelocity();
    }
    private void KeepPlayerOnScreen(){
        Vector3 newPOS = transform.position;
        Vector3 viewPortPOS =  mainCamera.WorldToViewportPoint(transform.position);

        if(viewPortPOS.x > 1){
            newPOS.x = - newPOS.x + 0.1f;
        }else if(viewPortPOS.x < 0){
            newPOS.x = - newPOS.x - 0.1f;
        }

        if(viewPortPOS.y > 1){
            newPOS.y = - newPOS.y + 0.1f;
        }else if(viewPortPOS.y < 0){
            newPOS.y = - newPOS.y - 0.1f;
        }
        transform.position =  newPOS;
    }

    private void ProcessInput(){
        // float currentYRotation = transform.eulerAngles.y;
        // float normalizedYRotation = ((currentYRotation + 180) % 360) - 180;

        if(Touchscreen.current.primaryTouch.press.isPressed){
            
            Vector2 touchPOS = Touchscreen.current.primaryTouch.position.ReadValue();
            Vector3 screenPosition = new Vector3(touchPOS.x, touchPOS.y, mainCamera.nearClipPlane);
            Vector3 worldPOS = mainCamera.ScreenToWorldPoint(screenPosition);
            // Debug.Log(worldPOS);
            movementDirection = worldPOS - transform.position;
            movementDirection.z = 0f;
            movementDirection.Normalize();
            // Apply rotation when moving
            // Debug.Log(moveRotaionStatus);
            
            
            // if (moveRotaionStatus == "left" && normalizedYRotation < 50f)
            // {
            //     // float newRotation = normalizedYRotation + (rotationSpeed * Time.deltaTime);
            //     // SetChildYRotation(newRotation);
            //     float newRotation = normalizedYRotation + (rotationSpeed * Time.deltaTime);
            //     Quaternion debugData = Quaternion.Euler(x:-90, y:newRotation, z:transform.rotation.eulerAngles.z);
            //     transform.rotation = Quaternion.Euler(x:-90, y:newRotation, z:transform.rotation.eulerAngles.z);
            //     Debug.Log("newRotation " + newRotation);
            //     Debug.Log(debugData);
            // }
            
            // if (moveRotaionStatus == "right" && normalizedYRotation > -50f)
            // {
            //     float newRotation = normalizedYRotation - (rotationSpeed * Time.deltaTime);
            //     Quaternion debugData = Quaternion.Euler(x:-90, y:newRotation, z:transform.rotation.eulerAngles.z);
            //     transform.rotation = Quaternion.Euler(x:-90, y:newRotation, z:transform.rotation.eulerAngles.z);
            //     Debug.Log("newRotation " + newRotation);
            //     Debug.Log(debugData);
            // }
        }else{
            movementDirection = Vector3.zero;
            // if(moveRotaionStatus != null){
            //     if(moveRotaionStatus == "left" && normalizedYRotation >= 1.2f ){
            //         float newRotation = normalizedYRotation - (rotationSpeed * Time.deltaTime);
            //         transform.rotation = Quaternion.Euler(x:-90, y:newRotation, z:transform.rotation.eulerAngles.z);
            //         // Debug.Log(transform.rotation);

            //     }else if(moveRotaionStatus == "right" && normalizedYRotation <= -1.2 ){
            //         float newRotation = normalizedYRotation + (rotationSpeed * Time.deltaTime);
            //         transform.rotation = Quaternion.Euler(x:-90, y:newRotation, z:transform.rotation.eulerAngles.z);
            //         // Debug.Log(transform.rotation);

            //     }
            //     else if (normalizedYRotation < 1.2f && normalizedYRotation > - 1.2) 
            //     {
            //         moveRotaionStatus = null;  
            //     }
            // }
        }
    }

    // private float ToSignedAngle(float angle)
    // {
    //     float signed = ((angle + 180f) % 360f) - 180f;
    //     return signed;
    // }

    // // Helper: แปลง -180..180 => 0..360
    // private float ToPositiveAngle(float angle)
    // {
    //     float positive = angle % 360f;
    //     if (positive < 0) positive += 360f;
    //     return positive;
    // }

    public void MoveLeftRotation(){
        moveRotaionStatus = "left";
        
    }
    public void MoveRightRotaion(){
        moveRotaionStatus = "right";
        
    }
    void FixedUpdate(){
        if(movementDirection == Vector3.zero){return;}
        rb.AddForce(movementDirection * forceMagnitude * Time.deltaTime,ForceMode.Force);
        rb.linearVelocity =Vector3.ClampMagnitude(rb.linearVelocity, maxVelocity);
    }

    private void RotateToFaceVelocity(){
        if(rb.linearVelocity  == Vector3.zero){return;}

        Quaternion targetRotation = Quaternion.LookRotation(rb.linearVelocity, Vector3.back);
        transform.rotation = Quaternion.Lerp(
            transform.rotation,targetRotation, rotationSpeed * Time.deltaTime);
    }
}

