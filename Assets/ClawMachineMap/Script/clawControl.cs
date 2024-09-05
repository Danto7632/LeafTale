using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clawControl : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject powerBar;

    public bool clawsOpen;
    bool /*goUp,*/ goDown, goLeft, goRight;
    Rigidbody2D Rclaw, Lclaw, machine;
    float speed = 0.02f;
    public bool gameOver;

    void Awake()
    {
        powerBar = GameObject.Find("powerValue");
    }

    void Start()
    {
        Rclaw = GameObject.Find("clawRight").GetComponent<Rigidbody2D>();
        Lclaw = GameObject.Find("clawLeft").GetComponent<Rigidbody2D>();
        machine = gameObject.GetComponent<Rigidbody2D>();
        clawsOpen = true;
        gameOver = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameOver)
        {
            //down
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                goDown = true;
            }
            if (Input.GetKeyUp(KeyCode.DownArrow))
            {
                goDown = false;
            }
            ////up
            //if (Input.GetKeyDown(KeyCode.UpArrow))
            //{
            //    goUp = true;
            //}
            //if (Input.GetKeyUp(KeyCode.UpArrow))
            //{
            //    goUp = false;
            //}
            //left
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                goLeft = true;
            }
            if (Input.GetKeyUp(KeyCode.LeftArrow))
            {
                goLeft = false;
            }
            //right
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                goRight = true;
            }
            if (Input.GetKeyUp(KeyCode.RightArrow))
            {
                goRight = false;
            }
            //claw
            //if (Input.GetKeyUp(KeyCode.Space))
            //{
            //    clawsOpen = !clawsOpen;
            //}
            if (Input.GetKeyDown(KeyCode.Space))
            {
                clawsOpen = false;
            }
            if (Input.GetKeyUp(KeyCode.Space))
            {
                clawsOpen = true;
            }

            //-------------------------------------

            //if (goUp)
            //{
            //    if (gameObject.transform.position.y < 4)
            //    {
            //        gameObject.transform.Translate(0, speed, 0);
            //    }
            //}
            
            if (!goDown)
            {
                if (gameObject.transform.position.y < 3.5f)
                {
                    gameObject.transform.Translate(0, speed - .005f, 0);
                }
            }
            if (clawsOpen)
            {
                if (Lclaw.transform.eulerAngles.z > 300)
                {
                    Lclaw.transform.Rotate(0, 0, -0.5f);
                }
                if (Rclaw.transform.eulerAngles.z < 60)
                {
                    Rclaw.transform.Rotate(0, 0, 0.5f);
                }
            }
            if (goDown || goLeft || goRight || !clawsOpen)
            {
                if (goDown)
                {
                    if (gameObject.transform.position.y > -3.0f && gameObject.transform.position.x > -1.95f)
                    {
                        gameObject.transform.Translate(0, -speed, 0);
                    }
                    else if (gameObject.transform.position.y > 3.0f)
                    {
                        gameObject.transform.Translate(0, -speed, 0);
                    }
                }
                if (goLeft)
                {
                    if (gameObject.transform.position.x > -1.8f)
                    {
                        gameObject.transform.Translate(-speed, 0, 0);
                    }
                    else if (gameObject.transform.position.x > -6.32f && gameObject.transform.position.y > 2.9f)
                    {
                        gameObject.transform.Translate(-speed, 0, 0);
                    }
                }
                if (goRight)
                {
                    if (gameObject.transform.position.x < 6f)
                    {
                        gameObject.transform.Translate(speed, 0, 0);
                    }
                }
                if (!clawsOpen)
                {
                    if (Lclaw.transform.eulerAngles.z < 353)
                    {
                        Lclaw.transform.Rotate(0, 0, 1f);
                    }
                    if (Rclaw.transform.eulerAngles.z > 6)
                    {
                        Rclaw.transform.Rotate(0, 0, -1f);
                    }
                }
                powerBar.GetComponent<powerBar>().usePower();
            }
        }
    }
}
