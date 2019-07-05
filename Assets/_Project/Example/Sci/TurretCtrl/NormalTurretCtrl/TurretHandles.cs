using UnityEngine;
using UnityEditor;

/// <summary>
/// Form Forge3D F3DTurretHandles
/// </summary>
namespace GPL
{
    public class TurretHandles
    {
        public static float AngleSlider(Vector3 centrP, Vector3 normalV, Vector3 forward, float prevAngle, float radius, Handles.DrawCapFunction capF, bool horizontal, Quaternion rot)
        {
            int controlId = GUIUtility.GetControlID(FocusType.Passive);
            Vector3 startPos = Quaternion.AngleAxis(prevAngle, normalV) * (forward * radius) + centrP;
            float controlSize = HandleUtility.GetHandleSize(startPos) / 5.0f;
            float newAngle = prevAngle;
            switch (Event.current.GetTypeForControl(controlId))
            {
                // Cause mouse dragging
                case EventType.MouseDrag:
                    if (GUIUtility.hotControl == controlId)
                    {
                        Plane raycastablePlane = new Plane(normalV, centrP);
                        float dist = 0f;
                        Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
                        ray.origin = Handles.matrix.inverse.MultiplyPoint3x4(ray.origin);
                        ray.direction = Handles.matrix.inverse.MultiplyVector(ray.direction);
                        raycastablePlane.Raycast(ray, out dist);
                        Vector3 closestPoint = ray.GetPoint(dist);
                        Vector3 direction = closestPoint - centrP;
                        newAngle = Vector3.Angle(forward, direction);
                        if (horizontal)
                        {
                            if (Vector3.Cross(forward, direction).y < 0)
                            {
                                if (prevAngle < 90f)
                                {
                                    newAngle = -newAngle;
                                }
                                else
                                {
                                    newAngle = 360 - Mathf.Abs(newAngle);
                                }
                            }
                            else
                            {
                                if (prevAngle < -120f)
                                {
                                    newAngle = -180 - (180 - newAngle);
                                }
                                else if (prevAngle > 330f)
                                {
                                    newAngle = newAngle + 360f;
                                }
                            }
                        }
                        else
                        {
                            if (Vector3.Cross(forward, direction).x < 0)
                            {
                                if (prevAngle < 90f)
                                {
                                    newAngle = -newAngle;
                                }
                                else
                                {
                                    newAngle = 360 - Mathf.Abs(newAngle);
                                }
                            }
                            else
                            {
                                if (prevAngle < -130f)
                                {
                                    newAngle = -180 - (180 - newAngle);
                                }
                            }
                        }
                        GUI.changed = true;
                        Event.current.Use();
                    }
                    break;
                case EventType.Layout:
                    HandleUtility.AddControl(controlId, HandleUtility.DistanceToCircle(startPos, controlSize));
                    break;
                case EventType.Repaint:
                    Color oldCol = Handles.color;
                    if (GUIUtility.hotControl == controlId)
                    {
                        Handles.color = Color.yellow;
                    }
                    if (!horizontal)
                    {
                        Vector3 rot2 = rot.eulerAngles;
                        rot2.z += 90f;
                        capF(controlId, startPos, Quaternion.Euler(rot2), controlSize);
                    }
                    else
                    {
                        capF(controlId, startPos, rot, controlSize);
                    }
                    Handles.color = oldCol;
                    break;
                case EventType.MouseDown:
                    if ((HandleUtility.nearestControl == controlId) && (Event.current.button == 0))
                    {
                        GUIUtility.hotControl = controlId;
                        Event.current.Use();
                    }
                    break;
                case EventType.MouseUp:
                    if (GUIUtility.hotControl == controlId)
                    {
                        GUIUtility.hotControl = 0;
                        Event.current.Use();
                    }
                    break;
            }
            return newAngle;
        }
        //DRAWING CIRCLE
        private static float circleScale = 1.0f / 4.0f;
        private static Vector3[] circlePoints =
        {
        new Vector3( -4, 0, 0 ) * circleScale,
        new Vector3( -3, 0, 2 ) * circleScale,
        new Vector3( -2, 0, 3 ) * circleScale,
        new Vector3( 0, 0, 4 ) * circleScale,
        new Vector3( 2, 0, 3 ) * circleScale,
        new Vector3( 3, 0, 2 ) * circleScale,
        new Vector3( 4, 0, 0 ) * circleScale,
        new Vector3( 3, 0, -2 ) * circleScale,
        new Vector3( 2, 0, -3 ) * circleScale,
        new Vector3( 0, 0, -4 ) * circleScale,
        new Vector3( -2, 0, -3 ) * circleScale,
        new Vector3( -3, 0, -2 ) * circleScale,
        new Vector3( -4, 0, 0 ) * circleScale,
    };
        public static void CircleArrow(int controlId, Vector3 position, Quaternion rotation, float size)
        {
            Vector3[] dots = new Vector3[circlePoints.Length];
            System.Array.Copy(circlePoints, dots, circlePoints.Length);
            for (int i = 0; i < circlePoints.Length; ++i)
            {
                dots[i] = (rotation * (dots[i] * size)) + position;
            }

            Handles.DrawPolyLine(dots);
        }
        //Drawing start
        private static float starScale = 1.0f / 8.0f;
        private static Vector3[] startPoints =
        {
        new Vector3( 0, 0, 8 ) * starScale,
        new Vector3( 2, 0, 4 ) * starScale,
        new Vector3( 5, 0, 6 ) * starScale,
        new Vector3( 4, 0, 2 ) * starScale,
        new Vector3( 8, 0, 0 ) * starScale,
        new Vector3( 4, 0, -2 ) * starScale,
        new Vector3( 5, 0, -6 ) * starScale,
        new Vector3( 2, 0, -4 ) * starScale,
        new Vector3( 0, 0, -8 ) * starScale,
        new Vector3( -2, 0, -4 ) * starScale,
        new Vector3( -5, 0, -6 ) * starScale,
        new Vector3( -4, 0, -2 ) * starScale,
        new Vector3( -8, 0, 0 ) * starScale,
        new Vector3( -4, 0, 2 ) * starScale,
        new Vector3( -5, 0, 6 ) * starScale,
        new Vector3( -2, 0, 4 ) * starScale,
        new Vector3( 0, 0, 8 ) * starScale,
    };
        public static void StarArrow(int controlId, Vector3 position, Quaternion rotation, float size)
        {
            Vector3[] dots = new Vector3[startPoints.Length];
            System.Array.Copy(startPoints, dots, startPoints.Length);
            for (int i = 0; i < startPoints.Length; ++i)
            {
                dots[i] = (rotation * (dots[i] * size)) + position;
            }
            Handles.DrawPolyLine(dots);
        }
    }
}
