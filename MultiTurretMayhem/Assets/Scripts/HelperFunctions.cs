using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelperFunctions{

    public static int randomIndex(List<float> probabilities)
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

    public static float sumOfList(List<float> list)
    {
        float result = 0.0f;
        foreach (float f in list)
            result += f;
        return result;
    }
}
