using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    public float timerValue = 0f;
    
    public bool isGameFinished;
    public Canvas gameOverCanvas;
    public Text gameOverTitleText;
    public Text gameOverTimerText;
    public Text gameOverCirclesText;

    void Start()
    {
        //gameObject.transform.localEulerAngles = new Vector3(camObject.transform.localEulerAngles.x, camObject.transform.eulerAngles.y, camObject.transform.localEulerAngles.z);
        Debug.Log(SceneManager.GetActiveScene().name);

        
        gameOverCanvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        gameOverTitleText = GameObject.Find("TitleText").GetComponent<Text>();
        gameOverTimerText = GameObject.Find("TimerText").GetComponent<Text>();
        gameOverCirclesText = GameObject.Find("CirclesText").GetComponent<Text>();

        gameOverCanvas.enabled = false;

        if (SceneManager.GetActiveScene().name.Equals("Example_Terrain"))
        {
            circles = 9;
            spheres = 8;
        }
        if (SceneManager.GetActiveScene().name.Equals("SceneForPlaneLast"))
        {
            circles = 9;
            spheres = 10;
        }
        GvrCardboardHelpers.Recenter();
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

        UpdateTimer();
        
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

        if (isGameFinished)
        {
            gameOverTitleText.text = "Полёт завершён!";
            gameOverTimerText.text = FormatTime(timerValue);
            gameOverCirclesText.text = "Сфер собрано: " + sphereCount; 
            gameOverCanvas.enabled = true;
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

        if (circleCount == circles)
        {
            isGameFinished = true;
            StartCoroutine(ChangeLocationAfterTime(3));
        }

    }
    
    void UpdateTimer()
    {
        if (isGameFinished) return;
        timerValue += Time.deltaTime;
    }

    String FormatTime(double unformatted)
    {
        int rounded = (int) Math.Round(unformatted);
        return AddLeadingZero(rounded / 3600) + ":" 
               + AddLeadingZero(rounded / 60) + ":"
               + AddLeadingZero(rounded % 60);
    }

    String AddLeadingZero(int n)
    {
        if (n < 10)
            return "0" + n.ToString();
        else
            return n.ToString();
    }
    
    IEnumerator ChangeLocationAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene("rpgpp_lt_scene");
    }
}
