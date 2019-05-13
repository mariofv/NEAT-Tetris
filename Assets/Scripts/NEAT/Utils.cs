using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils {
    private static System.Random random = null;

    public static double sigmoid(double x)
    {
        return 1f / (1 + Mathf.Exp((float)-x)); // Utilizar tangente hiperbolica
                                    // Exp de Z - Exp -Z / Exp de Z + Exp de -Z facilita aprendizaje y tiene media 0.
    }

    public static double hyperbolicTangent(double x)
    {
        return ( Mathf.Exp((float)x) - Mathf.Exp((float)-x) ) / (Mathf.Exp((float)x) + Mathf.Exp((float)-x));
    }

    public static int generateRandomNumber()
    {
        if (random == null)
        {
            random = new System.Random();
        }

        return random.Next(0, 101);
    }

    public static int generateRandomNumber(int min, int max)
    {
        if (random == null)
        {
            random = new System.Random();
        }

        return random.Next(min, max);
    }

    public static int generateRandomNumber(int max)
    {
        if (random == null)
        {
            random = new System.Random();
        }

        return random.Next(0, max);
    }

    public static double randomWeight()
    {
        if (random == null)
        {
            random = new System.Random();
        }

        double u1 = 1.0 - random.NextDouble(); //uniform(0,1] random doubles
        double u2 = 1.0 - random.NextDouble();
        double randStdNormal = System.Math.Sqrt(-2.0 * System.Math.Log(u1)) * System.Math.Sin(2.0 * System.Math.PI * u2); //random normal(0,1)
        double randNormal = (1 / System.Math.Sqrt(NEAT.Instance.parameters.NET_INPUTS)) * randStdNormal; //random normal(mean,stdDev^2)
        return randNormal;
    }
}
