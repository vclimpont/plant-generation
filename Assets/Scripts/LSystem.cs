using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LSystem : MonoBehaviour
{
    public GameObject branchPrefab;
    public GameObject leafPrefab;
    public GameObject branchLinePrefab;
    public GameObject leafLinePrefab;
    public int n;
    public float theta;
    public float phi;
    public string axiom;

    private List<Rule> rules;
    private string sentence;
    private Turtle turtle;
    private GameObject parentGO;

    // Start is called before the first frame update
    void Start()
    {
        rules = new List<Rule>();

        #region RULES
        //rules.Add(new Rule('X', new string[] { "F[+X][-X][^X][&X]FX", "F[+X][^X]FX", "F[-X][&X]FX" }));
        //rules.Add(new Rule('F', new string[] { "FF" }));

        //rules.Add(new Rule('X', new string[] { "F[+X]F[-X]F[^X]F[&X]+X", "F[+X]F[-X]+X", "F[^X]F[&X]+X" }));
        //rules.Add(new Rule('F', new string[] { "FF" }));

        //rules.Add(new Rule('X', new string[] { "F-[[X]+X][[X]^X]+F[+FX][&FX]-X", "F-[[X]+X]+F[&FX]-X", "F-[[X]^X]+F[+FX]-X" }));
        //rules.Add(new Rule('F', new string[] { "FF" }));

        //rules.Add(new Rule('F', new string[] { "FF-[-F+F+F][^F&F&F]+[+F-F-F][&F^F^F]", "FF-[-F+F+F]+[&F^F^F]", "FF-[^F&F&F]+[+F-F-F]" }));

        //rules.Add(new Rule('X', new string[] { "F[+X][^X]F[-X][&X][X]", "F[+X][^X][X]", "F[-X][&X][X]" }));
        //rules.Add(new Rule('F', new string[] { "FF" }));

        //rules.Add(new Rule('X', new string[] { "F[+X][^X]F[-X][&X]FX", "F[+X][^X]FX", "F[-X][&X]FX" }));
        //rules.Add(new Rule('F', new string[] { "FF" }));
        #endregion RULES

        InvokeRepeating("DrawTree", 0f, 3f);
    }

    void DrawTree()
    {
        turtle = new Turtle(new Vector3(0, 0, -5), 1f, 90f, 90f);

        if(parentGO != null)
        {
            Destroy(parentGO);
        }
        parentGO = new GameObject("parentTree");
        parentGO.transform.position = new Vector3(0, 0, -5);

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
                    DrawBranchLine(startPos, endPos);
                   // DrawBranch(startPos, endPos);
                    break;
                case '+':
                    turtle.RotateTheta(theta);
                    break;
                case '-':
                    turtle.RotateTheta(-theta);
                    break;
                case '&':
                    turtle.RotatePhi(phi);
                    break;
                case '^':
                    turtle.RotatePhi(-phi);
                    break;
                case '[':
                    turtle.Push();
                    break;
                case ']':
                    DrawLeafLine(turtle.CrtPosition, turtle.CrtPosition + turtle.GetRotatedTranslation());
                    //DrawLeaf(turtle.CrtPosition);
                    turtle.Pull();
                    break;
                default:
                    break;
            }
        }

        turtle.MultiplyTranslation(0.90f);
    }

    void DrawBranch(Vector3 startPos, Vector3 endPos)
    {
        Vector3 position = (startPos + endPos) * 0.5f;
        GameObject branchGO = Instantiate(branchPrefab, position, Quaternion.Euler(-1 * (turtle.CrtPhi -90f), 0, turtle.CrtTheta - 90f));
        branchGO.transform.localScale = Vector3.one * turtle.CrtTranslation * 0.5f;
        branchGO.transform.parent = parentGO.transform;
    }

    void DrawLeaf(Vector3 position)
    {
        GameObject leafGO = Instantiate(leafPrefab, position, Quaternion.identity);
        leafGO.transform.localScale = Vector3.one * Random.Range(0.75f, 1.5f);
        leafGO.transform.parent = parentGO.transform;
    }

    void DrawBranchLine(Vector3 startPos, Vector3 endPos)
    {
        GameObject branchLine = Instantiate(branchLinePrefab, parentGO.transform);
        LineRenderer line = branchLine.GetComponent<LineRenderer>();
        line.SetPosition(0, startPos);
        line.SetPosition(1, endPos);
    }

    void DrawLeafLine(Vector3 startPos, Vector3 endPos)
    {
        GameObject leafLine = Instantiate(leafLinePrefab, parentGO.transform);
        LineRenderer line = leafLine.GetComponent<LineRenderer>();
        line.SetPosition(0, startPos);
        line.SetPosition(1, endPos);
    }

    string GetSubInRules(char ch)
    {
        foreach (Rule rule in rules)
        {
            if (ch == rule.C)
            {
                return rule.GetRandomRule();
            }
        }

        return ch.ToString();
    }
}
