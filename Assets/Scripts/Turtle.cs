using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turtle
{
    struct State
    {
        public Vector3 position;
        public float translation;
        public float theta;
        public float phi;
    }

    private LinkedList<State> savedStates;
    public Vector3 CrtPosition { get; private set; }
    public float CrtTranslation { get; private set; }
    public float CrtTheta { get; private set; }
    public float CrtPhi { get; private set; }

    public Turtle(Vector3 startPosition, float startTranslation, float startTheta, float startPhi)
    {
        savedStates = new LinkedList<State>();
        CrtPosition = startPosition;
        CrtTranslation = startTranslation;
        CrtTheta = startTheta;
        CrtPhi = startPhi;
    }

    public void Push()
    {
        State s = new State();
        s.position = CrtPosition;
        s.translation = CrtTranslation;
        s.theta = CrtTheta;
        s.phi = CrtPhi;
        savedStates.AddLast(s);
    }

    public void Pull()
    {
        State s = savedStates.Last.Value;
        savedStates.RemoveLast();
        CrtPosition = s.position;
        CrtTranslation = s.translation;
        CrtTheta = s.theta;
        CrtPhi = s.phi;
    }

    public void RotateTheta(float alpha)
    {
        CrtTheta += alpha;
    }

    public void RotatePhi(float alpha)
    {
        CrtPhi += alpha;
    }

    public void Translate()
    {
        CrtPosition += new Vector3(
            CrtTranslation * Mathf.Sin(CrtPhi * Mathf.Deg2Rad) * Mathf.Cos(CrtTheta * Mathf.Deg2Rad),
            CrtTranslation * Mathf.Sin(CrtPhi * Mathf.Deg2Rad) * Mathf.Sin(CrtTheta * Mathf.Deg2Rad),
            CrtTranslation * Mathf.Cos(CrtPhi * Mathf.Deg2Rad));
    }

    public void MultiplyTranslation(float f)
    {
        CrtTranslation *= f;
    }

    float[,] GetRotationMatrix(int i, float alpha)
    {
        switch (i)
        {
            case 0:
                return new float[,] { 
                    { 1, 0, 0 }, 
                    { 0, Mathf.Cos(alpha), -Mathf.Sin(alpha) },
                    { 0, Mathf.Sin(alpha), Mathf.Cos(alpha) }
                };
            case 1:
                return new float[,] {
                    { Mathf.Cos(alpha), 0, -Mathf.Sin(alpha) },
                    { 0, 1, 0 },
                    { Mathf.Sin(alpha), 0, Mathf.Cos(alpha) }
                };
            case 2:
                return new float[,] {
                    { Mathf.Cos(alpha), Mathf.Sin(alpha), 0 },
                    { -Mathf.Sin(alpha), Mathf.Cos(alpha), 0 },
                    { 0, 0, 1 }
                };
            default:
                return null;
        }
    }
}
