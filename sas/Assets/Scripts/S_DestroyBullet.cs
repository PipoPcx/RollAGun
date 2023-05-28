using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class S_DestroyBullet : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        Invoke("Destruir", 0.5f);
    }

    private void Destruir() {

        Destroy(gameObject);
    }

    public void OnTriggerEnter(Collider collision) {
        
        if(collision.gameObject.CompareTag("Player")) { 
        
            Vector3 explosionCenter = transform.position;
            float explosionRadio = 5f;

            if(Physics.CheckSphere(explosionCenter, explosionRadio)) {

                
            }
        }
    }

}
