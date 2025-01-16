using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreacionObjetos : MonoBehaviour
{
    [SerializeField]
    GameObject[] prefabs;  // Aqu� arrastrar�s todos tus prefabs en el Inspector
    private GameObject objetoSeleccionado = null;  // Objeto seleccionado para mover
    private GameObject objetoCreador = null;  // Referencia al objeto que estamos creando
    private bool objetoColocado = false;  // Controla si el objeto ya ha sido colocado
    private int objetoIndice = -1;  // Indica el �ndice del prefab que estamos utilizando para crear el objeto

    // M�todo para seleccionar qu� prefab crear sin instanciarlo
    public void SeleccionarObjeto(int indice)
    {
        // Aseguramos que el �ndice es v�lido
        if (indice >= 0 && indice < prefabs.Length)
        {
            // Si ya existe un objeto creado y no ha sido colocado, lo destruimos
            if (objetoCreador != null && !objetoColocado)
            {
                Destroy(objetoCreador);  // Destruir el objeto creado si no ha sido colocado
            }

            // Si hay un objeto previamente seleccionado (de previsualizaci�n), lo destruimos
            if (objetoSeleccionado != null && !objetoColocado)
            {
                Destroy(objetoSeleccionado);  // Elimina el objeto anterior de previsualizaci�n solo si no se ha colocado
            }

            objetoIndice = indice;  // Guardamos el �ndice del prefab seleccionado
            IniciarCreacionDeObjeto();  // Iniciamos la creaci�n del objeto
        }
    }

    public void Update()
    {
        // Si tenemos un objeto seleccionado, lo movemos con el cursor
        if (objetoSeleccionado != null)
        {
            SeguirCursor();  // Mueve el objeto seleccionando la posici�n del cursor

            // Si se hace clic, colocamos el objeto en la escena
            if (Input.GetMouseButtonDown(0))
            {
                ColocarObjetoConRaycast();  // Coloca el objeto donde se hace clic
            }
        }
    }

    // Funci�n para mover el objeto seleccionando la posici�n del cursor
    private void SeguirCursor()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);  // Crea el rayo desde la posici�n del mouse
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            // Mueve el objeto seleccionado a la posici�n donde el raycast golpea una superficie
            objetoSeleccionado.transform.position = hit.point;
        }
    }

    // Funci�n para colocar el objeto usando Raycast
    private void ColocarObjetoConRaycast()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);  // Crea el rayo desde la posici�n del mouse
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            // Solo creamos el objeto cuando lo colocamos, no antes
            objetoCreador = Instantiate(prefabs[objetoIndice], hit.point, Quaternion.identity);

            // Asegurarse de que el objeto creado tiene un BoxCollider (si no lo tiene, lo agregamos)
            if (objetoCreador.GetComponent<BoxCollider>() == null)
            {
                objetoCreador.AddComponent<BoxCollider>();  // Agregar un BoxCollider si no lo tiene
            }

            Destroy(objetoSeleccionado);  // Elimina el objeto temporal de previsualizaci�n
            objetoSeleccionado = null;  // Termina la selecci�n del objeto
            objetoColocado = true;  // Marcamos que el objeto ha sido colocado
        }
    }
    // Funci�n que ser� llamada cuando el jugador presiona un bot�n para crear un objeto
    private void IniciarCreacionDeObjeto()
    {
        // Solo iniciamos la creaci�n si hemos seleccionado un prefab
        if (objetoIndice >= 0 && objetoIndice < prefabs.Length)
        {
            // Creamos el objeto de previsualizaci�n en una posici�n inicial
            objetoSeleccionado = Instantiate(prefabs[objetoIndice], Vector3.zero, Quaternion.identity);
            objetoSeleccionado.SetActive(true);  // Activamos el objeto de previsualizaci�n

            objetoColocado = false;  // Aseguramos que a�n no se ha colocado el objeto
        }
    }
}
