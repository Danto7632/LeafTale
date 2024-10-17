using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dragonHitted : MonoBehaviour
{
    void Start()
    {
        Destroy(gameObject, 1.333f);
    }
}
