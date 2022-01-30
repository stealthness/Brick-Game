using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour
{

    public GameManager gm;
    public Rigidbody2D rb;
    private AudioSource ballBounceClip;
    [SerializeField] float FORCE_MULTIPLER = 500;
    [SerializeField] Transform paddle;
    [SerializeField] Transform explosion;
    public Transform extraLifePowerUP;
    public Vector2 BALL_START_POSITION = new Vector2(0f, -2f);

    
    public float ps;
    public float boosterSpeed = 1f;

    private Vector2 savedVelocity; 
    private Vector2 savedForce;

    private bool reset = true;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ballBounceClip = GetComponent<AudioSource>();
        
        ResetBall();

    }

    // Update is called once per frame
    void Update()
    {
        switch (gm.gameState)
        {
            case GameState.paused:
                break;
            case GameState.ended:
                Time.timeScale = 0f;
                ResetBall();
                return;
            case GameState.firstTime:
                Time.timeScale = 1f;
                if (Input.GetButtonDown("Fire3") || Input.GetButtonDown("Jump"))
                {
                    Debug.Log("Left Shift pressed");
                    StartBall();
                }
                break;
            case GameState.playing:
                Time.timeScale = 1f;
                savedVelocity = rb.velocity;
                break;
        }

    }

    private void StartBall()
    {
        rb.AddForce(Vector2.up * FORCE_MULTIPLER);
        gm.gameState = GameState.playing;
    }

    public void ResetBall()
    {
        rb.velocity = Vector2.zero;
        transform.position = paddle.position + Vector3.up * 0.2f;
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

            ballBounceClip.Play();

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
            ballBounceClip.Play();

        }
        if (collision.gameObject.CompareTag("paddle"))
        {
            Debug.Log(string.Format("BallScript::OnCollisionEnterd -> paddle hit"));
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
        if (collision.gameObject.CompareTag("bottom"))
        {
            Debug.Log(string.Format("BallScript::OnCollisionEnterd -> bottom"));
            gm.gameState = GameState.firstTime;
            gm.RemoveLife();
            ResetBall();
        }


    }

    public void PaddleHit(float paddleSpeed)
    {
        if (System.Math.Abs(ps) > 0.1f)
        {
            rb.velocity = new Vector2(rb.velocity.x, - rb.velocity.y);

        }
        else
        {
            rb.velocity = new Vector2(rb.velocity.x, -rb.velocity.y);
        }
    }

    public void MirrorPaddlePosition(Vector3 paddlePos)
    {
        transform.position = paddlePos + Vector3.up * 0.2f;
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
