using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gegner : MonoBehaviour
{
    public Rigidbody2D rb2D;
    public float angle;

    // Start is called before the first frame update
    void Start()
    {
        angle = 180;
        rb2D.velocity = new Vector2(3.0f, rb2D.velocity.y);
        transform.rotation = Quaternion.Euler(0, angle, 0);
        Invoke("RevertMovement", 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void RevertMovement()
    {
        angle += 180;
        rb2D.velocity = rb2D.velocity * -1;
        transform.rotation = Quaternion.Euler(0, angle, 0);
        Invoke("RevertMovement", 1.0f);
    }
}
