using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClawClear : MonoBehaviour
{
    int[] vNumber = new int[3];
    int currentN;

    public double clawScore;
    public double maxScore;
    public double vPoint;
    public double sPoint;
    public double wvPoint;
    public int clearCount;
    public int failCount;

    GameObject vegetables;
    GameObject sVegetable;
    GameObject box;
    SpriteRenderer targetVS;

    public clawSoundManager csm;
    
    void Awake()
    {
        vegetables = GameObject.Find("vegetables");
        box = GameObject.Find("box");
    }

    // Start is called before the first frame update
    void Start()
    {
        clawScore = 0.0f;
        vPoint = 30.0f;
        maxScore = vPoint * 3;
        sPoint = 3.0f;
        wvPoint = 20.0f;
        clearCount = 0;
        failCount = 2;

        vNumber[0] = Random.Range(1, 10);
        vNumber[1] = Random.Range(1, 10);
        vNumber[2] = Random.Range(1, 10);
        while (true)
        {
            if (vNumber[0] != vNumber[1])
            {
                break;
            }
            vNumber[1] = Random.Range(1, 10);
        }
        while (true)
        {
            if (vNumber[0] != vNumber[2] && vNumber[1] != vNumber[2])
            {
                break;
            }
            vNumber[2] = Random.Range(1, 10);
        }

        for(int i = 0; i < 3; i++)
        {
            sVegetable = vegetables.transform.GetChild(vNumber[i] - 1).gameObject;
            Debug.Log($"{i+1}.n vegetable : {vNumber[i]},{sVegetable.name}");

            sVegetable.tag = "targetV";
            targetVS = box.transform.GetChild(i).gameObject.GetComponent<SpriteRenderer>();
            targetVS.sprite = sVegetable.GetComponent<SpriteRenderer>().sprite;
            targetVS.color = new Color(0.3f, 0.3f, 0.3f, 1);
        }

        csm = GameObject.Find("SoundManager").GetComponent<clawSoundManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (clearCount == 3)
        {
            if (StoryOrStage.instance != null) {
                StoryOrStage.instance.isClawGood = true;
                StoryOrStage.instance.clearCount++;
            }
            clawScore = clawScore * 100.0f / 90.0f;
            GameObject.Find("GameManager").GetComponent<GameManager>().AddScore((int)clawScore);
            GameObject.Find("GameManager").GetComponent<GameManager>().EndGame(0, 0);
            clawControl.gameOver = true;
            csm.endSound.Play();
            clearCount = 4;
        }

        if(failCount == 0) {
            if (StoryOrStage.instance != null) {
                StoryOrStage.instance.isClawGood = false;
            }
            clawScore = 0.0f;
            GameObject.Find("GameManager").GetComponent<GameManager>().AddScore((int)clawScore);
            GameObject.Find("GameManager").GetComponent<GameManager>().EndGame(0, 0);
            clawControl.gameOver = true;
            csm.endSound.Play();
            clearCount = 4;
        }
    }

    void OnCollisionEnter2D(Collision2D enterObject)
    {
        GameObject enterObj;
        string objName;

        enterObj = enterObject.gameObject;
        objName = enterObj.name;
        if (enterObj.CompareTag("soil"))
        {
            Debug.Log("soil");
            clawScore -= sPoint;
            if (clawScore < 0)
            {
                clawScore = 0;
            }
        }
        else if(enterObj.CompareTag("targetV"))
        {
            for(int i=0; i < 3; i++)
            {
                Debug.Log($"{objName.Substring(objName.Length - 1)}, {vNumber[i]}");
                if (int.Parse(objName.Substring(objName.Length - 1)) == vNumber[i])
                {
                    Debug.Log($"{objName}'s color change! {box.transform.GetChild(i).name}");
                    box.transform.GetChild(i).gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                }
            }
            clawScore += vPoint;
            clearCount++;
            csm.itemGoodSound.Play();
        }
        else
        {
            clawScore -= wvPoint;
            if (clawScore < 0)
            {
                clawScore = 0;
            }
            csm.itemFailSound.Play();
            failCount--;
        }
        Destroy(enterObj);
    }
}
