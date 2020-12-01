using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LSystem : MonoBehaviour
{
    public GameObject branchPrefab;
    public int n;
    public float angle;
    public string axiom;

    private List<Rule> rules;
    private string sentence;
    private Turtle turtle;

    // Start is called before the first frame update
    void Start()
    {
        rules = new List<Rule>();
        turtle = new Turtle(new Vector3(0, 0, -5), 1f, 90f);

        Rule r1 = new Rule('X', new string[] { "F[+X][-X]FX" });
        rules.Add(r1);
        Rule r2 = new Rule('F', new string[] { "FF" });
        rules.Add(r2);

        sentence = axiom;

        for (int i = 0; i < n; i++)
        {
            Generate();
        }

        MoveTurtle();
    }

    void Generate()
    {
        string nextSentence = "";

        foreach (char ch in sentence)
        {
            nextSentence += GetSubInRules(ch);
        }

        sentence = nextSentence;
    }

    void MoveTurtle()
    {
        foreach (char ch in sentence)
        {
            switch (ch)
            {
                case 'F':
                    Vector3 startPos = turtle.CrtPosition;
                    turtle.Translate();
                    Vector3 endPos = turtle.CrtPosition;
                    Draw(startPos, endPos);
                    break;
                case '+':
                    turtle.Rotate(angle);
                    break;
                case '-':
                    turtle.Rotate(-angle);
                    break;
                case '[':
                    turtle.Push();
                    break;
                case ']':
                    turtle.Pull();
                    break;
                default:
                    break;
            }
        }

        turtle.MultiplyTranslation(0.75f);
    }

    void Draw(Vector3 startPos, Vector3 endPos)
    {
        Vector3 position = (startPos + endPos) * 0.5f;
        GameObject branchGO = Instantiate(branchPrefab, position, Quaternion.Euler(0, 0, turtle.CrtRotation - 90f));
        branchGO.transform.localScale = Vector3.one * turtle.CrtTranslation * 0.5f;
    }

    string GetSubInRules(char ch)
    {
        foreach (Rule rule in rules)
        {
            if (ch == rule.C)
            {
                return rule.Subs[0];
            }
        }

        return ch.ToString();
    }
}
