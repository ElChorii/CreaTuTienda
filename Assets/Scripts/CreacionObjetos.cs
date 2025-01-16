using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreacionObjetos : MonoBehaviour
{
    [SerializeField]
    GameObject[] prefabs;  // Aquí arrastrarás todos tus prefabs en el Inspector
    private GameObject objetoSeleccionado = null;  // Objeto seleccionado para mover
    private GameObject objetoCreador = null;  // Referencia al objeto que estamos creando
    private bool objetoColocado = false;  // Controla si el objeto ya ha sido colocado
    private int objetoIndice = -1;  // Indica el índice del prefab que estamos utilizando para crear el objeto

    // Método para seleccionar qué prefab crear sin instanciarlo
    public void SeleccionarObjeto(int indice)
    {
        // Aseguramos que el índice es válido
        if (indice >= 0 && indice < prefabs.Length)
        {
            // Si ya existe un objeto creado y no ha sido colocado, lo destruimos
            if (objetoCreador != null && !objetoColocado)
            {
                Destroy(objetoCreador);  // Destruir el objeto creado si no ha sido colocado
            }

            // Si hay un objeto previamente seleccionado (de previsualización), lo destruimos
            if (objetoSeleccionado != null && !objetoColocado)
            {
                Destroy(objetoSeleccionado);  // Elimina el objeto anterior de previsualización solo si no se ha colocado
            }

            objetoIndice = indice;  // Guardamos el índice del prefab seleccionado
            IniciarCreacionDeObjeto();  // Iniciamos la creación del objeto
        }
    }

    public void Update()
    {
        // Si tenemos un objeto seleccionado, lo movemos con el cursor
        if (objetoSeleccionado != null)
        {
            SeguirCursor();  // Mueve el objeto seleccionando la posición del cursor

            // Si se hace clic, colocamos el objeto en la escena
            if (Input.GetMouseButtonDown(0))
            {
                ColocarObjetoConRaycast();  // Coloca el objeto donde se hace clic
            }
        }
    }

    // Función para mover el objeto seleccionando la posición del cursor
    private void SeguirCursor()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);  // Crea el rayo desde la posición del mouse
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            // Mueve el objeto seleccionado a la posición donde el raycast golpea una superficie
            objetoSeleccionado.transform.position = hit.point;
        }
    }

    // Función para colocar el objeto usando Raycast
    private void ColocarObjetoConRaycast()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);  // Crea el rayo desde la posición del mouse
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

            Destroy(objetoSeleccionado);  // Elimina el objeto temporal de previsualización
            objetoSeleccionado = null;  // Termina la selección del objeto
            objetoColocado = true;  // Marcamos que el objeto ha sido colocado
        }
    }
    // Función que será llamada cuando el jugador presiona un botón para crear un objeto
    private void IniciarCreacionDeObjeto()
    {
        // Solo iniciamos la creación si hemos seleccionado un prefab
        if (objetoIndice >= 0 && objetoIndice < prefabs.Length)
        {
            // Creamos el objeto de previsualización en una posición inicial
            objetoSeleccionado = Instantiate(prefabs[objetoIndice], Vector3.zero, Quaternion.identity);
            objetoSeleccionado.SetActive(true);  // Activamos el objeto de previsualización

            objetoColocado = false;  // Aseguramos que aún no se ha colocado el objeto
        }
    }
}
