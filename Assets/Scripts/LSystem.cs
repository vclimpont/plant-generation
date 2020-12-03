using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LSystem : MonoBehaviour
{
    public GameObject branchPrefab;
    public Material woodMaterial;
    public int n;
    public float angle;
    public string axiom;

    private List<Rule> rules;
    private string sentence;
    private Turtle turtle;
    private GameObject parentGO;

    // Start is called before the first frame update
    void Start()
    {
        parentGO = new GameObject("parentTree");
        parentGO.transform.position = new Vector3(0, 0, -5);

        rules = new List<Rule>();
        turtle = new Turtle(new Vector3(0, 0, -5), new Vector3(0, 1, 0));

        rules.Add(new Rule('A', new string[] { "B-F+CFC+F-D&F∧D-F+&&CFC+F+B//" }));
        rules.Add(new Rule('B', new string[] { "A&F∧CFB∧F∧D∧∧-F-D∧|F∧B|FC∧F∧A//" }));
        rules.Add(new Rule('C', new string[] { "|D∧|F∧B-F+C∧F∧A&&FA&F∧C+F+B∧F∧D//" }));
        rules.Add(new Rule('D', new string[] { "|CFB-F+B|FA&F∧A&&FB-F+B|FC//" }));
        //rules.Add(new Rule('X', new string[] { "F-[[X]+X]+F[+FX]-X" }));
        //rules.Add(new Rule('F', new string[] { "FF" }));

        sentence = axiom;

        for (int i = 0; i < n; i++)
        {
            Generate();
            Debug.Log(sentence);
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
                    DrawLine(startPos, endPos);
                    //DrawObject(startPos, endPos);
                    break;
                case '+':
                    turtle.Rotate(2, angle);
                    break;
                case '-':
                    turtle.Rotate(2, -angle);
                    break;
                case '&':
                    turtle.Rotate(0, angle);
                    break;
                case '∧':
                    turtle.Rotate(0, -angle);
                    break;
                case '_':
                    turtle.Rotate(1, angle);
                    break;
                case '/':
                    turtle.Rotate(1, -angle);
                    break;
                case '|':
                    turtle.Rotate(2, 180f);
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

        turtle.MultiplyTranslation(1f);
    }

    void DrawObject(Vector3 startPos, Vector3 endPos)
    {
        Vector3 position = (startPos + endPos) * 0.5f;
        GameObject branchGO = Instantiate(branchPrefab, position, Quaternion.Euler(turtle.CrtTranslation * -90f));
        branchGO.transform.localScale = Vector3.one * 0.5f;
        branchGO.transform.parent = parentGO.transform;
    }

    void DrawLine(Vector3 startPos, Vector3 endPos)
    {
        GameObject branchGO = new GameObject("branch");
        LineRenderer line = branchGO.AddComponent<LineRenderer>();
        line.positionCount = 2;
        line.SetPosition(0, startPos);
        line.SetPosition(1, endPos);
        line.material = woodMaterial;
        line.generateLightingData = true;
        line.startWidth = 0.5f;
        line.endWidth = 0.5f;
        branchGO.transform.parent = parentGO.transform;
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
