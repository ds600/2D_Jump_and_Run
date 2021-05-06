using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class Spieler : MonoBehaviour
{
    public Rigidbody2D rb2D;
    public float thrust;
    public bool jumped = false;
    public int leben;

    public float coinPosition;
    public GameObject blood;
    public Vector3 startPosition;
    public GameObject coin;
    public Text infoAnzeige;

    public Text zeit;
    public Text besteZeit;
    public float zeitStart;

    public AudioSource musik;

    public GameObject main_camera;
    public AudioClip jump;
    public AudioSource coinSound;
    public AudioSource m_MyAudioSource;
    public Animator animator;
    bool tutorialVorbei = false;

    // Start is called before the first frame update
    void Start()
    {
        besteZeit.text = string.Format("Beste Zeit: {0,6:0.0} sec.", PlayerPrefs.GetFloat("BestTime"));
        zeitStart = Time.time;

        if (!PlayerPrefs.HasKey("Lautstaerke"))
        {
            PlayerPrefs.SetFloat("Lautstaerke", 0.05f);
        }
        musik.volume = PlayerPrefs.GetFloat("Lautstaerke");

        if (!PlayerPrefs.HasKey("BestTime"))
        {
            PlayerPrefs.SetFloat("BestTime", 0.0f);
        }

        infoAnzeige.text = "W A D zum Bewegen. Der Sprung erneuert sich, wenn man den Boden\r\noder eine Platform bei über 50% Höhe an der Seite berührt. \r\n Sammel alle Münzen um zu gewinnen!";

        coinSound.volume = 0.2f;
        leben = 3;

        coinPosition = -4f;
        coin.transform.position = new Vector3(Random.Range(-6.0f, 6.0f), coinPosition, 0);

        m_MyAudioSource.volume = 0.1f;
        rb2D = this.GetComponent<Rigidbody2D>();

        startPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);

    }

    // Update is called once per frame
    void Update()
    {
        zeit.text = string.Format("zeit: {0,6:0.0} sec.", Time.time - zeitStart);

        animator.SetFloat("velocity.y", rb2D.velocity.y);

        if (Input.GetKeyDown(KeyCode.W) && !jumped)
        {
            m_MyAudioSource.Play();
            rb2D.velocity = new Vector2(rb2D.velocity.x, 6.0f);
            jumped = true;
            if (!tutorialVorbei)
            {
                tutorialVorbei = true;
                infoAnzeige.text = "";
            }
        }

        if (Input.GetKey(KeyCode.D))
        {
            rb2D.velocity = new Vector2(3.0f, rb2D.velocity.y);
            if (!tutorialVorbei)
            {
                tutorialVorbei = true;
                infoAnzeige.text = "";
            }
        }
        else if (Input.GetKey(KeyCode.A))
        {
            rb2D.velocity = new Vector2(-3.0f, rb2D.velocity.y);
            if (!tutorialVorbei)
            {
                tutorialVorbei = true;
                infoAnzeige.text = "";
            }
        }
        else
        {
            rb2D.velocity = new Vector2(0.0f, rb2D.velocity.y);
        }

    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if(coll.gameObject.tag == "Boden" && jumped)
        {
            animator.SetBool("Landed", true);
            Invoke("SetLanded", 0.5f);
        }

        if (coll.gameObject.tag == "Boden" && coll.gameObject.transform.position.y < transform.position.y + 0.158)
        {
            jumped = false;
        }

        if (coll.gameObject.tag == "Gegner")
        {
            
            Instantiate(blood, transform.position, Quaternion.identity);
            transform.position = startPosition;
            leben--;
            if (leben != 0)
            {
                infoAnzeige.text = "Du hast noch " + leben + " Leben";
                Invoke("infoAnzeigeLeeren", 2);
            } else
            {
                infoAnzeige.text = "Du hast verloren :(";
                gameObject.SetActive(false);
            }
            

        }

    }

    void OnTriggerEnter2D(Collider2D trigger)
    {
        if (coinPosition < 2)
        {
            if (trigger.gameObject.tag == "Coin")
            {
                coinSound.Play();
                coin.transform.position = new Vector3(Random.Range(-6.0f, 6.0f), coinPosition, 0);
                coinPosition++;
            }

        } else
        {
            if (coin.transform.position.y == 3.5f && trigger.gameObject.tag == "Coin")
            {
                coinSound.Play();
                coin.SetActive(false);
                infoAnzeige.text = "Du hast gewonnen! :)";
                if (PlayerPrefs.GetFloat("BestTime") == 0.0f || PlayerPrefs.GetFloat("BestTime") > Time.time - zeitStart)
                {
                    PlayerPrefs.SetFloat("BestTime", Time.time - zeitStart);
                }
                besteZeit.text = string.Format("Beste Zeit: {0,6:0.0} sec.", PlayerPrefs.GetFloat("BestTime"));
                gameObject.SetActive(false);
            }

            coin.transform.position = new Vector3(-7.0f, 3.5f, 0f);
            coinSound.Play();
        }

        
        
    }

    void SetLanded()
    {
        animator.SetBool("Landed", false);
    }

    void infoAnzeigeLeeren()
    {
        infoAnzeige.text = "";
    }

    public void EndGame()
    {
        Application.Quit();
    }

    public void NewGame()
    {
        zeitStart = Time.time;
        leben = 3;
        coin.SetActive(true);
        coinPosition = -4f;
        coin.transform.position = new Vector3(Random.Range(-6.0f, 6.0f), coinPosition, 0);
        gameObject.SetActive(true);
        infoAnzeige.text = "";
        transform.position = startPosition;

        gameObject.SetActive(true);
    }

    public void Lauter()
    {
        musik.volume = musik.volume + 0.04f;
        PlayerPrefs.SetFloat("Lautstaerke", musik.volume);
    }

    public void Leiser()
    {
        musik.volume = musik.volume - 0.04f;
        PlayerPrefs.SetFloat("Lautstaerke", musik.volume);
    }
}
