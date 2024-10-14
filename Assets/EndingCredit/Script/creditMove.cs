using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class creditMove : MonoBehaviour
{
    public Sprite[] goodEndings;
    GameObject endingPictures;
    int endingCount = 0;
    public float speed = 0.004f;

    // Start is called before the first frame update
    void Start()
    {
        endingPictures = this.transform.Find("endings").gameObject;

        //change ending pictures
        if(StoryOrStage.instance == null)
        {
            Debug.Log("StoryOrStage does not exist");
        }
        else
        {
            if (StoryOrStage.instance.isClawGood)
            {
                endingPictures.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = goodEndings[0];
                endingCount++;
            }
            if (StoryOrStage.instance.isBroomGood)
            {
                endingPictures.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().sprite = goodEndings[1];
                endingCount++;
            }
            if (StoryOrStage.instance.isPlatGood)
            {
                endingPictures.transform.GetChild(2).gameObject.GetComponent<SpriteRenderer>().sprite = goodEndings[2];
                endingCount++;
            }
            if (StoryOrStage.instance.isRhythmGood)
            {
                endingPictures.transform.GetChild(3).gameObject.GetComponent<SpriteRenderer>().sprite = goodEndings[3];
                endingCount++;
            }
            if (StoryOrStage.instance.isMagicGood)
            {
                endingPictures.transform.GetChild(4).gameObject.GetComponent<SpriteRenderer>().sprite = goodEndings[4];
                endingCount++;
            }
            if (endingCount >= 3)
            {
                endingPictures.transform.GetChild(5).gameObject.GetComponent<SpriteRenderer>().sprite = goodEndings[5];
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("p") == true) //skip credit
        {
            SceneManager.LoadScene("StartPage");
        }

        //credit move
        if (gameObject.transform.position.y < 45.5f)
        {
            gameObject.transform.Translate(0, speed, 0);
        }
        else
        {
            Invoke("goBackToStart", 2.5f);
        }
    }
    void goBackToStart()
    {
        SceneManager.LoadScene("StartPage");
    }
}
