using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class S_Tests : MonoBehaviour
{
    public Rigidbody rb;
    public float dashDistance = 5f;
    public float dashSpeed = 10f;

    private Vector3 dashTarget;
    private bool isDashing = false;

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

    private void Update()
    {
        Dash();

        if (Input.GetMouseButton(1))
        {

            if (isGrappling)
            {

                isGrappling = false;
                StopGrapplingHook();
            }
            else
            {

                isGrappling = true;
                GrapplingHook();
            }
        }
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {

            float step = dashSpeed * Time.fixedDeltaTime;

            rb.MovePosition(Vector3.MoveTowards(transform.position, dashTarget, step));

            if (Vector3.Distance(transform.position, dashTarget) < 0.1f)
            {

                isDashing = false;
                Debug.Log("IsDashing = false");
                dashTarget = Vector3.zero;
            }
        }

        if (isGrappling)
        { // esto debe estar ejecutandose siempre

            Vector3 targetPosition = transform.position + grapplingDirection * grapplingSpeed * Time.fixedDeltaTime;
            float distanceToTarget = Vector3.Distance(transform.position, targetPosition);

            if (distanceToTarget <= Vector3.Distance(transform.position, hitInfo.point))
            {

                transform.position = targetPosition;
                lineRenderer.SetPosition(0, transform.position);

                if (Vector3.Distance(lineRenderer.GetPosition(0), lineRenderer.GetPosition(1)) <= 1f) {

                    StopGrapplingHook();
                }
                else if (Input.GetMouseButtonUp(1) && isGrappling) {

                    StopGrapplingHook();
                    Debug.Log("Desengancha");
                }
            }
            else
            {

                StopGrapplingHook();
            }
        }
    }

    private void Dash()
    {

        if (Input.GetKeyDown("c") && !isDashing)
        {

            dashTarget = transform.position + transform.forward * dashDistance;
            isDashing = true;
            Debug.Log("DashTarget: " + dashTarget);
        }
    }

    private void GrapplingHook() { // todo esto debe ejecutarse una sola vez

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hitInfo, hookMaxDistance))
        {

            isGrappling = true;
            grapplingDirection = new Vector3(-transform.position.x,0f,0f);
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, hitInfo.point);


            hook.SetActive(true);
            hook.transform.position = lineRenderer.GetPosition(1);
            hook.transform.rotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
        }
    }

    private void StopGrapplingHook()
    {

        isGrappling = false;
        lineRenderer.enabled = false;
    }
}






