﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class movement : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed = 0.00001f;
    public GameObject gameObject;
    public GameObject game;
    public Camera camObject;
    public float rotSpeed = 1;
    public CharacterController character;
    private float distanceToGround;
    private int circleCount = 0, sphereCount = 0;
    private int circles = 0, spheres = 0;
    public float timer;
    public bool isGameFinished = false;

    void Start()
    {
        //gameObject.transform.localEulerAngles = new Vector3(camObject.transform.localEulerAngles.x, camObject.transform.eulerAngles.y, camObject.transform.localEulerAngles.z);
        Debug.Log(SceneManager.GetActiveScene().name);
        if (SceneManager.GetActiveScene().name.Equals("Example_Terrain"))
        {
            circles = 8;
            spheres = 8;
        }
        if (SceneManager.GetActiveScene().name.Equals("SceneForPlaneLast"))
        {
            circles = 9;
            spheres = 10;
        }

    }

    // Update is called once per frame
    void Update()
    {
        //gameObject.transform.rotation = new Quaternion((360 - camObject.transform.localEulerAngles.z)%360, (360 - camObject.transform.localEulerAngles.y) % 360, (360 - camObject.transform.localEulerAngles.x) % 360, camObject.transform.rotation.w);
        //gameObject.transform.rotation = new Quaternion(camObject.transform.rotation.z, camObject.transform.rotation.y, camObject.transform.rotation.x, camObject.transform.rotation.w);
        //gameObject.transform.rotation = new Quaternion(camObject.transform.rotation.z, camObject.transform.rotation.y, camObject.transform.rotation.x, camObject.transform.rotation.w);
        //gameObject.transform.rotation = new Quaternion(camObject.transform.rotation.z, camObject.transform.rotation.y, camObject.transform.rotation.x, camObject.transform.rotation.w);
        //gameObject.transform.position = new Vector3(transform.position.x + speed * (float)Math.Cos(camObject.transform.localEulerAngles.x * Math.PI/180),
        //transform.position.y + speed * (float)Math.Cos(camObject.transform.localEulerAngles.y * Math.PI / 180), transform.position.z + speed * (float)Math.Cos(camObject.transform.localEulerAngles.z * Math.PI / 180));

        

        // Debug.Log("Time: " + timer);

        ChangeTime();
        gameObject.transform.Rotate(360 - camObject.transform.localRotation.x * rotSpeed, camObject.transform.localRotation.y * rotSpeed, 360 - camObject.transform.localRotation.z * rotSpeed, Space.Self);
        gameObject.transform.Translate(Vector3.back * speed * Time.deltaTime);

        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(transform.position, -Vector3.up, out hit))
        {
            distanceToGround = hit.distance;
        }
        if (Physics.Raycast(transform.position, -Vector3.forward, out hit))
        {
            var distanceToFirst = hit.distance;
            if (distanceToFirst < 0.3)
            {
                gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z+distanceToFirst);
            }
        }

        if (distanceToGround < 0.3)
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + distanceToGround, gameObject.transform.position.z);
        }
    }
    public void OnTriggerEnter(Collider col)
    {
        //Debug.Log("Keeeeeeek " + col.gameObject.name);
        if (col.gameObject.name.Contains("Sphere"))
        {
            Destroy(col.gameObject);
            sphereCount++;
            Debug.Log("Spheres collected: " + sphereCount);
        }
        if (col.gameObject.name.Contains("Circle"))
        {
            Destroy(col.gameObject);
            circleCount++;
            Debug.Log("Circles collected: " + circleCount);
        }

        //if(circleCount == circles && sphereCount == spheres)
        if (circleCount > 0)
        {
            isGameFinished = true;
            StartCoroutine(ChangeLocationAfterTime(5));
        }

    }
    void OnGUI()
    {
        GUIStyle guiStyle = new GUIStyle {fontSize = 40};
        if (isGameFinished)
        {
            GUI.contentColor = Color.white;
            GUI.backgroundColor = Color.black;
            GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "");
            GUI.Label(new Rect(Screen.width / 2f, Screen.height / 2f, 100, 100),
                timer.ToString());
        }
    }

    void ChangeTime()
    {
        if (isGameFinished) return;
        timer += Time.deltaTime;
    }
    
    IEnumerator ChangeLocationAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
 
        SceneManager.LoadScene("rpgpp_lt_scene");
    }
}
