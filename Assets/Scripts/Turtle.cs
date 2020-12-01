using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turtle
{
    struct State
    {
        public Vector3 position;
        public float translation;
        public float rotation;
    }

    private LinkedList<State> savedStates;
    public Vector3 CrtPosition { get; private set; }
    public float CrtTranslation { get; private set; }
    public float CrtRotation { get; private set; }

    public Turtle(Vector3 startPosition, float startTranslation, float startRotation)
    {
        savedStates = new LinkedList<State>();
        CrtPosition = startPosition;
        CrtTranslation = startTranslation;
        CrtRotation = startRotation;
    }

    public void Push()
    {
        State s = new State();
        s.position = CrtPosition;
        s.translation = CrtTranslation;
        s.rotation = CrtRotation;
        savedStates.AddLast(s);
    }

    public void Pull()
    {
        State s = savedStates.Last.Value;
        savedStates.RemoveLast();
        CrtPosition = s.position;
        CrtTranslation = s.translation;
        CrtRotation = s.rotation;
    }

    public void Rotate(float alpha)
    {
        CrtRotation += alpha;
    }

    public void Translate()
    {
        CrtPosition += new Vector3(
            CrtTranslation * Mathf.Cos(CrtRotation * Mathf.Deg2Rad),
            CrtTranslation * Mathf.Sin(CrtRotation * Mathf.Deg2Rad),
            0);
    }

    public void MultiplyTranslation(float f)
    {
        CrtTranslation *= f;
    }
}
