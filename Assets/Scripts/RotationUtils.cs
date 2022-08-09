using System;
using System.Collections;
using UnityEngine;

public class RotationUtils
{
    public const int ROTATED_BOTTOM = 0;
    public const int ROTATED_RIGHT = 90;
    public const int ROTATED_TOP = 180;
    public const int ROTATED_LEFT = 270;

    public static IEnumerator RotateCamera(GameObject objectToRotate, Vector3 angles, float duration, Action action)
    {
        Quaternion startRotation = objectToRotate.transform.rotation;
        Quaternion endRotation = Quaternion.Euler(angles) * startRotation;
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            objectToRotate.transform.rotation = Quaternion.Lerp(startRotation, endRotation, t / duration);
            yield return null;
        }
        objectToRotate.transform.rotation = endRotation;
        action();
    }
}
