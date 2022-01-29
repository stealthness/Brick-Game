using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpScript : MonoBehaviour
{
    private readonly float DROP_SPEED = -1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector2(0f, DROP_SPEED) * Time.deltaTime);

        // destroys the power up if it drops of the end of the screen
        if (transform.position.y < -6f)
        {
            Destroy(gameObject);
        }

    }
}
