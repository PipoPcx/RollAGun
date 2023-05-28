using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    public Animator coinAnimator;

    private void Awake()
    {
        if (instance == null)
        {

            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {

            Destroy(gameObject);
        }
    }

    #region WeaponSelector Methods
    public enum WeaponChoice {

        ThunderGun,
        Shotgun
    }

    public WeaponChoice chosenWeapon;

    public event Action<WeaponChoice> OnWeaponSelection;

    public void ChooseRandomWeapon()
    {

        chosenWeapon = (UnityEngine.Random.value < .5f) ? WeaponChoice.Shotgun : WeaponChoice.ThunderGun;
        Debug.Log(chosenWeapon);

        OnWeaponSelection?.Invoke(chosenWeapon);
    }

    public void ActivateWeapon(WeaponChoice chosenWeapon)
    {

        GameObject player = GameObject.FindWithTag("Player");
        if (player != null) { Debug.Log("Encontró player"); }

        Transform joint = player.transform.Find("Joint");
        if (joint != null) { Debug.Log("Encontró joint"); }

        Transform playerCamera = joint.Find("PlayerCamera");
        if (playerCamera != null) { Debug.Log("Encontró playerCamera"); }

        Transform thunderGun = playerCamera.Find("ThunderGun");
        if (thunderGun != null) { Debug.Log("Encontró thunderGun"); }

        Transform shotgun = playerCamera.Find("Shotgun");
        if (shotgun != null) { Debug.Log("Encotró Shotgun"); }

        switch (chosenWeapon)
        {

            case WeaponChoice.ThunderGun:
                thunderGun.gameObject.SetActive(true);
                if (thunderGun.gameObject.activeSelf) { Debug.Log("thundeGun activada"); }

                shotgun.gameObject.SetActive(false);
                break;

            case WeaponChoice.Shotgun:
                shotgun.gameObject.SetActive(true);
                if (shotgun.gameObject.activeSelf) { Debug.Log("shotgun activada"); }


                thunderGun.gameObject.SetActive(false);
                break;
        }
    }

    public void HandleWeaponSelection(GameManager.WeaponChoice chosenWeapon)
    {

        if (chosenWeapon == GameManager.WeaponChoice.Shotgun)
        {

            coinAnimator.SetTrigger("isS");
            StartCoroutine(CambiarEscena());
        }

        else if (chosenWeapon == GameManager.WeaponChoice.ThunderGun)
        {

            coinAnimator.SetTrigger("isT");
            StartCoroutine(CambiarEscena());
        }

    }

    IEnumerator CambiarEscena()
    {

        yield return new WaitForSeconds(5);
        SceneManager.LoadScene("Demo");

        string activeSceneName = SceneManager.GetActiveScene().name;
        Debug.Log(activeSceneName);

        if (SceneManager.GetActiveScene().name == "Demo")
        {

            GameManager.instance.ActivateWeapon(GameManager.instance.chosenWeapon);
        }
    } 
    #endregion

}
    
