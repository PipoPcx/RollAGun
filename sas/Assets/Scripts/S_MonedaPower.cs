using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_MonedaPower : MonoBehaviour
{
    public GameManager gameManager;

    private void Start() {

        gameManager.OnPowerSelection += gameManager.HandlePowerSelection;
    }

    private void OnDestroy() {

        gameManager.OnPowerSelection -= gameManager.HandlePowerSelection;
    }
}
