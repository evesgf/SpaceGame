using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamRig : MonoBehaviour
{
    public bool usMouseCtrl;

    public Transform target;
    public Vector3 offset;
    public float distance = 5.0f;
    public float xSpeed = 120.0f;
    public float ySpeed = 120.0f;

    public float yMinLimit = -60f;
    public float yMaxLimit = 60f;

    public float distanceMin = 2f;
    public float distanceMax = 15f;

    float x = 0.0f;
    float y = 0.0f;

    public void SetTargetDis()
    {
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;
        Vector3 p = new Vector3(0.0f, 3.0f, -distance) + target.position + offset;
        transform.position = p;
        transform.LookAt(target);
    }

    // Use this for initialization
    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;

        Input.imeCompositionMode= IMECompositionMode.Off;
    }

    internal bool isDis;

    void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            isDis = true;
        }
        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            isDis = false;
        }

        if (target)
        {


            //Gesture current = EasyTouch.current;

            //// Swipe
            //if (current!=null && current.type == EasyTouch.EvtType.On_Swipe && current.touchCount == 1)
            //{
            //    x += current.deltaPosition.x / Screen.width * distance * xSpeed;
            //    y -= current.deltaPosition.y / Screen.height * ySpeed ;
            //}

            if (isDis)
            {
                SetTargetDis();
            }
            else
            {
                x += Input.GetAxis("Mouse X") / Screen.width * distance * xSpeed;
                y -= Input.GetAxis("Mouse Y") / Screen.height * ySpeed;

                y = ClampAngle(y, yMinLimit, yMaxLimit);

                Quaternion rotation = Quaternion.Euler(y, x, 0);

                // 围绕着这根轴进行偏移
                Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
                // 先旋转再平移 (rotation * negDistance 返回的是旋转之后的位置)
                Vector3 position = rotation * negDistance + target.position + offset;

                transform.rotation = rotation;
                transform.position = position;

                Debug.DrawLine(transform.position, target.position, Color.red);
            }

            

        }
    }

    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }
}

