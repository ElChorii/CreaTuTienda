using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotarObjetos : MonoBehaviour
{
    public bool modoRotar = false;  // Activar/desactivar modo rotar
    public GameObject objetoSeleccionado = null;  // El objeto seleccionado que queremos rotar
    public MoverObjetos moverScript;  // Referencia al script de mover, para desactivar el modo mover
    public bool modoDestruir = false;

    void Update()
    {
        // Si el modo rotar está activo y se hace clic
        if (modoRotar && Input.GetMouseButtonUp(0))
        {
            // Detectar si el raycast toca un objeto
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // Verificar si el objeto tocado tiene "(Clone)" en su nombre (para asegurarnos de que es un clon)
                if (hit.collider.gameObject.name.Contains("(Clone)"))
                {
                    objetoSeleccionado = hit.collider.gameObject;
                    RotarObjeto();  // Rotar el objeto
                }
            }
        }
        if (modoDestruir && Input.GetMouseButtonUp(0))
        {
            // Detectar si el raycast toca un objeto
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // Verificar si el objeto tocado tiene "(Clone)" en su nombre (para asegurarnos de que es un clon)
                if (hit.collider.gameObject.name.Contains("(Clone)"))
                {
                    objetoSeleccionado = hit.collider.gameObject;
                    EliminarObjeto();  // Eliminar el objeto
                }
            }
        }
    }

    // Función para rotar el objeto 90 grados en el eje Y
    void RotarObjeto()
    {
        if (objetoSeleccionado != null)
        {
            // Realizamos la rotación de 90 grados
            objetoSeleccionado.transform.Rotate(0, 90, 0);

            // Opcionalmente, podemos imprimir la rotación actual si es necesario
            Debug.Log("Objeto rotado a: " + objetoSeleccionado.transform.rotation.eulerAngles);
        }
    }
    void EliminarObjeto()
    {
        if (objetoSeleccionado != null)
        {
            objetoSeleccionado.SetActive(false);
        }
    }

    // Activar el modo rotar y desactivar el modo mover
    public void ActivarModoRotar()
    {
        modoRotar = true;
        moverScript.modoMover = false;  // Desactivamos el modo mover en el script de mover
    }

    // Desactivar el modo rotar
    public void DesactivarModoRotar()
    {
        modoRotar = false;
    }
    public void ActivarModoDestruir()
    {
        modoDestruir = true;
    }
    public void DesactivarModoDestruir()
    {
        modoDestruir = false;
    }
}
