using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turtle
{
    struct State
    {
        public Vector3 position;
        public Vector3 translation;
    }

    private LinkedList<State> savedStates;
    public Vector3 CrtPosition { get; private set; }
    public Vector3 CrtTranslation { get; private set; }

    public Turtle(Vector3 startPosition, Vector3 startTranslation)
    {
        savedStates = new LinkedList<State>();
        CrtPosition = startPosition;
        CrtTranslation = startTranslation;
    }

    public void Push()
    {
        State s = new State();
        s.position = CrtPosition;
        s.translation = CrtTranslation;
        savedStates.AddLast(s);
    }

    public void Pull()
    {
        State s = savedStates.Last.Value;
        savedStates.RemoveLast();
        CrtPosition = s.position;
        CrtTranslation = s.translation;
    }

    public void Rotate(int i, float alpha, bool d = false)
    {
        float[,] rM = GetRotationMatrix(i, alpha);
        
        if(d)
        {
            string s = "";
            for (int j = 0; j < rM.GetLength(0); j++)
            {
                for (int k = 0; k < rM.GetLength(1); k++)
                {
                    s += rM[j, k] + " ";
                }
                s += " | ";
            }
            Debug.Log(s);
        }

        CrtTranslation = GetOrientedTranslation(CrtTranslation, rM);
    }

    public void Translate()
    {
        CrtPosition += CrtTranslation;
    }

    public void MultiplyTranslation(float f)
    {
        CrtTranslation *= f;
    }

    private Vector3 GetOrientedTranslation(Vector3 crtTranslation, float[,] rM)
    {
        float[] tArr = new float[] { crtTranslation.x, crtTranslation.y, crtTranslation.z };
        float[] newTArr = new float[3];

        for (int j = 0; j < rM.GetLength(1); j++)
        {
            float s = 0;
            for (int k = 0; k < tArr.Length; k++)
            {
                s += rM[k, j] * tArr[k];
            }
            newTArr[j] = s;
        }

        return new Vector3(newTArr[0], newTArr[1], newTArr[2]);
    }

    private float[,] GetRotationMatrix(int i, float alpha)
    {
        float alphaRad = Mathf.Deg2Rad * alpha;

        switch (i)
        {
            case 0:
                return new float[,] { 
                    { 1, 0, 0 }, 
                    { 0, Mathf.Cos(alphaRad), -Mathf.Sin(alphaRad) },
                    { 0, Mathf.Sin(alphaRad), Mathf.Cos(alphaRad) }
                };
            case 1:
                return new float[,] {
                    { Mathf.Cos(alphaRad), 0, -Mathf.Sin(alphaRad) },
                    { 0, 1, 0 },
                    { Mathf.Sin(alphaRad), 0, Mathf.Cos(alphaRad) }
                };
            case 2:
                return new float[,] {
                    { Mathf.Cos(alphaRad), Mathf.Sin(alphaRad), 0 },
                    { -Mathf.Sin(alphaRad), Mathf.Cos(alphaRad), 0 },
                    { 0, 0, 1 }
                };
            default:
                return null;
        }
    }
}
