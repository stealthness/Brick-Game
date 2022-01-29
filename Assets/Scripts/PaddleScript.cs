using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleScript : MonoBehaviour
{

    private CharacterController characterController;

    private readonly float TOL = 0.0000001f;

    public float speed = 10f;
    public float rightScreenEdge;
    public float leftScreenEdge;
    public GameManager gm;
    public Transform explosion;
    private AudioSource clip;
    public float paddleSpeed = 0f;
    private float xPosition;

    // Start is called before the first frame update
    void Start()
    {
        paddleSpeed = 0f;
        clip = GetComponent<AudioSource>();
        xPosition = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        MovePaddle(Input.GetAxis("Horizontal"));
        paddleSpeed = (xPosition - transform.position.x)/ Time.deltaTime;
        xPosition = transform.position.x;

    }

    void MovePaddle(float horizontal)
    {
        if(gm.isGameOver){
            return;
        }
        if (Mathf.Abs(horizontal) > TOL)
        {
            transform.Translate(Vector2.right * horizontal * Time.deltaTime * speed);
            if (transform.position.x < leftScreenEdge)
            {
                transform.position = new Vector2(leftScreenEdge, transform.position.y);
            }
            if (transform.position.x > rightScreenEdge)
            {

                transform.position = new Vector2(rightScreenEdge, transform.position.y);
            }
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(string.Format("Paddle::OnTriggeredEnter2d with {0}", collision.name));

        if (collision.CompareTag("extraLife"))
        {
            //Debug.Log("power up hit");
            gm.addOneLife();
            Transform newExplosion = Instantiate(explosion, collision.transform.position, collision.transform.rotation);
            Destroy(newExplosion.gameObject, 1.2f);
            Destroy(collision.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        Debug.Log(string.Format("Paddle::OnCollision with {0}", collision.gameObject.name));

        if (collision.gameObject.CompareTag("ball"))
        {
            //Debug.Log("(2) paddle hit by ball");
            
            clip.Play();
        }
        if (collision.gameObject.CompareTag("extraLife"))
        {
            //Debug.Log("power up hit");
            gm.addOneLife();
            Transform newExplosion = Instantiate(explosion, collision.transform.position, collision.transform.rotation);
            Destroy(newExplosion.gameObject, 1.2f);
            Destroy(collision.gameObject);
        }
    }
}
