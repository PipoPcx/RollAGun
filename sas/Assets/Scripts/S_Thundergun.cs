using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class S_Thundergun : MonoBehaviour
{
    [SerializeField]
    private S_Controller controller;
    [SerializeField]
    private AudioSource shot;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private float fuerzaTelekinetica;

    private void Update()
    {

        if (Input.GetMouseButtonDown(0)) {


            animator.SetTrigger("IsShooting");
            shot.Play();

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("Kinetic"))
                {
                    Vector3 direccion = (hit.point - transform.position).normalized;
                    Quaternion rotacion = Quaternion.LookRotation(direccion);
                    Vector3 rotacionVector = rotacion.eulerAngles.normalized;

                    Rigidbody rb = hit.collider.GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        rb.AddForce(direccion * fuerzaTelekinetica, ForceMode.Impulse);
                    }
                }
            }

        }
    }
}
