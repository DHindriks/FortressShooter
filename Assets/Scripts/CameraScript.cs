﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{

    public GameObject FollowTarget;

    [HideInInspector]
    public float PosLerpSpeed = 1;
    float LookLerpspeed = 1f;

    Quaternion TargetRotation;
    Coroutine LastRoutine;

    Camera MainCamera;
    public GameObject LevelCenter; //position to have overview over the whole level, camera defaults to this.
    [HideInInspector]
    public float AxisLerpamount;
    float LookLerpamount;

    // Use this for initialization
    void Start()
    {
        if (!LevelCenter) // if levelcenter has not been assigned create one at cam starting pos.
        {
            LevelCenter = new GameObject("Level center(Generated by CameraScript)");
            LevelCenter.transform.position = transform.position;
        }
        MainCamera = gameObject.GetComponentInChildren<Camera>();
        SetcamPos();
        ChangeZoom();
        GameManager.Instance.cameraScript = this;
    }

    public void SetTarget(GameObject Target)
    {
        FollowTarget = Target;
    }

    public void SetcamPos(GameObject Postarget = null, GameObject Lookttarget = null, float Axislerp = 0, float LookLerp = 0, float PosSpeed = 5f, float RotSpeed = 0f)
    {
        if (Postarget == null && LevelCenter != null)
        {
            Postarget = LevelCenter;
        }
        if (Lookttarget == null && LevelCenter != null)
        {
            Lookttarget = LevelCenter;
        }
        FollowTarget = Postarget;
        AxisLerpamount = Axislerp;
        LookLerpamount = LookLerp;
        PosLerpSpeed = PosSpeed;
        LookLerpspeed = RotSpeed;
    }

    public void ChangeFOV(float FOV = 60, float LerpTime = 2)
    {
        if (LastRoutine != null)
        {
            StopCoroutine(LastRoutine);
        }
        LastRoutine = StartCoroutine(LerpFOV(FOV, LerpTime));
    }

    public void ChangeZoom(float Zoom = -60, float LerpTime = 2)
    {
        if (LastRoutine != null)
        {
            StopCoroutine(LastRoutine);
        }
        LastRoutine = StartCoroutine(LerpZoom(Zoom, LerpTime));
    }

    IEnumerator LerpFOV(float FOV, float LerpTime)
    {
        float Rate = 1.0f / LerpTime;
        float i = 0;

        while (i < 1)
        {
            i += Time.deltaTime * Rate;
            MainCamera.fieldOfView = Mathf.Lerp(MainCamera.fieldOfView, FOV, i);
            yield return 0;
        }

        MainCamera.fieldOfView = FOV;
    }

    IEnumerator LerpZoom(float Zoom, float LerpTime)
    {
        float Rate = 1.0f / LerpTime;
        float i = 0;

        Vector3 pos = new Vector3();
        pos = MainCamera.transform.localPosition;

        while (i < 1)
        {
            i += Time.deltaTime * Rate;
            pos.z = Mathf.Lerp(pos.z, Zoom, i);
            MainCamera.gameObject.transform.localPosition = pos;
            yield return 0;
        }

        pos.z = Zoom;
        MainCamera.gameObject.transform.localPosition = pos;
    }

    public void ResetCam()
    {
        CancelInvoke("ResetCam");
        SetcamPos();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(FollowTarget != null)
        {
            this.transform.position = Vector3.Lerp(this.transform.position, new Vector3(Vector3.Lerp(FollowTarget.transform.position, this.transform.GetChild(0).transform.localPosition, AxisLerpamount).x, FollowTarget.transform.position.y, FollowTarget.transform.position.z), PosLerpSpeed * Time.deltaTime);
            //this.transform.GetChild(0).rotation = Quaternion.Slerp(this.transform.GetChild(0).rotation, Quaternion.Lerp(TargetRotation, transform.rotation, LookLerpamount), LookLerpspeed * Time.deltaTime);
        }else if (GameManager.Instance.MainSlingshot != null) 
        {
            Invoke("ResetCam", 1);
        }
    }
}
