using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed;
    /*
    Los límites definidos con bound nos hacen falta debido a que el jugador se puede salir de la pantalla
    debido a que su rigidbody es quinemático, por lo que no se ve afectado por la gravedad ni puede colisionar
    con objetos estáticos.
    */
    [SerializeField] private float bound = 4.5f; // x axis bound 

    private Vector2 startPos; // Posición inicial del jugador
    private Vector2 startSize;
    private float defaultBound;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position; // Guardamos la posición inicial del jugador
        startSize = transform.localScale;
        defaultBound = bound;
    }

    // Update is called once per frame
    void Update()
    {
       PlayerMovement();
    }

    void PlayerMovement()
    {
         float moveInput = Input.GetAxisRaw("Horizontal");
        // Controlaríamo el movimiento de la siguiente forma de no ser el rigidbody quinemático
        // transform.position += new Vector3(moveInput * speed * Time.deltaTime, 0f, 0f);

        Vector2 playerPosition = transform.position;
        // Mathf.Clamp nos permite limitar un valor entre un mínimo y un máximo
        playerPosition.x = Mathf.Clamp(playerPosition.x + moveInput * speed * Time.deltaTime, -bound, bound);
        transform.position = playerPosition;
    }

    public void ResetPlayer()
    {
        transform.position = startPos; // Posición inicial del jugador
        RemovePowerUp();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("powerUp")) // Si colisionamos con un powerUp
        {
            Destroy(collision.gameObject); // Lo destruimos
            GameManager.Instance.AddLife(); // Añadimos una vida
        }
        else if (collision.CompareTag("powerUpSize"))
        {
            RemovePowerUp();
            StopCoroutine(DoubleSize());
            StartCoroutine(DoubleSize());
        }
        else if (collision.CompareTag("powerDown")) // Si colisionamos con un powerUp
        {
            Destroy(collision.gameObject); // Lo destruimos
            GameManager.Instance.LoseLifePowerDown(); // Añadimos una vida
        }
    }

    private IEnumerator DoubleSize()
    {
        transform.localScale = new Vector2(startSize.x * 2, startSize.y);
        FindObjectOfType<Ball>().Resize();
        bound -= 0.7f;
        yield return new WaitForSeconds(10f);
        RemovePowerUp();
    }

    private void RemovePowerUp()
    {
        transform.localScale = startSize;
        FindObjectOfType<Ball>().Resize();
        bound = defaultBound;
    }
}
