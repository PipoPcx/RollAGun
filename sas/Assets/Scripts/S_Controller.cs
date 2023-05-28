using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.VisualScripting;
using UnityEngine;

public class S_Controller : MonoBehaviour
{
    #region Variables
    private Rigidbody rb;
    private Vector3 originalScale;
    public bool lockCursor = true;

    public float telekineticForce = 30f;

    #region Grappling Hook
    [Header("GRAPPLING HOOK")]
    [SerializeField]
    private float grapplingSpeed = 10f;
    [SerializeField]
    private float hookMaxDistance = 70f;
    [SerializeField]
    private float hookMinDistance = 3f;
    private RaycastHit hitInfo;
    [SerializeField]
    private GameObject hook;
    private bool isGrappling = false;
    private Vector3 grapplingDirection;
    private LineRenderer lineRenderer; 
    #endregion


    #region Move
    public bool playerCanMove = true;
    public float walkSpeed = 5f;
    public float maxVelocityChange = 10f;

    private bool isWalking = false;
    #endregion

    #region Dash
    [Header("DASH")]
    [SerializeField]
    private float dashDistance = 5f;
    [SerializeField]
    private float dashSpeed = 10f;

    private Vector3 dashTarget;
    private bool isDashing = false; 
    #endregion

    #region Sprint
    public float sprintSpeed = 7f;
    public bool enableSprint = true;
    private bool isSprinting = false;

    #endregion

    #region Jump
    private bool isGrounded = false;
    public bool enableJump = true;
    public float jumpForce = 7f;

    #endregion

    #region Camera
    public Camera playerCam;
    public bool cameraCanMove = true;
    public bool invertCamera = false;
    private float yaw = 0.0f;
    private float mouseSensitivity = 2f;
    private float pitch = 0.0f;
    public float maxLookAngle = 50f;
    #endregion

    #region HeadBob
    public bool enableHeadBob = true;
    public Transform joint;
    public float bobSpeed = 10f;
    public Vector3 bobAmount = new Vector3(.15f, .05f, 0f);

    private Vector3 jointPos;
    private float timer = 0f;
    #endregion

    #region RocketJump
    [Header("Rocket")]

    [SerializeField]
    public float rocketJumpForce = 10f;
    private bool desactivarRocketJump = false;
    #endregion

    #endregion

    #region Voids
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        jointPos = joint.localPosition;
        originalScale = transform.localScale;

    }

    private void Start()
    {
        if (lockCursor) {

            Cursor.lockState = CursorLockMode.Locked;
        }

        lineRenderer = GetComponent<LineRenderer>();
        hook.SetActive(false);
    }

    private void FixedUpdate() {

        if (playerCanMove)
        {

            Vector3 targetVelocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

            if (targetVelocity.x != 0 || targetVelocity.z != 0 && isGrounded)
            {

                isWalking = true;
            }
            else
            {

                isWalking = false;
            }
            if (enableSprint && Input.GetKey(KeyCode.LeftShift))
            {

                targetVelocity = transform.TransformDirection(targetVelocity) * sprintSpeed;

                Vector3 velocity = rb.velocity;
                Vector3 velocityChange = (targetVelocity - velocity);
                velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
                velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
                velocityChange.y = 0;

                if (velocityChange.x != 0 || velocityChange.z != 0)
                {

                    isSprinting = true;
                }

                rb.AddForce(velocityChange, ForceMode.VelocityChange);
            }

            else
            {

                isSprinting = false;
                targetVelocity = transform.TransformDirection(targetVelocity) * walkSpeed;
                Vector3 velocity = rb.velocity;
                Vector3 velocityChange = (targetVelocity - velocity);
                velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
                velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
                velocityChange.y = 0;

                rb.AddForce(velocityChange, ForceMode.VelocityChange);
            }
        }

        if (isDashing)
        {

            float step = dashSpeed * Time.fixedDeltaTime;

            rb.MovePosition(Vector3.MoveTowards(transform.position, dashTarget, step));

            if (Vector3.Distance(transform.position, dashTarget) < 0.5f)
            {

                isDashing = false;
                Debug.Log("IsDashing = false");
                dashTarget = Vector3.zero; //Reinicia la posición con tal de que no se quede pegado el personaje
            }
        }

        if(isGrappling) { // esto debe estar ejecutandose siempre
        
            Vector3 targetPosition = transform.position + grapplingDirection * grapplingSpeed * Time.fixedDeltaTime;
            Vector3 hookPos = hook.transform.position;
            float distanceToTarget = Vector3.Distance(transform.position, hookPos); //originalmente la segunda variable es targetPosition, no hookPos

            if(distanceToTarget <= Vector3.Distance(transform.position, hookPos)) { 
            
                transform.position = targetPosition;
                lineRenderer.SetPosition(0, transform.position);
                lineRenderer.SetPosition(1, hookPos);

                if (Vector3.Distance(transform.position, hook.transform.position) <= 1f) { 
                
                    StopGrapplingHook();
                    Debug.Log(Vector3.Distance(transform.position, hook.transform.position));
                    Debug.Log("distancia fallo");
                }
                else if (Input.GetMouseButtonDown(1) && isGrappling) {

                    StopGrapplingHook();
                    Debug.Log("Desengancha");
                }
            }
            else { 
            
                StopGrapplingHook();
            }
        }
    }

    private void Update()
    {

        if (enableHeadBob) { //Activa el head Bobing

            HeadBob();
        }

        transform.localScale = new Vector3(originalScale.x, originalScale.y, originalScale.z); //Debería arreglar la escala del personaje

        #region Camera
        if (cameraCanMove)
        {

            yaw = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * mouseSensitivity;

            if (!invertCamera)
            {

                pitch -= mouseSensitivity * Input.GetAxis("Mouse Y");
            }
            else
            {

                pitch += mouseSensitivity * Input.GetAxis("Mouse Y");
            }

            pitch = Mathf.Clamp(pitch, -maxLookAngle, maxLookAngle);

            transform.localEulerAngles = new Vector3(0, yaw, 0);
            playerCam.transform.localEulerAngles = new Vector3(pitch, 0, 0);
        }
        #endregion

        if (enableJump && Input.GetKeyDown(KeyCode.Space) && isGrounded) { //saltar

            Jump();
        }

        CheckGround(); //Ve si el personaje está en el suelo

        Dash(); //Avtiva el método Dash

        if(Input.GetKeyDown("f")) { 
        
            Telekinesis();
        }

        if(Input.GetMouseButtonDown(1)) { 
        
           if(isGrappling) { 
            
                isGrappling = false;
                StopGrapplingHook();
               
            }
           else { 
            
                isGrappling = true;
                GrapplingHook();
                Debug.Log("Hola");
            }
        }

    }
    #endregion

    #region Métodos
    private void CheckGround()
    {

        Vector3 origin = new Vector3(transform.position.x, transform.position.y - (transform.localScale.y * .5f), transform.position.z);
        Vector3 direction = transform.TransformDirection(Vector3.down);
        float distance = .75f;

        if (Physics.Raycast(origin, direction, out RaycastHit hit, distance))
        {

            Debug.DrawRay(origin, direction * distance, Color.red);
            isGrounded = true;
        }
        else
        {

            isGrounded = false;
        }
    }

    private void Jump()
    {

        if (isGrounded)
        {

            rb.AddForce(0f, jumpForce, 0f, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    private void HeadBob()
    {

        if (isWalking)
        {

            if (isSprinting)
            {

                timer += Time.deltaTime * (bobSpeed + sprintSpeed);
            }
            else
            {

                timer += Time.deltaTime * bobSpeed;
            }

            joint.localPosition = new Vector3(jointPos.x + Mathf.Sin(timer) *
            bobAmount.x, jointPos.y + Mathf.Sin(timer) * bobAmount.y, jointPos.z + Mathf.Sin(timer) * bobAmount.z);

        }
        else
        {

            timer = 0;
            joint.localPosition = new Vector3(Mathf.Lerp(joint.localPosition.x, jointPos.x, Time.deltaTime * bobSpeed),
            Mathf.Lerp(joint.localPosition.y, jointPos.y, Time.deltaTime * bobSpeed), Mathf.Lerp(joint.localPosition.z, jointPos.z, Time.deltaTime * bobSpeed));
        }
    }

    public void RocketJump()
    {
        if (!isGrounded && !desactivarRocketJump)
        {
            rb.velocity = Vector3.zero;
            rb.AddForce(Vector3.up * rocketJumpForce, ForceMode.Impulse);
            desactivarRocketJump = true;
            Debug.Log("Bool True");
            StartCoroutine(Rocket());
        }

    }

    public void Dash() {

        if (Input.GetKeyDown("c") && !isDashing)
        {
            Vector3 moveDirection = transform.TransformDirection(new Vector3(Input.GetAxis("Horizontal"),0f,Input.GetAxis("Vertical"))).normalized;
            dashTarget = transform.position + moveDirection * dashDistance;
            isDashing = true;
            Debug.Log("DashTarget: " + dashTarget);
        }
    }

    public void Telekinesis() {

        // Vector3 forward = transform.forward;
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

       if(Physics.Raycast(ray, out hit)) { 
        
            Vector3 direccion = (hit.point - transform.position).normalized;

            if(hit.collider.CompareTag("Kinetic")) {


                Rigidbody rb = hit.collider.GetComponent<Rigidbody>();
                if(rb != null) {

                    rb.AddForce(direccion * telekineticForce, ForceMode.Impulse);
                }

                //Rigidbody rb = hit.collider.GetComponent<Rigidbody>(); //Buscamos el RigidBody del objeto con tag Kinetic que colisiona con el RayCast
                //if(rb != null) {

                //    rb.AddForce(forward * telekineticForce, ForceMode.Impulse);
                //}
            }
       }
    }

    private void GrapplingHook() { // todo esto debe ejecutarse una sola vez
    
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        if(Physics.Raycast(ray, out hitInfo, hookMaxDistance)) {

            //Physics.gravity = new Vector3(0f,0f,0f);
            isGrappling = true;
            grapplingDirection = (hitInfo.point - transform.position).normalized;
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, hitInfo.point);
            
            

            hook.SetActive(true);
            hook.transform.position = hitInfo.point; //Originalmente la Pos del hook es lineRenderer.GetPosition(1), no hitInfo.Point
            hook.transform.rotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
            hook.transform.localScale = Vector3.one;
            hook.transform.SetParent(hitInfo.collider.transform);
        }
    }

    private void StopGrapplingHook() {

        //Physics.gravity = new Vector3(0f, -9.81f, 0f);
        isGrappling = false;
        lineRenderer.enabled = false;
        hook.transform.SetParent(null);
    }

    
    #endregion


  IEnumerator Rocket()
  {
    yield return new WaitForSeconds(2f);
    desactivarRocketJump = false;
    yield break;
  }
}

