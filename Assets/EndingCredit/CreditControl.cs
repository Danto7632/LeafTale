using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditControl : MonoBehaviour
{
    GameObject Ending;
    public int[] endingNum = new int[6] { 0, 0, 0, 0, 0, 0};
    public int endingStory = 0;

    // Start is called before the first frame update
    void Awake()
    {
        Ending = GameObject.Find("EndingPhoto");
    }

    void Start()
    {
        endingNum = EndingValue.endingValue;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
