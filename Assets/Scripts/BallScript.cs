using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour
{

    [SerializeField] float FORCE_MULTIPLER = 500;
    [SerializeField] Transform paddle;
    [SerializeField] Transform explosion;
    public Transform extraLifePowerUP;
    public GameManager gm;
    public Rigidbody2D rb;
    private bool inPlay;
    public Vector2 BALL_START_POSITION = new Vector2(0f, -2f);

    private AudioSource clip;
    public float ps;
    public float boosterSpeed = 1f;

    private Vector2 savedVelocity; 
    private Vector2 savedForce;
    private bool newPause;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        clip = GetComponent<AudioSource>();
        ResetBall();
        newPause = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (gm.gameState == GameState.ended)
        {
            ResetBall();          
            return;
        }  

        if (inPlay)
        {
            if (gm.gameState != GameState.paused && !newPause)
            {
                savedVelocity = rb.velocity;
            }

            if (gm.gameState == GameState.paused) // pause the ball velocity
            {
                if (!newPause)
                {
                    rb.velocity = Vector2.zero;
                    newPause = true;
                }
                return;
            }

            if (gm.gameState != GameState.playing && newPause) // resumethe ball velocity
            {
                rb.velocity = savedVelocity;
                newPause = false;
            }
        }
        



        if (!inPlay)
        {
            transform.position = paddle.position;
            if (Input.GetButtonDown("Jump"))
            {
                gm.gameState = GameState.playing;
                StartBall();
            }
        }

        if (inPlay && Input.GetButtonDown("Fire3"))
        {
            Debug.Log("Left Shift pressed");
        }
    }

    private void StartBall()
    {
        rb.AddForce(Vector2.up * FORCE_MULTIPLER);
        inPlay = true;
    }

    public void ResetBall()
    {
        rb.velocity = Vector2.zero;
        inPlay = false;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("bottom"))
        {           
            //Debug.Log("bottom reached");
            gm.RemoveLife();
            ResetBall();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("brick"))
        {
            //Debug.Log("Brick Hit");

            BrickScript brickScript = collision.gameObject.GetComponent<BrickScript>();

            clip.Play();

            if (brickScript.hitsToBreak > 1)
            {
                gm.AddScore(brickScript.points);
                brickScript.BreakBrick();
            }
            else // destroy brick
            {                
                // when brick is destroyed check if power is released
                if (gm.gameState == GameState.ended)
                {
                    GeneratePowerUp(brickScript.powerUp, collision);
                }

                gm.AddScore(brickScript.points);
                gm.RemoveOneBricks();
                Transform newExplosion = Instantiate(explosion, collision.transform.position, collision.transform.rotation);
                Destroy(newExplosion.gameObject, 3.0f);
                Destroy(collision.gameObject);
                if (gm.gameState == GameState.nextLevel)
                {
                    gm.NextLevel();
                }
            }
        }
        if (collision.gameObject.CompareTag("edge"))
        {
            //Debug.Log("(2)side reached");
            collision.gameObject.GetComponent<AudioSource>().Play();

        }
        if (collision.gameObject.CompareTag("paddle"))
        {
            ps = collision.gameObject.GetComponent<PaddleScript>().paddleSpeed;
            Debug.Log(string.Format("ps: {0}", ps));

            if (System.Math.Abs(ps) > 0.1f)
            {
                rb.velocity += new Vector2(ps * boosterSpeed * Time.deltaTime, 0f);
                
            }
            else
            {
                //rb.AddForce(new Vector2(0f, boosterSpeed));
            }

            
        }

    }


    private void GeneratePowerUp(string powerUp, Collision2D collision) {

        //Debug.Log(string.Format("To Do: Power Up is {0}", powerUp));
        if (powerUp.Equals("none"))
        {
            //Debug.Log("No Power inside");
            return;
        }

        if (powerUp.Equals("extraLife"))
        {
            //Debug.Log("Extralife inside");
            Instantiate(extraLifePowerUP, collision.transform.position, collision.transform.rotation);
        }
        else
        {
            //Debug.Log("To Do");
        }
    }
}
