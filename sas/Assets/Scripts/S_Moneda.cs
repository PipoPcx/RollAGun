using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static GameManager;

public class S_Moneda : MonoBehaviour
{
    public GameManager gameManager;

    private void Start() {

        gameManager.OnWeaponSelection += gameManager.HandleWeaponSelection;
    }

    private void OnDestroy() {

        gameManager.OnWeaponSelection -= gameManager.HandleWeaponSelection;
    }

   
}
