using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseCamera : MonoBehaviour
{
    private GameObject simple_human;
    private GameObject Armature;
    private GameObject Root;
    private Vector3 offset;
    private float hight;
    private Vector3 nextPosition;

    // Start is called before the first frame update
    void Start()
    {
        simple_human = GameObject.Find("simple_human_1");
        Armature = simple_human.transform.Find("Armature").gameObject;
        Root = Armature.transform.Find("root").gameObject;
        offset = transform.position - Root.transform.position;
        hight = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (Root == null)
        {
            simple_human = GameObject.Find("simple_human_1");
            Armature = simple_human.transform.Find("Armature").gameObject;
            Root = Armature.transform.Find("root").gameObject;
        }
        nextPosition.x = Root.transform.position.x + offset.x;
        nextPosition.y = hight;
        nextPosition.z = Root.transform.position.z + offset.z;
        transform.position = nextPosition;
    }
}
