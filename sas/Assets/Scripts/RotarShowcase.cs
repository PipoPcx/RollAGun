using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RotarShowcase : MonoBehaviour
{
    [SerializeField]
    public float rotate;
    

    // Update is called once per frame
    void Update() {

        gameObject.transform.Rotate(0, rotate, 0, Space.Self);
    }
}
