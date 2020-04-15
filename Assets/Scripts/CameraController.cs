using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private GameObject Camera1;
    private GameObject Camera2;
    private GameObject Camera3;
    private GameObject Camera4;
    private GameObject Camera5;


    // Start is called before the first frame update
    void Start()
    {
        Camera1 = GameObject.Find("Camera1");
        Camera2 = GameObject.Find("Camera2");
        Camera3 = GameObject.Find("Camera3");
        Camera4 = GameObject.Find("FrontCamera");
        Camera5 = GameObject.Find("SideCamera");

        //Camera2.SetActive(false);
        //Camera3.SetActive(false);
        //Camera4.SetActive(false);
        //Camera5.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("1"))
        {
            Camera1.SetActive(true);
            Camera2.SetActive(false);
            Camera3.SetActive(false);
            Camera4.SetActive(false);
            Camera5.SetActive(false);
        }
        if (Input.GetKey("2"))
        {
            Camera1.SetActive(false);
            Camera2.SetActive(true);
            Camera3.SetActive(false);
            Camera4.SetActive(false);
            Camera5.SetActive(false);
        }
        if (Input.GetKey("3"))
        {
            Camera1.SetActive(false);
            Camera2.SetActive(false);
            Camera3.SetActive(true);
            Camera4.SetActive(false);
            Camera5.SetActive(false);
        }
        if (Input.GetKey("4"))
        {
            Camera1.SetActive(false);
            Camera2.SetActive(false);
            Camera3.SetActive(false);
            Camera4.SetActive(true);
            Camera5.SetActive(false);
        }
        if (Input.GetKey("5"))
        {
            Camera1.SetActive(false);
            Camera2.SetActive(false);
            Camera3.SetActive(false);
            Camera4.SetActive(false);
            Camera5.SetActive(true);
        }
    }
}
