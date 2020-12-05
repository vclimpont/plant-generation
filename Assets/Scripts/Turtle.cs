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
        public float width;
    }

    private LinkedList<State> savedStates;
    public Vector3 CrtPosition { get; private set; }
    public float CrtTranslation { get; private set; }
    public float CrtTheta { get; private set; }
    public float CrtPhi { get; private set; }
    public float CrtWidth { get; private set; }

    public Turtle(Vector3 startPosition, float startTranslation, float startTheta, float startPhi, float startWidth)
    {
        savedStates = new LinkedList<State>();
        CrtPosition = startPosition;
        CrtTranslation = startTranslation;
        CrtTheta = startTheta;
        CrtPhi = startPhi;
        CrtWidth = startWidth;
    }

    public void Push()
    {
        State s = new State();
        s.position = CrtPosition;
        s.translation = CrtTranslation;
        s.theta = CrtTheta;
        s.phi = CrtPhi;
        s.width = CrtWidth;
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
        CrtWidth = s.width;
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
        CrtPosition += GetRotatedTranslation();
    }

    public Vector3 GetRotatedTranslation()
    {
        return new Vector3(
            CrtTranslation * Mathf.Sin(CrtPhi * Mathf.Deg2Rad) * Mathf.Cos(CrtTheta * Mathf.Deg2Rad),
            CrtTranslation * Mathf.Sin(CrtPhi * Mathf.Deg2Rad) * Mathf.Sin(CrtTheta * Mathf.Deg2Rad),
            CrtTranslation * Mathf.Cos(CrtPhi * Mathf.Deg2Rad));
    }

    public void MultiplyWidth(float f)
    {
        CrtWidth *= f;
    }
}
