using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControladorDeIU : MonoBehaviour
{
    public GameObject menuBoton;
    public GameObject menuAcciones;
    public GameObject menuObjetos;

    private void Start()
    {
        menuBoton.SetActive(true);
        menuAcciones.SetActive(false);
        menuObjetos.SetActive(false);
    }

    public void AccionarBotonMenu()
    {
        menuBoton.SetActive(false);
        menuAcciones.SetActive(true);
        LeanTween.moveLocalY(menuAcciones, -700f, 0);
        LeanTween.moveLocalY(menuAcciones, 0f, 0.5f).setEase(LeanTweenType.easeOutBack);
    }
    public void AccionarConstruir()
    {
        menuObjetos.SetActive(true);
        LeanTween.moveLocalX(menuObjetos, 1000f, 0);
        LeanTween.moveLocalX(menuObjetos, 465f, 0.5f).setEase(LeanTweenType.easeOutBack);
    }
    public void AccionarAtrasObjetos()
    {
        LeanTween.moveLocalX(menuObjetos, 1000f, 1f).setEase(LeanTweenType.easeInOutBack).setOnComplete(() =>
        {
            menuObjetos.SetActive(false);
        });          
    }
    public void AccionarAtrasAcciones()
    {
        menuBoton.SetActive(true);
        LeanTween.moveLocalY(menuBoton, -700f, 0);
        LeanTween.moveLocalY(menuBoton, -250f, 1f).setEase(LeanTweenType.easeOutBack);
        LeanTween.moveLocalX(menuObjetos, 1000f, 1f).setEase(LeanTweenType.easeInOutBack).setOnComplete(() =>
        {
            menuObjetos.SetActive(false);
        });
        LeanTween.moveLocalY(menuAcciones, -700f, 1f).setEase(LeanTweenType.easeInOutBack).setOnComplete(() =>
        {
            menuAcciones.SetActive(false);
        });
    }
    public void AccionarSubirMenu()
    {
        LeanTween.moveLocalY(menuObjetos, -180f, 0);
        LeanTween.moveLocalY(menuObjetos, 470f, 0.5f).setEase(LeanTweenType.easeOutBack);
    }
    public void AccionarBajarMenu()
    {
        LeanTween.moveLocalY(menuObjetos, -180f, 0.5f).setEase(LeanTweenType.easeOutBack);
    }
    public void SeActivaElModoMover()
    {
        LeanTween.moveLocalX(menuObjetos, 1000f, 1f).setEase(LeanTweenType.easeInOutBack).setOnComplete(() =>
        {
            menuObjetos.SetActive(false);
        });
    }
}
