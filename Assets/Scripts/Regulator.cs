using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using MLAgents;
using MLAgents.Sensors;

public class Regulator : Agent
{
    public int ID=1;
    private float distance=8.0f;

    private GameObject Armature;
    private Rigidbody Root;

    private Rigidbody X_Stomach, Y_Stomach, Z_Stomach, Stomach;
    private Rigidbody X_Chest, Y_Chest, Z_Chest, Chest;

    private Rigidbody Neck, Head;

    private Rigidbody Shoulder_L;
    private Rigidbody X_Upper_L, Y_Upper_L, Z_Upper_L, Upper_L;
    private Rigidbody X_Forearm_L, Y_Forearm_L, Z_Forearm_L, Forearm_L;
    private Rigidbody X_Hand_L, Y_Hand_L, Z_Hand_L, Hand_L;

    private Rigidbody Shoulder_R;
    private Rigidbody X_Upper_R, Y_Upper_R, Z_Upper_R, Upper_R;
    private Rigidbody X_Forearm_R, Y_Forearm_R, Z_Forearm_R, Forearm_R;
    private Rigidbody X_Hand_R, Y_Hand_R, Z_Hand_R, Hand_R;

    private Rigidbody X_Crotch_L, Y_Crotch_L, Z_Crotch_L, Crotch_L;
    private Rigidbody Z_Thigh_L, Y_Thigh_L, X_Thigh_L, Thigh_L;
    private Rigidbody X_Shin_L, Y_Shin_L, Z_Shin_L, Shin_L;
    private Rigidbody X_Foot_L, Y_Foot_L, Z_Foot_L, Foot_L;

    private Rigidbody X_Crotch_R, Y_Crotch_R, Z_Crotch_R, Crotch_R;
    private Rigidbody Z_Thigh_R, Y_Thigh_R, X_Thigh_R, Thigh_R;
    private Rigidbody X_Shin_R, Y_Shin_R, Z_Shin_R, Shin_R;
    private Rigidbody X_Foot_R, Y_Foot_R, Z_Foot_R, Foot_R;



    private float reward_Chest_hight_ratio = 1.0f;
    private float reward_velocity_ratio = 0.5f;
    private float reward_Neck_similarity_ratio = 1.5f;
    private float reward_Crotch_similarity_ratio = 1.5f;
    //private float reward_Shin_distance_ratio = 30f;
    private float reward_Neck_distance_ratio = 5f;
    private float reward_Shin_distance_ratio = 5f;
    private float reward_Foot_distance_ratio = 5f;


    // public float penalty_rotation_ratio = 1.0f;
    private float penalty_range_x = 2.0f;
    //private float penalty_chest_y = 1.2f;
    //private float boader_thigh_y = 0.3f;
    //private float EndEpisode_hight_y = 0.9f;
    private float base_arm_torque = 40f;
    private float base_chest_torque = 40f;
    private float base_stomach_torque = 40;
    private float base_thigh_torque = 400f;
    private float base_shin_torque = 400f;
    private float base_foot_torque = 40f;
    private float vel_threshold = 40f;
    private float timeLimit = 90f;


    private float countup = 0.0f;
    private Quaternion init_Neck_rotation;
    private Vector3 init_Neck_position;
    private Quaternion init_Chest_rotation;
    private Quaternion init_Crotch_rotation;
    private Quaternion init_Thigh_L_rotation;
    private Quaternion init_Thigh_R_rotation;
    private Vector3 init_Shin_L_position;
    private Vector3 init_Shin_R_position;


    public override void OnEpisodeBegin()
    {
        string name;
        name = "simple_human_" + ID.ToString();

        if (GameObject.Find(name) != null)
        {
            Destroy(GameObject.Find(name));
        }
        GameObject simple_human = Instantiate(Resources.Load("simple_human_1")) as GameObject;
        simple_human.name = name;
        simple_human.transform.position = new Vector3((ID-1)*distance,simple_human.transform.position.y,simple_human.transform.position.z);

        Armature = simple_human.transform.Find("Armature").gameObject;
        Root = Armature.transform.Find("root").GetComponent<Rigidbody>();

        X_Stomach = Root.transform.Find("jointX_stomach").GetComponent<Rigidbody>();
        Y_Stomach = X_Stomach.transform.Find("jointY_stomach").GetComponent<Rigidbody>();
        Z_Stomach = Y_Stomach.transform.Find("jointZ_stomach").GetComponent<Rigidbody>();
        Stomach = Z_Stomach.transform.Find("stomach").GetComponent<Rigidbody>();

        X_Chest = Stomach.transform.Find("jointX_chest").GetComponent<Rigidbody>();
        Y_Chest = X_Chest.transform.Find("jointY_chest").GetComponent<Rigidbody>();
        Z_Chest = Y_Chest.transform.Find("jointZ_chest").GetComponent<Rigidbody>();
        Chest = Z_Chest.transform.Find("chest").GetComponent<Rigidbody>();

        Neck = Chest.transform.Find("neck").GetComponent<Rigidbody>();
        Head = Neck.transform.Find("head").GetComponent<Rigidbody>();


        Shoulder_L = Chest.transform.Find("shoulder_L").GetComponent<Rigidbody>();
        X_Upper_L = Shoulder_L.transform.Find("jointX_upper_L").GetComponent<Rigidbody>();
        Y_Upper_L = X_Upper_L.transform.Find("jointY_upper_L").GetComponent<Rigidbody>();
        Z_Upper_L = Y_Upper_L.transform.Find("jointZ_upper_L").GetComponent<Rigidbody>();
        Upper_L = Z_Upper_L.transform.Find("upper_L").GetComponent<Rigidbody>();

        X_Forearm_L = Upper_L.transform.Find("jointX_forearm_L").GetComponent<Rigidbody>();
        Y_Forearm_L = X_Forearm_L.transform.Find("jointY_forearm_L").GetComponent<Rigidbody>();
        Z_Forearm_L = Y_Forearm_L.transform.Find("jointZ_forearm_L").GetComponent<Rigidbody>();
        Forearm_L = Z_Forearm_L.transform.Find("forearm_L").GetComponent<Rigidbody>();

        X_Hand_L = Forearm_L.transform.Find("jointX_hand_L").GetComponent<Rigidbody>();
        Y_Hand_L = X_Hand_L.transform.Find("jointY_hand_L").GetComponent<Rigidbody>();
        Z_Hand_L = Y_Hand_L.transform.Find("jointZ_hand_L").GetComponent<Rigidbody>();
        Hand_L = Z_Hand_L.transform.Find("hand_L").GetComponent<Rigidbody>();


        Shoulder_R = Chest.transform.Find("shoulder_R").GetComponent<Rigidbody>();
        X_Upper_R = Shoulder_R.transform.Find("jointX_upper_R").GetComponent<Rigidbody>();
        Y_Upper_R = X_Upper_R.transform.Find("jointY_upper_R").GetComponent<Rigidbody>();
        Z_Upper_R = Y_Upper_R.transform.Find("jointZ_upper_R").GetComponent<Rigidbody>();
        Upper_R = Z_Upper_R.transform.Find("upper_R").GetComponent<Rigidbody>();

        X_Forearm_R = Upper_R.transform.Find("jointX_forearm_R").GetComponent<Rigidbody>();
        Y_Forearm_R = X_Forearm_R.transform.Find("jointY_forearm_R").GetComponent<Rigidbody>();
        Z_Forearm_R = Y_Forearm_R.transform.Find("jointZ_forearm_R").GetComponent<Rigidbody>();
        Forearm_R = Z_Forearm_R.transform.Find("forearm_R").GetComponent<Rigidbody>();

        X_Hand_R = Forearm_R.transform.Find("jointX_hand_R").GetComponent<Rigidbody>();
        Y_Hand_R = X_Hand_R.transform.Find("jointY_hand_R").GetComponent<Rigidbody>();
        Z_Hand_R = Y_Hand_R.transform.Find("jointZ_hand_R").GetComponent<Rigidbody>();
        Hand_R = Z_Hand_R.transform.Find("hand_R").GetComponent<Rigidbody>();


        X_Crotch_L = Root.transform.Find("jointX_crotch_L").GetComponent<Rigidbody>();
        Y_Crotch_L = X_Crotch_L.transform.Find("jointY_crotch_L").GetComponent<Rigidbody>();
        Z_Crotch_L = Y_Crotch_L.transform.Find("jointZ_crotch_L").GetComponent<Rigidbody>();
        Crotch_L = Z_Crotch_L.transform.Find("crotch_L").GetComponent<Rigidbody>();

        Z_Thigh_L = Crotch_L.transform.Find("jointZ_thigh_L").GetComponent<Rigidbody>();
        Y_Thigh_L = Z_Thigh_L.transform.Find("jointY_thigh_L").GetComponent<Rigidbody>();
        X_Thigh_L = Y_Thigh_L.transform.Find("jointX_thigh_L").GetComponent<Rigidbody>();
        Thigh_L = X_Thigh_L.transform.Find("thigh_L").GetComponent<Rigidbody>();

        X_Shin_L = Thigh_L.transform.Find("jointX_shin_L").GetComponent<Rigidbody>();
        Y_Shin_L = X_Shin_L.transform.Find("jointY_shin_L").GetComponent<Rigidbody>();
        Z_Shin_L = Y_Shin_L.transform.Find("jointZ_shin_L").GetComponent<Rigidbody>();
        Shin_L = Z_Shin_L.transform.Find("shin_L").GetComponent<Rigidbody>();

        X_Foot_L = Shin_L.transform.Find("jointX_foot_L").GetComponent<Rigidbody>();
        Y_Foot_L = X_Foot_L.transform.Find("jointY_foot_L").GetComponent<Rigidbody>();
        Z_Foot_L = Y_Foot_L.transform.Find("jointZ_foot_L").GetComponent<Rigidbody>();
        Foot_L = Z_Foot_L.transform.Find("foot_L").GetComponent<Rigidbody>();


        X_Crotch_R = Root.transform.Find("jointX_crotch_R").GetComponent<Rigidbody>();
        Y_Crotch_R = X_Crotch_R.transform.Find("jointY_crotch_R").GetComponent<Rigidbody>();
        Z_Crotch_R = Y_Crotch_R.transform.Find("jointZ_crotch_R").GetComponent<Rigidbody>();
        Crotch_R = Z_Crotch_R.transform.Find("crotch_R").GetComponent<Rigidbody>();

        Z_Thigh_R = Crotch_R.transform.Find("jointZ_thigh_R").GetComponent<Rigidbody>();
        Y_Thigh_R = Z_Thigh_R.transform.Find("jointY_thigh_R").GetComponent<Rigidbody>();
        X_Thigh_R = Y_Thigh_R.transform.Find("jointX_thigh_R").GetComponent<Rigidbody>();
        Thigh_R = X_Thigh_R.transform.Find("thigh_R").GetComponent<Rigidbody>();

        X_Shin_R = Thigh_R.transform.Find("jointX_shin_R").GetComponent<Rigidbody>();
        Y_Shin_R = X_Shin_R.transform.Find("jointY_shin_R").GetComponent<Rigidbody>();
        Z_Shin_R = Y_Shin_R.transform.Find("jointZ_shin_R").GetComponent<Rigidbody>();
        Shin_R = Z_Shin_R.transform.Find("shin_R").GetComponent<Rigidbody>();

        X_Foot_R = Shin_R.transform.Find("jointX_foot_R").GetComponent<Rigidbody>();
        Y_Foot_R = X_Foot_R.transform.Find("jointY_foot_R").GetComponent<Rigidbody>();
        Z_Foot_R = Y_Foot_R.transform.Find("jointZ_foot_R").GetComponent<Rigidbody>();
        Foot_R = Z_Foot_R.transform.Find("foot_R").GetComponent<Rigidbody>();


        init_Neck_rotation = Quaternion.Euler(new Vector3(0,180,0));
        init_Neck_position = Neck.transform.position;
        init_Chest_rotation = Quaternion.Euler(new Vector3(0,180,0));
        init_Crotch_rotation = Quaternion.Euler(new Vector3(180,0,90));
        init_Thigh_L_rotation = Quaternion.Euler(new Vector3(180,0,0));
        init_Thigh_R_rotation = Quaternion.Euler(new Vector3(180,0,0));
        init_Shin_L_position = Shin_L.transform.position;
        init_Shin_R_position = Shin_R.transform.position;
        
        countup = 0.0f;
    }

    private void limit_ang(Rigidbody rBody, float ux,float lx,float uy,float ly,float uz,float lz)
    {
        float limit_x = Mathf.Clamp(rBody.transform.localEulerAngles.x, lx, ux);
        float limit_y = Mathf.Clamp(rBody.transform.localEulerAngles.y, ly, uy);
        float limit_z = Mathf.Clamp(rBody.transform.localEulerAngles.z, lz, uz);
        
        rBody.transform.localEulerAngles = new Vector3(limit_x, limit_y, limit_z);
    }

    private void limit_vel(Rigidbody rBody, float ux,float uy,float uz)
    {
        Vector3 localAngVel = transform.InverseTransformDirection(rBody.angularVelocity);
        float limit_x = Mathf.Clamp(localAngVel.x, -ux, ux);
        float limit_y = Mathf.Clamp(localAngVel.y, -uy, uy);
        float limit_z = Mathf.Clamp(localAngVel.z, -uz, uz);
        Vector3 vec = new Vector3(limit_x, limit_y, limit_z);
    }


    private void FixedJoint(Rigidbody rBody)
    {
        rBody.transform.localEulerAngles = new Vector3(0,0,0);
    }


    private void controlTorque(Rigidbody rBodyX, Rigidbody rBodyY, Rigidbody rBodyZ, float signalX, float signalY, float signalZ, float base_torque)
    {
        Vector3 torqueX = Vector3.zero;
        Vector3 torqueY = Vector3.zero;
        Vector3 torqueZ = Vector3.zero;
        torqueX.x = Mathf.Clamp(signalX,-1,1) * base_torque * Mathf.PI;
        torqueY.y = Mathf.Clamp(signalY,-1,1) * base_torque * Mathf.PI;
        torqueZ.z = Mathf.Clamp(signalZ,-1,1) * base_torque * Mathf.PI;

        rBodyX.AddTorque(torqueX);
        rBodyY.AddTorque(torqueY);
        rBodyZ.AddTorque(torqueZ);
    }


    private void FixJoint(Rigidbody rX, Rigidbody rY, Rigidbody rZ, float bX, float bY, float bZ)
    {
        rX.transform.localEulerAngles = new Vector3(bX,0,0);
        rY.transform.localEulerAngles = new Vector3(0,bY,0);
        rZ.transform.localEulerAngles = new Vector3(0,0,bZ);
    }


    public override void OnActionReceived(float[] vectorAction)
    {
        float b_Thigh_L_rotationX = X_Thigh_L.transform.localEulerAngles.x;
        float b_Thigh_L_rotationY = Y_Thigh_L.transform.localEulerAngles.y;
        float b_Thigh_L_rotationZ = Z_Thigh_L.transform.localEulerAngles.z;

        float b_Shin_L_rotationX = X_Shin_L.transform.localEulerAngles.x;
        
        float b_Foot_L_rotationX = X_Foot_L.transform.localEulerAngles.x;
        float b_Foot_L_rotationY = Y_Foot_L.transform.localEulerAngles.y;
        float b_Foot_L_rotationZ = Z_Foot_L.transform.localEulerAngles.z;


        float b_Thigh_R_rotationX = X_Thigh_R.transform.localEulerAngles.x;
        float b_Thigh_R_rotationY = Y_Thigh_R.transform.localEulerAngles.y;
        float b_Thigh_R_rotationZ = Z_Thigh_R.transform.localEulerAngles.z;

        float b_Shin_R_rotationX = X_Shin_R.transform.localEulerAngles.x;
        
        float b_Foot_R_rotationX = X_Foot_R.transform.localEulerAngles.x;
        float b_Foot_R_rotationY = Y_Foot_R.transform.localEulerAngles.y;
        float b_Foot_R_rotationZ = Z_Foot_R.transform.localEulerAngles.z;


        float b_Chest_rotationX = X_Chest.transform.localEulerAngles.x;
        float b_Chest_rotationY = Y_Chest.transform.localEulerAngles.y;
        float b_Chest_rotationZ = Z_Chest.transform.localEulerAngles.z;

        float b_Stomach_rotationX = X_Stomach.transform.localEulerAngles.x;
        float b_Stomach_rotationY = Y_Stomach.transform.localEulerAngles.y;
        float b_Stomach_rotationZ = Z_Stomach.transform.localEulerAngles.z;


        float b_Upper_L_rotationX = X_Upper_L.transform.localEulerAngles.x;
        float b_Upper_L_rotationY = Y_Upper_L.transform.localEulerAngles.y;
        float b_Upper_L_rotationZ = Z_Upper_L.transform.localEulerAngles.z;

        float b_Forearm_L_rotationX = X_Forearm_L.transform.localEulerAngles.x;
        float b_Forearm_L_rotationY = Y_Forearm_L.transform.localEulerAngles.y;
        float b_Forearm_L_rotationZ = Z_Forearm_L.transform.localEulerAngles.z;


        float b_Upper_R_rotationX = X_Upper_L.transform.localEulerAngles.x;
        float b_Upper_R_rotationY = Y_Upper_L.transform.localEulerAngles.y;
        float b_Upper_R_rotationZ = Z_Upper_L.transform.localEulerAngles.z;

        float b_Forearm_R_rotationX = X_Forearm_R.transform.localEulerAngles.x;
        float b_Forearm_R_rotationY = Y_Forearm_R.transform.localEulerAngles.y;
        float b_Forearm_R_rotationZ = Z_Forearm_R.transform.localEulerAngles.z;




        float signalX_L_Thigh = vectorAction[0];
        float signalY_L_Thigh = vectorAction[1];
        float signalZ_L_Thigh = vectorAction[2];

        float signalX_L_Shin = vectorAction[3];

        float signalX_L_Foot = vectorAction[4];
        float signalY_L_Foot = vectorAction[5];
        float signalZ_L_Foot = vectorAction[6];

        float signalX_R_Thigh = vectorAction[7];
        float signalY_R_Thigh = vectorAction[8];
        float signalZ_R_Thigh = vectorAction[9];

        float signalX_R_Shin = vectorAction[10];

        float signalX_R_Foot = vectorAction[11];
        float signalY_R_Foot = vectorAction[12];
        float signalZ_R_Foot = vectorAction[13];

        float signalX_Chest = vectorAction[14];
        float signalY_Chest = vectorAction[15];
        float signalZ_Chest = vectorAction[16];

        float signalX_L_Upper = vectorAction[17];
        float signalY_L_Upper = vectorAction[18];
        float signalZ_L_Upper = vectorAction[19];

        float signalX_R_Upper = vectorAction[20];
        float signalY_R_Upper = vectorAction[21];
        float signalZ_R_Upper = vectorAction[22];

        float signalX_Stomach = vectorAction[23];
        float signalY_Stomach = vectorAction[24];
        float signalZ_Stomach = vectorAction[25];

        float signalX_L_Forearm = vectorAction[26];
        float signalY_L_Forearm = vectorAction[27];
        float signalZ_L_Forearm = vectorAction[28];

        float signalX_R_Forearm = vectorAction[29];
        float signalY_R_Forearm = vectorAction[30];
        float signalZ_R_Forearm = vectorAction[31];


        controlTorque(X_Thigh_L, Y_Thigh_L, Z_Thigh_L, signalX_L_Thigh, signalY_L_Thigh, signalZ_L_Thigh, base_thigh_torque);
        controlTorque(X_Shin_L, Y_Shin_L, Z_Shin_L, signalX_L_Shin, 0.0f, 0.0f, base_shin_torque);
        controlTorque(X_Foot_L, Y_Foot_L, Z_Foot_L, signalX_L_Foot, signalY_L_Foot, signalZ_L_Foot, base_foot_torque);

        controlTorque(X_Thigh_R, Y_Thigh_R, Z_Thigh_R, signalX_R_Thigh, signalY_R_Thigh, signalZ_R_Thigh, base_thigh_torque);
        controlTorque(X_Shin_R, Y_Shin_R, Z_Shin_R, signalX_R_Shin, 0.0f, 0.0f, base_shin_torque);
        controlTorque(X_Foot_R, Y_Foot_R, Z_Foot_R, signalX_R_Foot, signalY_R_Foot, signalZ_R_Foot, base_foot_torque);

        controlTorque(X_Chest, Y_Chest, Z_Chest, signalX_Chest, signalY_Chest, signalZ_Chest, base_chest_torque);
        controlTorque(X_Stomach, Y_Stomach, Z_Stomach, signalX_Stomach, signalY_Stomach, signalZ_Stomach, base_stomach_torque);

        controlTorque(X_Upper_L, Y_Upper_L, Z_Upper_L, signalX_L_Upper, signalY_L_Upper, signalZ_L_Upper, base_arm_torque);
        controlTorque(X_Upper_R, Y_Upper_R, Z_Upper_R, signalX_R_Upper, signalY_R_Upper, signalZ_R_Upper, base_arm_torque);

        controlTorque(X_Forearm_L, Y_Forearm_L, Z_Forearm_L, signalX_L_Forearm, signalY_L_Forearm, signalZ_L_Forearm, base_arm_torque);
        controlTorque(X_Forearm_R, Y_Forearm_R, Z_Forearm_R, signalX_R_Forearm, signalY_R_Forearm, signalZ_R_Forearm, base_arm_torque);
    }


    private float calcSimilarity(Rigidbody rBody, Quaternion init_rotation, Vector3 vec_org)
    {
        Vector3 vec_global = init_rotation*vec_org;
        Vector3 vec_local = rBody.transform.rotation*vec_org;

        float similarity = Vector3.Dot(vec_local,vec_global)/(vec_global.magnitude*vec_local.magnitude);

        return similarity;
    }


    public override void CollectObservations(VectorSensor sensor)
    {
        // 12
        sensor.AddObservation(Head.transform.position.x - (ID-1)*distance);
        sensor.AddObservation(Head.transform.position.y);
        sensor.AddObservation(Head.transform.position.z);
        sensor.AddObservation(Head.transform.eulerAngles);
        sensor.AddObservation(Head.transform.position - Root.transform.position);
        sensor.AddObservation(Head.velocity);

        // 3
        sensor.AddObservation(Chest.transform.position.x - (ID-1)*distance);
        sensor.AddObservation(Chest.transform.position.y);
        sensor.AddObservation(Chest.transform.position.z);

        // 12
        sensor.AddObservation(Hand_L.rotation.eulerAngles);
        sensor.AddObservation(Hand_L.transform.position - Root.transform.position);
        sensor.AddObservation(Hand_R.rotation.eulerAngles);
        sensor.AddObservation(Hand_R.transform.position - Root.transform.position);


        // 15
        sensor.AddObservation(Stomach.transform.eulerAngles);
        sensor.AddObservation(Stomach.transform.position - Root.transform.position);
        sensor.AddObservation(Stomach.velocity);
        sensor.AddObservation(X_Stomach.transform.localEulerAngles.x);
        sensor.AddObservation(transform.InverseTransformDirection(X_Stomach.angularVelocity).x);
        sensor.AddObservation(Y_Stomach.transform.localEulerAngles.y);
        sensor.AddObservation(transform.InverseTransformDirection(Y_Stomach.angularVelocity).y);
        sensor.AddObservation(Z_Stomach.transform.localEulerAngles.z);
        sensor.AddObservation(transform.InverseTransformDirection(Z_Stomach.angularVelocity).z);


        // 15
        sensor.AddObservation(Chest.transform.eulerAngles);
        sensor.AddObservation(Chest.transform.position - Root.transform.position);
        sensor.AddObservation(Chest.velocity);
        sensor.AddObservation(X_Chest.transform.localEulerAngles.x);
        sensor.AddObservation(transform.InverseTransformDirection(X_Chest.angularVelocity).x);
        sensor.AddObservation(Y_Chest.transform.localEulerAngles.y);
        sensor.AddObservation(transform.InverseTransformDirection(Y_Chest.angularVelocity).y);
        sensor.AddObservation(Z_Chest.transform.localEulerAngles.z);
        sensor.AddObservation(transform.InverseTransformDirection(Z_Chest.angularVelocity).z);


        // 15
        sensor.AddObservation(Upper_L.transform.eulerAngles);
        sensor.AddObservation(Upper_L.transform.position - Root.transform.position);
        sensor.AddObservation(Upper_L.transform.localEulerAngles);
        sensor.AddObservation(X_Upper_L.transform.localEulerAngles.x);
        sensor.AddObservation(transform.InverseTransformDirection(X_Upper_L.angularVelocity).x);
        sensor.AddObservation(Y_Upper_L.transform.localEulerAngles.y);
        sensor.AddObservation(transform.InverseTransformDirection(Y_Upper_L.angularVelocity).y);
        sensor.AddObservation(Z_Upper_L.transform.localEulerAngles.z);
        sensor.AddObservation(transform.InverseTransformDirection(Z_Upper_L.angularVelocity).z);

        // 15
        sensor.AddObservation(Forearm_L.transform.eulerAngles);
        sensor.AddObservation(Forearm_L.transform.position - Root.transform.position);
        sensor.AddObservation(Forearm_L.transform.localEulerAngles);
        sensor.AddObservation(X_Forearm_L.transform.localEulerAngles.x);
        sensor.AddObservation(transform.InverseTransformDirection(X_Forearm_L.angularVelocity).x);
        sensor.AddObservation(Y_Forearm_L.transform.localEulerAngles.y);
        sensor.AddObservation(transform.InverseTransformDirection(Y_Forearm_L.angularVelocity).y);
        sensor.AddObservation(Z_Forearm_L.transform.localEulerAngles.z);
        sensor.AddObservation(transform.InverseTransformDirection(Z_Forearm_L.angularVelocity).z);


        // 15
        sensor.AddObservation(Upper_R.transform.eulerAngles);
        sensor.AddObservation(Upper_R.transform.position - Root.transform.position);
        sensor.AddObservation(Upper_R.transform.localEulerAngles);
        sensor.AddObservation(X_Upper_R.transform.localEulerAngles.x);
        sensor.AddObservation(transform.InverseTransformDirection(X_Upper_R.angularVelocity).x);
        sensor.AddObservation(Y_Upper_R.transform.localEulerAngles.y);
        sensor.AddObservation(transform.InverseTransformDirection(Y_Upper_R.angularVelocity).y);
        sensor.AddObservation(Z_Upper_R.transform.localEulerAngles.z);
        sensor.AddObservation(transform.InverseTransformDirection(Z_Upper_R.angularVelocity).z);

        // 15
        sensor.AddObservation(Forearm_R.transform.eulerAngles);
        sensor.AddObservation(Forearm_R.transform.position - Root.transform.position);
        sensor.AddObservation(Forearm_R.transform.localEulerAngles);
        sensor.AddObservation(X_Forearm_R.transform.localEulerAngles.x);
        sensor.AddObservation(transform.InverseTransformDirection(X_Forearm_R.angularVelocity).x);
        sensor.AddObservation(Y_Forearm_R.transform.localEulerAngles.y);
        sensor.AddObservation(transform.InverseTransformDirection(Y_Forearm_R.angularVelocity).y);
        sensor.AddObservation(Z_Forearm_R.transform.localEulerAngles.z);
        sensor.AddObservation(transform.InverseTransformDirection(Z_Forearm_R.angularVelocity).z);


        // 15
        sensor.AddObservation(Crotch_L.transform.eulerAngles);
        sensor.AddObservation(Crotch_L.transform.position - Root.transform.position);
        sensor.AddObservation(Crotch_L.velocity);
        sensor.AddObservation(X_Crotch_L.transform.localEulerAngles.x);
        sensor.AddObservation(transform.InverseTransformDirection(X_Crotch_L.angularVelocity).x);
        sensor.AddObservation(Y_Crotch_L.transform.localEulerAngles.y);
        sensor.AddObservation(transform.InverseTransformDirection(Y_Crotch_L.angularVelocity).y);
        sensor.AddObservation(Z_Crotch_L.transform.localEulerAngles.z);
        sensor.AddObservation(transform.InverseTransformDirection(Z_Crotch_L.angularVelocity).z);

        // 15
        sensor.AddObservation(Thigh_L.transform.eulerAngles);
        sensor.AddObservation(Thigh_L.transform.position - Root.transform.position);
        sensor.AddObservation(Thigh_L.velocity);
        sensor.AddObservation(X_Thigh_L.transform.localEulerAngles.x);
        sensor.AddObservation(transform.InverseTransformDirection(X_Thigh_L.angularVelocity).x);
        sensor.AddObservation(Y_Thigh_L.transform.localEulerAngles.y);
        sensor.AddObservation(transform.InverseTransformDirection(Y_Thigh_L.angularVelocity).y);
        sensor.AddObservation(Z_Thigh_L.transform.localEulerAngles.z);
        sensor.AddObservation(transform.InverseTransformDirection(Z_Thigh_L.angularVelocity).z);

        // 11
        sensor.AddObservation(Shin_L.transform.eulerAngles);
        sensor.AddObservation(Shin_L.transform.position - Root.transform.position);
        sensor.AddObservation(Shin_L.velocity);
        sensor.AddObservation(X_Shin_L.transform.localEulerAngles.x);
        sensor.AddObservation(transform.InverseTransformDirection(X_Shin_L.angularVelocity).x);

        // 15
        sensor.AddObservation(Foot_L.transform.eulerAngles);
        sensor.AddObservation(Foot_L.transform.position - Root.transform.position);
        sensor.AddObservation(Foot_L.velocity);
        sensor.AddObservation(X_Foot_L.transform.localEulerAngles.x);
        sensor.AddObservation(transform.InverseTransformDirection(X_Foot_L.angularVelocity).x);
        sensor.AddObservation(Y_Foot_L.transform.localEulerAngles.y);
        sensor.AddObservation(transform.InverseTransformDirection(Y_Foot_L.angularVelocity).y);
        sensor.AddObservation(Z_Foot_L.transform.localEulerAngles.z);
        sensor.AddObservation(transform.InverseTransformDirection(Z_Foot_L.angularVelocity).z);

        
        // 15
        sensor.AddObservation(Crotch_R.transform.eulerAngles);
        sensor.AddObservation(Crotch_R.transform.position - Root.transform.position);
        sensor.AddObservation(Crotch_R.velocity);
        sensor.AddObservation(X_Crotch_R.transform.localEulerAngles.x);
        sensor.AddObservation(transform.InverseTransformDirection(X_Crotch_R.angularVelocity).x);
        sensor.AddObservation(Y_Crotch_R.transform.localEulerAngles.y);
        sensor.AddObservation(transform.InverseTransformDirection(Y_Crotch_R.angularVelocity).y);
        sensor.AddObservation(Z_Crotch_R.transform.localEulerAngles.z);
        sensor.AddObservation(transform.InverseTransformDirection(Z_Crotch_R.angularVelocity).z);

        // 15
        sensor.AddObservation(Thigh_R.transform.eulerAngles);
        sensor.AddObservation(Thigh_R.transform.position - Root.transform.position);
        sensor.AddObservation(Thigh_R.velocity);
        sensor.AddObservation(X_Thigh_R.transform.localEulerAngles.x);
        sensor.AddObservation(transform.InverseTransformDirection(X_Thigh_R.angularVelocity).x);
        sensor.AddObservation(Y_Thigh_R.transform.localEulerAngles.y);
        sensor.AddObservation(transform.InverseTransformDirection(Y_Thigh_R.angularVelocity).y);
        sensor.AddObservation(Z_Thigh_R.transform.localEulerAngles.z);
        sensor.AddObservation(transform.InverseTransformDirection(Z_Thigh_R.angularVelocity).z);

        // 11
        sensor.AddObservation(Shin_R.transform.eulerAngles);
        sensor.AddObservation(Shin_R.transform.position - Root.transform.position);
        sensor.AddObservation(Shin_R.velocity);
        sensor.AddObservation(X_Shin_R.transform.localEulerAngles.x);
        sensor.AddObservation(transform.InverseTransformDirection(X_Shin_R.angularVelocity).x);

        // 15
        sensor.AddObservation(Foot_R.transform.eulerAngles);
        sensor.AddObservation(Foot_R.transform.position - Root.transform.position);
        sensor.AddObservation(Foot_R.velocity);
        sensor.AddObservation(X_Foot_R.transform.localEulerAngles.x);
        sensor.AddObservation(transform.InverseTransformDirection(X_Foot_R.angularVelocity).x);
        sensor.AddObservation(Y_Foot_R.transform.localEulerAngles.y);
        sensor.AddObservation(transform.InverseTransformDirection(Y_Foot_R.angularVelocity).y);
        sensor.AddObservation(Z_Foot_R.transform.localEulerAngles.z);
        sensor.AddObservation(transform.InverseTransformDirection(Z_Foot_R.angularVelocity).z);


        // Reward for maintain
        // if (Chest.transform.position.y < EndEpisode_hight_y)
        // {
        //     EndEpisode();
        // }
        // SetReward(1.0f);
        // SetReward(Chest.transform.position.y - EndEpisode_hight_y);


        // SetReward(Chest.transform.position.y - penalty_chest_y);

        float NeckSimilarityZ = calcSimilarity(Neck,init_Neck_rotation,new Vector3(0, 0, 1));
        float NeckSimilarityY = calcSimilarity(Neck, init_Neck_rotation, new Vector3(0, 1, 0));
        float ChestSimilarityZ = calcSimilarity(Chest,init_Chest_rotation, new Vector3(0, 0, 1));
        float CrotchSimilarityZ = calcSimilarity(Crotch_L,init_Crotch_rotation, new Vector3(0, 0, 1));
        float CrotchSimilarityY = calcSimilarity(Crotch_L, init_Crotch_rotation, new Vector3(0, 1, 0));
        float ThighSimilarityX_L = calcSimilarity(Thigh_L, init_Thigh_L_rotation, new Vector3(1, 0, 0));
        float ThighSimilarityY_L = calcSimilarity(Thigh_L, init_Thigh_L_rotation, new Vector3(0, 1, 0));
        float ThighSimilarityX_R = calcSimilarity(Thigh_R, init_Thigh_R_rotation, new Vector3(1, 0, 0));
        float ThighSimilarityY_R = calcSimilarity(Thigh_R, init_Thigh_R_rotation, new Vector3(0, 1, 0));



        // if (Chest.transform.position.y > EndEpisode_hight_y)
        // {
        //     SetReward(1.0f);
        //     SetReward(Chest.transform.position.y - EndEpisode_hight_y);
        //     if (NeckSimilarity > reward_Neck_similarity_range)
        //     {
        //         SetReward(NeckSimilarity);
        //         SetReward(Mathf.Clamp(Head.velocity.z, 0, 10));
        //     }
        // }

        float r_ChestHight = Chest.transform.position.y * reward_Chest_hight_ratio;
        float r_Vel = Mathf.Clamp(Head.velocity.z,-10,10) * reward_velocity_ratio;

        float r_NeckSim;
        if ((NeckSimilarityZ > 0) && (NeckSimilarityY > 0)){
            r_NeckSim = NeckSimilarityZ * NeckSimilarityY * reward_Neck_similarity_ratio;}
        else {
            r_NeckSim = -1 * Mathf.Abs(NeckSimilarityZ * NeckSimilarityY) * reward_Neck_similarity_ratio;}

        float r_CrotchSim;
        if ((CrotchSimilarityZ > 0) && (CrotchSimilarityY > 0)){
            r_CrotchSim = CrotchSimilarityZ * CrotchSimilarityY * reward_Crotch_similarity_ratio;}
        else{
            r_CrotchSim = -1 * Mathf.Abs(CrotchSimilarityZ * CrotchSimilarityY) * reward_Crotch_similarity_ratio;}

        float r_NeckCrotchDis = Mathf.Pow((Neck.transform.position.y - (Crotch_L.transform.position.y + Crotch_R.transform.position.y) / 2), 1) * reward_Neck_distance_ratio;
        float r_CrotchShinDis = Mathf.Pow(((Crotch_L.transform.position.y + Crotch_R.transform.position.y) / 2 - System.Math.Min(Shin_L.transform.position.y, Shin_R.transform.position.y)), 1) * reward_Shin_distance_ratio;

        float r_ShinFootDis;
        if (Shin_L.transform.position.y < Shin_R.transform.position.y){
            r_ShinFootDis = Mathf.Pow((Shin_L.transform.position.y - Foot_L.transform.position.y), 1) * reward_Foot_distance_ratio;}
        else{
            r_ShinFootDis = Mathf.Pow((Shin_R.transform.position.y - Foot_R.transform.position.y), 1) * reward_Foot_distance_ratio;}
        
        float r_NeckShinDis = r_CrotchShinDis * r_NeckCrotchDis;

        float r_NeckFootDis;
        if ((r_NeckCrotchDis > 0) && (r_CrotchShinDis > 0) && (r_ShinFootDis > 0)) {
            r_NeckFootDis = r_NeckCrotchDis * r_CrotchShinDis * r_ShinFootDis;}
        else {
            r_NeckFootDis = -1 * Mathf.Abs(r_NeckCrotchDis * r_CrotchShinDis * r_ShinFootDis);}


        SetReward(
            + r_ChestHight
            + r_Vel
            + r_NeckSim
            + r_CrotchSim
            //+ r_CrotchShinDis
            //+ r_NeckCrotchDis
            //+ r_NeckShinDis
            + r_NeckFootDis
        );

        print($"Chest hight ({reward_Chest_hight_ratio}): {r_ChestHight} | " +
                $"Head Velocity ({reward_velocity_ratio}): {r_Vel} | " +
                $"Neck Similarity ({reward_Neck_similarity_ratio}): {r_NeckSim} | " +
                $"Crotch Similarity ({reward_Crotch_similarity_ratio}): {r_CrotchSim} | " +
                $"Neck - Crotch distance (^1*{reward_Neck_distance_ratio}): {r_NeckCrotchDis} | " +
                $"Crotch - Shin distance (^1*{reward_Shin_distance_ratio}): {r_CrotchShinDis} | " +
                $"Shin - Foot distance (^1*{reward_Foot_distance_ratio}): {r_ShinFootDis} | " + "\n" +
                $"(Neck - Crotch distance)(^1*{reward_Neck_distance_ratio}) * (Crotch - Shin distance)(^1*{reward_Shin_distance_ratio}) * (Shin - Foot distance)(^1*{reward_Foot_distance_ratio}): {r_NeckFootDis}");
        

        
        // else
        // {
        //     SetReward((NeckSimilarity-reward_Neck_similarity_range)*penalty_rotation_ratio);
        // }

        // SetReward((NeckSimilarity-1)*penalty_rotation_ratio);

        // SetReward(Mathf.Clamp(Head.velocity.z, -10, 10)*reward_velocity_ratio);

        if (Head.transform.position.x > (init_Neck_position.x + penalty_range_x) && Head.velocity.x > 0)
        {
            SetReward(-Head.velocity.x);
        }
        if (Head.transform.position.x < (init_Neck_position.x - penalty_range_x) && Head.velocity.x < 0)
        {
            SetReward(Head.velocity.x);
        }
        // if (Chest.transform.position.y < penalty_chest_y && Head.velocity.y < 0)
        // {
        //     SetReward(Head.velocity.y);
        // }
        
        // print($"Chest.transform.position.y: {Chest.transform.position.y} | Neck rotation's cos Similarity: {NeckSimilarity} | Thigh_L rotation's cos Similarity: {ThighSimilarity_L} | Thigh_R rotation's cos Similarity: {ThighSimilarity_R}");
        // print($"Chest rotation's cos Similarity: {ChestSimilarity}");
        
        if (Head.transform.position.y < 0.0 || Chest.transform.position.y < 0.0 ||
            Upper_L.transform.position.y < 0.0 || Forearm_L.transform.position.y < 0.0 || 
            Hand_L.transform.position.y < 0.0 || 
            Upper_R.transform.position.y < 0.0 || Forearm_R.transform.position.y < 0.0 || 
            Hand_R.transform.position.y < 0.0 || 
            Thigh_L.transform.position.y < 0.0 || Shin_L.transform.position.y < 0.0 ||
            Foot_L.transform.position.y < 0.0 ||
            Thigh_R.transform.position.y < 0.0 || Shin_R.transform.position.y < 0.0 ||
            Foot_R.transform.position.y < 0.0)
        {
            EndEpisode();
        }

        if (Head.velocity.magnitude > vel_threshold)
        {
            EndEpisode();
        }

        // print($"coutup: {countup}");
        countup += Time.deltaTime;
        if (countup >= timeLimit)
        {
            EndEpisode();
        }
    }

    public override float[] Heuristic()
    {  
        var action = new float[32];
        
        return action;
    }
}
