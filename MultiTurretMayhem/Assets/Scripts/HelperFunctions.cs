using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelperFunctions{

    public static int randomIndex(List<float> probabilities) // Pick a random index in a list of floats weighted by the contents in the list
    {
        float selection = Random.Range(0.0f, sumOfList(probabilities));
        for (int i = 0; i < probabilities.Count; ++i)
        {
            if (selection < probabilities[i])
                return i;
            selection -= probabilities[i];
        }
        return -1;
    }

    public static int randomIndex(List<Vector2> probabilities) // Pick a random index in a list of vector2 weighted by the contents in the list
    {
        float selection = Random.Range(0.0f, sumOfList(probabilities));
        for (int i = 0; i < probabilities.Count; ++i)
        {
            if (selection < probabilities[i].y - probabilities[i].x)
                return i;
            selection -= probabilities[i].y - probabilities[i].x;
        }
        return -1;
    }

    public static float sumOfList(List<float> list) // Return the sum of a list of floats
    {
        float result = 0.0f;
        foreach (float f in list)
            result += f;
        return result;
    }

    public static float sumOfList(List<Vector2> list) // Return the sum of a list of Vector2 (if the vector2 is a range)
    {
        float result = 0.0f;
        foreach (Vector2 v in list)
            result += v.y - v.x;
        return result;
    }

    public static Vector3 lineVector(float angle, float magnitude = 1)
    {
        return magnitude * new Vector3(Mathf.Cos(angle * Mathf.PI / 180), Mathf.Sin(angle * Mathf.PI / 180));
    }

    public static float directionBetween(Vector2 v1, Vector2 v2)
    {
        return Mathf.Atan2(v2.y - v1.y, v2.x - v1.x) * 180 / Mathf.PI;
    }

    public static Vector3 objectCameraConvert(Vector3 pos, GameObject can, Camera cam)
    {
        return pos * can.GetComponent<RectTransform>().rect.height/2 / cam.orthographicSize;
    }

    public static float friendlySqrt(float num)
    {
        return (num < 0 ? -Mathf.Sqrt(-num) : Mathf.Sqrt(num));
    }

    public static Color colorInterpolation(Color c1, Color c2, float percent)
    {
        return new Color(c1.r * percent + c2.r * (1 - percent), c1.g * percent + c2.g * (1 - percent), c1.b * percent + c2.b * (1 - percent), c1.a * percent + c2.a * (1 - percent));
    }

    public static void playSound(ref AudioSource source, AudioClip clip)
    {
        source.clip = clip;
        source.Play();
    }
}
