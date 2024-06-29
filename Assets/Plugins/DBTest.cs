using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using MySql.Data.MySqlClient;

public class DBTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Connection Test: " + ConnectionTest());
    }

    public bool ConnectionTest()
    {
        string conStr = string.Format("Server={0};Database={1};Uid={2};Pwd={3};", "127.0.0.1", "handb", "root", "cateye123");
        try
        {
            using (MySqlConnection conn = new MySqlConnection(conStr))
            {
                conn.Open();
            }
            return true;
        }
        catch(Exception e)
        {
            Debug.Log("e:" + e.ToString());
            return false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
