﻿using System.Collections;
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

    public static float sumOfList(List<float> list) // Return the sum of a list of floats
    {
        float result = 0.0f;
        foreach (float f in list)
            result += f;
        return result;
    }

    public static Vector3 lineVector(float angle, float magnitude = 1)
    {
        return magnitude * new Vector3(Mathf.Cos(angle * Mathf.PI / 180), Mathf.Sin(angle * Mathf.PI / 180));
    }
}
