using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Form Forge3D F3DTurretEditor
/// </summary>
namespace GPL
{
    [CustomEditor(typeof(NormalTurretCtrl))]
    public class NormalTurretCtrlEditor : Editor
    {
        public float arrowSize = 1f;
        NormalTurretCtrl turret;

        void OnEnable()
        {
            if (target == null)
                return;
            turret = target as NormalTurretCtrl;
        }

        void OnSceneGUI()
        {
            if (turret == null)
                return;
            //if (turret.destroyIt)
            //{
            //    DestroyImmediate(turret);
            //    return;
            //}
            Color newC = Color.red;
            newC.a = 0.1f;
            Handles.color = newC;
            //drawing horizontal arc:
            if (turret.swivel != null && turret.barrel != null && !Application.isPlaying)
            {
                //Left arrow
                Transform mount = turret.barrel;
                Vector3 need = mount.forward;
                Vector3 rotated = Quaternion.AngleAxis(turret.HeadingLimit.x, mount.up) * need;
                float angles = turret.HeadingLimit.y - turret.HeadingLimit.x;
                Handles.DrawSolidArc(mount.position, mount.up, rotated, angles, 5);
                newC = Color.red;
                newC.a = 0.5f;
                Handles.color = newC;
                Handles.DrawWireArc(mount.position, mount.up, rotated, angles, 5);
                newC = Color.red;
                newC.a = 0.5f;
                Handles.color = newC;
                Vector3 lookRotation = rotated - mount.position;
                lookRotation.y = 0;
                turret.HeadingLimit.x = TurretHandles.AngleSlider(mount.position, mount.up, mount.forward, turret.HeadingLimit.x, 5f, TurretHandles.CircleArrow, true, mount.rotation);
                CheckHorizontalValuesMin();

                //Right arrow
                rotated = Quaternion.AngleAxis(turret.HeadingLimit.y, mount.up) * need;
                lookRotation = rotated - mount.position;
                lookRotation.y = 0;
                turret.HeadingLimit.y = TurretHandles.AngleSlider(mount.position, mount.up, mount.forward, turret.HeadingLimit.y, 5f, TurretHandles.CircleArrow, true, mount.rotation);
                CheckHorizontalValuesMax();
                CheckForInputedH();

                //drawing vertical arc 
                Transform barell = turret.barrel;
                newC = Color.blue;
                newC.a = 0.1f;
                Handles.color = newC;
                need = mount.forward;

                //BOTTOM ARROW
                rotated = Quaternion.AngleAxis(turret.ElevationLimit.x, barell.right) * need;
                angles = turret.ElevationLimit.y - turret.ElevationLimit.x;
                Handles.DrawSolidArc(barell.position, barell.right, rotated, angles, 5);
                newC = Color.blue;
                newC.a = 0.5f;
                Handles.color = newC;
                Handles.DrawWireArc(barell.position, barell.right, rotated, angles, 5);
                newC = Color.blue;
                newC.a = 0.5f;
                Handles.color = newC;

                lookRotation = rotated;
                turret.ElevationLimit.x = TurretHandles.AngleSlider(barell.position, barell.right, barell.forward, turret.ElevationLimit.x, 5f, TurretHandles.CircleArrow, false, barell.rotation);
                CheckVerticalValuesMin();

                //TOP ARROW
                rotated = Quaternion.AngleAxis(turret.ElevationLimit.y, barell.right) * need;
                lookRotation = rotated;
                turret.ElevationLimit.y = TurretHandles.AngleSlider(barell.position, barell.right, barell.forward, turret.ElevationLimit.y, 5f, TurretHandles.CircleArrow, false, barell.rotation);
                CheckVerticalValuesMax();
                CheckForInputedV();
            }
        }

        void CheckForInputedH()
        {
            float min = turret.HeadingLimit.x;
            float max = turret.HeadingLimit.y;

            if (min < 0 && max < 0)
            {
                if (min < -340 && max > -340)
                {
                    min += 360;
                    max += 360;
                }
                else if (min > -340 && max < -340)
                {
                    min += 360;
                    max += 360;
                }
            }
            else if (max > 0 && min > 0)
            {
                if (max > 340 && min < 340)
                {
                    max -= 360;
                    min -= 360;
                }
                else if (max < 340 && min > 340)
                {
                    max -= 360;
                    min -= 360;
                }
            }
            turret.HeadingLimit.x = min;
            turret.HeadingLimit.y = max;
        }

        void CheckForInputedV()
        {
            float min = turret.ElevationLimit.x;
            float max = turret.ElevationLimit.y;
            if (min < 0 && max < 0)
            {
                if (min < -340 && max > -340)
                {
                    min += 360;
                    max += 360;
                }
                else if (min > -340 && max < -340)
                {
                    min += 360;
                    max += 360;
                }
            }
            else if (max > 0 && min > 0)
            {
                if (max > 340 && min < 340)
                {
                    max -= 360;
                    min -= 360;
                }
                else if (max < 340 && min > 340)
                {
                    max -= 360;
                    min -= 360;
                }
            }
            turret.ElevationLimit.x = min;
            turret.ElevationLimit.y = max;
        }

        void CheckVerticalValuesMin()
        {

            if (turret.ElevationLimit.y < turret.ElevationLimit.x)
            {
                turret.ElevationLimit.x = turret.ElevationLimit.y;
            }
            if (turret.ElevationLimit.x <= 0)
            {
                if (turret.ElevationLimit.y - turret.ElevationLimit.x > 360)
                {
                    turret.ElevationLimit.x = -(360 - turret.ElevationLimit.y);
                }
            }
            else
            {
                if (turret.ElevationLimit.y - turret.ElevationLimit.x > 360)
                {
                    turret.ElevationLimit.x = 360 - turret.ElevationLimit.y;
                }
            }
        }

        void CheckVerticalValuesMax()
        {

            if (turret.ElevationLimit.y < turret.ElevationLimit.x)
            {
                turret.ElevationLimit.y = turret.ElevationLimit.x;
            }
            if (turret.ElevationLimit.x <= 0)
            {

                if (turret.ElevationLimit.y - turret.ElevationLimit.x > 360)
                {
                    turret.ElevationLimit.y = 360 + turret.ElevationLimit.x;
                }
            }
            else
            {
                if (turret.ElevationLimit.y - turret.ElevationLimit.x > 360)
                {
                    turret.ElevationLimit.y = 360 + turret.ElevationLimit.x;
                }
            }
        }

        void CheckHorizontalValuesMin()
        {
            if (turret.HeadingLimit.y < turret.HeadingLimit.x)
            {
                turret.HeadingLimit.x = turret.HeadingLimit.y;
            }
            if (turret.HeadingLimit.x <= 0)
            {
                if (turret.HeadingLimit.y - turret.HeadingLimit.x > 360)
                {
                    turret.HeadingLimit.x = -(360 - turret.HeadingLimit.y);
                }
            }
            else
            {
                if (turret.HeadingLimit.y - turret.HeadingLimit.x > 360)
                {
                    turret.HeadingLimit.x = 360 - turret.HeadingLimit.y;
                }
            }
        }
        void CheckHorizontalValuesMax()
        {

            if (turret.HeadingLimit.y < turret.HeadingLimit.x)
            {
                turret.HeadingLimit.y = turret.HeadingLimit.x;
            }
            if (turret.HeadingLimit.x <= 0)
            {

                if (turret.HeadingLimit.y - turret.HeadingLimit.x > 360)
                {
                    turret.HeadingLimit.y = 360 + turret.HeadingLimit.x;
                }
            }
            else
            {
                if (turret.HeadingLimit.y - turret.HeadingLimit.x > 360)
                {
                    turret.HeadingLimit.y = 360 + turret.HeadingLimit.x;
                }
            }
        }
    }
}