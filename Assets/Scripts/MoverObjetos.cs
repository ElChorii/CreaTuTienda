using UnityEngine;

public class MoverObjetos : MonoBehaviour
{
    //mover objeto
    public bool modoMover = false;  // Variable que controla si el modo de mover está activado o no
    private GameObject objetoSeleccionado = null;  // El objeto actualmente seleccionado para mover
    private Vector3 posicionOriginal;  // Guardamos la posición original del objeto antes de moverlo
    private bool siguiendoCursor = false;  // Controla si el objeto sigue al cursor o no
    public GameObject circulito;

    //escalar objeto
    public bool sePuedeEscalar = false;
    public bool seEstaEscalando = false;
    private GameObject objetoParaEscalar = null; // El objeto cuya escala queremos modificar
    public float scaleSpeed = 1f; // Velocidad de cambio de escala
    public Vector3 minScale = new Vector3(0.5f, 0.5f, 0.5f); // Escala mínima
    public Vector3 maxScale = new Vector3(2f, 2f, 2f); // Escala máxima

    private Vector3 previousMousePosition;

    //circulito
    bool laPrimeraEstaX = false;
    bool laPrimeraEstaZ = false;
    bool sePuedeRepetir = true;

    private void Start()
    {
        previousMousePosition = Input.mousePosition;
    }

    void Update()
    {

        if (modoMover == true)
        {   
            //Animacion del circulo
            if (circulito.activeSelf)
            {
                //movimiento del circulito azul

                if (sePuedeRepetir == true)
                {
                    sePuedeRepetir = false;
                    LeanTween.scaleX(circulito, 1f, 1f).setEase(LeanTweenType.easeInOutQuad).setOnComplete(() =>
                    {
                        LeanTween.scaleX(circulito, 2f, 1f).setEase(LeanTweenType.easeInOutQuad).setOnComplete(() =>
                        {
                            laPrimeraEstaX = true;
                        });
                    });
                    LeanTween.scaleZ(circulito, 1f, 1f).setEase(LeanTweenType.easeInOutQuad).setOnComplete(() =>
                    {
                        LeanTween.scaleZ(circulito, 2f, 1f).setEase(LeanTweenType.easeInOutQuad).setOnComplete(() =>
                        {
                            laPrimeraEstaZ = true;
                        });
                    });
                }
                //comprobar si la animacion ha finalizado para que vuelva a comenzar
                if (laPrimeraEstaX == true && laPrimeraEstaZ == true)
                {
                    laPrimeraEstaX = false;
                    laPrimeraEstaZ = false;
                    sePuedeRepetir = true;
                }
            }
            // Si el modoMover está activo y hay un objeto seleccionado, se puede mover
            if (objetoSeleccionado != null && siguiendoCursor)
            {
                MoverObjetoConCursor();  // Llama a la función para mover el objeto con el cursor
            }

            // Al hacer clic, seleccionamos un objeto si no hay ninguno seleccionado
            if (Input.GetMouseButtonDown(0))
            {
                SeleccionarObjetoConRaycast();  // Llama a la función para seleccionar un objeto
            }

            // Cuando se suelta el clic, se coloca el objeto en la nueva posición
            if (Input.GetMouseButtonUp(0) && objetoSeleccionado != null)
            {
                ColocarObjetoConRaycast();  // Llama a la función para colocar el objeto
            }
        }  
        if (sePuedeEscalar == true)
        {
            // Al hacer clic, seleccionamos un objeto si no hay ninguno seleccionado
            if (Input.GetMouseButtonDown(0))
            {
                SeleccionarObjetoConRaycastParaEscalar();  // Llama a la función para seleccionar un objeto
                
            }
            if (seEstaEscalando == true)
            {
                // Obtener la posición actual del mouse
                Vector3 currentMousePosition = Input.mousePosition;

                // Calcular el cambio en el eje Y
                float mouseDeltaY = currentMousePosition.y - previousMousePosition.y;

                // Ajustar la escala del objeto basado en el cambio en Y
                if (mouseDeltaY != 0)
                {
                    Vector3 newScale = objetoParaEscalar.transform.localScale;

                    // Modificar la escala proporcionalmente al movimiento del mouse
                    newScale += (Vector3.one * mouseDeltaY * scaleSpeed) * 8 * Time.deltaTime;

                    // Limitar la escala al rango definido
                    newScale = new Vector3(
                        Mathf.Clamp(newScale.x, minScale.x, maxScale.x),
                        Mathf.Clamp(newScale.y, minScale.y, maxScale.y),
                        Mathf.Clamp(newScale.z, minScale.z, maxScale.z)
                    );

                    // Aplicar la nueva escala
                    objetoParaEscalar.transform.localScale = newScale;
                }

                // Actualizar la posición del mouse
                previousMousePosition = currentMousePosition;

                if (Input.GetMouseButtonUp(0))
                {
                    seEstaEscalando = false;
                }
            }
        }
    }

    // Función que mueve el objeto seleccionado con el cursor
    private void MoverObjetoConCursor()
    {
        // Desactivamos el objeto para que el raycast lo atraviese
        objetoSeleccionado.SetActive(false);
        circulito.SetActive(false);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit) && (modoMover == true))
        {
            // Mueve el objeto a la posición donde golpea el rayo
            objetoSeleccionado.transform.position = hit.point;
            circulito.transform.position = hit.point;
        }

        // Reactivamos el objeto para que se vea en la nueva posición
        objetoSeleccionado.SetActive(true);
        circulito.SetActive(true);
    }

    // Función que selecciona un objeto si el clic fue sobre un objeto cuyo nombre contenga "(Clone)"
    private void SeleccionarObjetoConRaycast()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider != null && hit.collider.gameObject.name.Contains("(Clone)"))  // Comprobamos si el nombre contiene "(Clone)"
            {
                objetoSeleccionado = hit.collider.gameObject;  // Selecciona el objeto cuyo nombre contiene "(Clone)"
                posicionOriginal = objetoSeleccionado.transform.position;  // Guardamos la posición original
                siguiendoCursor = true;  // El objeto comienza a seguir el cursor
            }
        }
    }
    private void SeleccionarObjetoConRaycastParaEscalar()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider != null && hit.collider.gameObject.name.Contains("(Clone)")) // Comprobamos si el nombre contiene "(Clone)"
            {
                objetoParaEscalar = hit.collider.gameObject;
                seEstaEscalando = true;
            }
        }
    }
        
    // Función para colocar el objeto cuando el clic se suelta
    private void ColocarObjetoConRaycast()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            objetoSeleccionado.transform.position = hit.point;  // Coloca el objeto en la nueva posición
            siguiendoCursor = false;  // El objeto deja de seguir el cursor
            objetoSeleccionado = null;
        }
        
    }

    // Esta función será llamada desde el botón en Unity para activar/desactivar el modoMover
    public void ToggleModoMover()
    {
        modoMover = true;  // Actuva el valor de modoMover)
        if (modoMover == false)
        {
            circulito.SetActive(false);
        }
    }
    public void DesactivarModoMover()
    {
        modoMover = false;
    }

    public void ModoEscalar()
    {
        DesactivarModoMover();
        sePuedeEscalar = true;
        seEstaEscalando = false;
    }
    public void DesactivarModoEscalar()
    {
        sePuedeEscalar = false;
    }
}