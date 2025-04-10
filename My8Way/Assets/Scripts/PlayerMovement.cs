using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    public float speed;
    AudioSource audio;

    private GameManager gameManager;
    private SpriteRenderer spriteRenderer;

    public AudioSource explosionAudio;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audio = GetComponent<AudioSource>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector2 movement = new Vector2(moveHorizontal, moveVertical);
        rb.MovePosition(rb.position + movement.normalized * speed * Time.fixedDeltaTime);

        if (moveHorizontal > 0.01f)
        {
            transform.localScale = new Vector3(0.3f, 0.3f, 1f);
        }
        else if (moveHorizontal < -0.01f)
        {
            transform.localScale = new Vector3(-0.3f, 0.3f, 1f);
        }
    }

    IEnumerator BlinkEffect(int blinks, float blinkDuration)
    {
        for (int i = 0; i < blinks; i++)
        {
            spriteRenderer.enabled = false;
            yield return new WaitForSeconds(blinkDuration);
            spriteRenderer.enabled = true;
            yield return new WaitForSeconds(blinkDuration);
        }
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Colisão detectada com: " + other.gameObject.name);
        
        if (other.tag == "Coletavel")
        {
            audio.Play();
            Destroy(other.gameObject);
            gameManager.AddScore(1);
            gameManager.SpawnColetavel();

            Debug.Log("Coletável coletado, tentando recarregar combustível...");
            FuelManager.instance.RefillFuel(0.3f);
        }

        if (other.tag == "Enemy")
        {
            if (explosionAudio != null)
                {
                    explosionAudio.Play();
                }
            StartCoroutine(BlinkEffect(2, 0.1f)); 
            Debug.Log("Colisão com inimigo! Reduzindo combustível em 20%.");
            FuelManager.instance.ReduceFuelByPercent(0.2f); // Reduz 20% do combustível
            Destroy(other.gameObject);
            gameManager.OnEnemyDestroyed();
        }
    }
}
