using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class S_gun : MonoBehaviour
{
    #region Variables

    [Header("GameObjects")]

    [SerializeField]
    private GameObject balaPrefab;

    [SerializeField]
    private GameObject pivot;

    [Header("Bala")]

    [SerializeField]
    private float fuerzaBala = 5f;

    [SerializeField]
    public float municion = 4f;

    [Header("CD")]

    [SerializeField]
    private float waitBeforeShootAgain = 1f;

    [SerializeField]
    private float lastTimeShot;

    [SerializeField]
    private float timeBeforeDestroy = 1f;

    public S_Controller controller;

    #endregion

    #region Voids
    private void Update()
    {
        if (Input.GetMouseButton(0) && Time.time > lastTimeShot + waitBeforeShootAgain)
        {
            lastTimeShot = Time.time;

            Disparar();
        }
    }
    #endregion

    #region Métodos


    public void Disparar()
    {

        GameObject bala = Instantiate(balaPrefab, pivot.transform.position, pivot.transform.rotation);
        Rigidbody rb = bala.GetComponent<Rigidbody>();
        rb.AddForce(pivot.transform.forward * fuerzaBala, ForceMode.Impulse);
        controller.RocketJump();
    }

    #endregion
}
    
