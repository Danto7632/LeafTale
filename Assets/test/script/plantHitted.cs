using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class plantHitted : MonoBehaviour
{
    void Start()
    {
        Destroy(gameObject, 0.75f);
    }
}
