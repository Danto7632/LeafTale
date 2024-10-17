using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class creditTextMove : MonoBehaviour
{
    public float speed = 0.006f;
    float pointValue = 0;

    // Start is called before the first frame update
    void Start()
    {
        pointValue = this.transform.position.y + 77;
    }

    // Update is called once per frame
    void Update()
    {
        if(this.transform.position.y < pointValue)
        {
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + speed);
        }
        
    }
}
