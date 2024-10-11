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

        //���� ä�� �̱� 1~9
        vNumber[0] = Random.Range(1, 10);
        vNumber[1] = Random.Range(1, 10);
        vNumber[2] = Random.Range(1, 10);
        while(true)
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

        //����ä�� ����
        for(int i = 0; i < 3; i++)
        {
            sVegetable = vegetables.transform.GetChild(vNumber[i] - 1).gameObject;
            Debug.Log($"{i+1}��° ä�� : {vNumber[i]},{sVegetable.name}"); //�ܼ�

            //�̹��� ����
            sVegetable.tag = "targetV";
            targetVS = box.transform.GetChild(i).gameObject.GetComponent<SpriteRenderer>();
            targetVS.sprite = sVegetable.GetComponent<SpriteRenderer>().sprite;
            targetVS.color = new Color(0.3f, 0.3f, 0.3f, 1);
        }
        Debug.Log("ClawClear ����");

        csm = GameObject.Find("SoundManager").GetComponent<clawSoundManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(clearCount == 3)
        {
            if(StoryOrStage.instance != null) {
                StoryOrStage.instance.isClawGood = true;
            }
            clawScore = clawScore * 100.0f / 90.0f;
            GameObject.Find("GameManager").GetComponent<GameManager>().AddScore((int)clawScore);
            GameObject.Find("GameManager").GetComponent<GameManager>().EndGame(0, 0);
            clawControl.gameOver = true;
            csm.endSound.Play();
            csm.clawSound.Stop();
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
            Debug.Log("����");
            clawScore -= sPoint;
            if (clawScore < 0)
            {
                clawScore = 0;
            }
        }
        else if(enterObj.CompareTag("targetV"))
        {
            Debug.Log($"{objName}Ŭ����");
            for(int i=0; i < 3; i++)
            {
                if(int.Parse(objName.Substring(objName.Length - 1)) == vNumber[i])
                {
                    box.transform.GetChild(i).gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                }
            }
            clawScore += vPoint;
            clearCount++;
            csm.itemGoodSound.Play();
        }
        else
        {
            Debug.Log("�߸��� ����");
            clawScore -= wvPoint;
            if (clawScore < 0)
            {
                clawScore = 0;
            }
            csm.itemFailSound.Play();
        }
        Destroy(enterObj);
    }
}
