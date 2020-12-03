using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LSystem : MonoBehaviour
{
    public GameObject branchPrefab;
    public Material woodMaterial;
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
        parentGO = new GameObject("parentTree");
        parentGO.transform.position = new Vector3(0, 0, -5);

        rules = new List<Rule>();
        turtle = new Turtle(new Vector3(0, 0, -5), 1f, 90f, 90f);

        //rules.Add(new Rule('X', new string[] { "F[+X][-X][^X][&X]FX" }));
        //rules.Add(new Rule('F', new string[] { "FF" }));

        rules.Add(new Rule('X', new string[] { "F[+X][-X][^X][&X]FX" }));
        rules.Add(new Rule('F', new string[] { "FF" }));

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
                    DrawLine(startPos, endPos);
                    //DrawObject(startPos, endPos);
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
                    turtle.Pull();
                    break;
                default:
                    break;
            }
        }

        turtle.MultiplyTranslation(0.95f);
    }

    void DrawObject(Vector3 startPos, Vector3 endPos)
    {
        Vector3 position = (startPos + endPos) * 0.5f;
        GameObject branchGO = Instantiate(branchPrefab, position, Quaternion.Euler(-1 * (turtle.CrtPhi -90f), 0, turtle.CrtTheta - 90f));
        branchGO.transform.localScale = Vector3.one * turtle.CrtTranslation * 0.5f;
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

    //void CombineMeshes()
    //{
    //    meshFilters = parentGO.GetComponentsInChildren<MeshFilter>();
    //    combine = new CombineInstance[meshFilters.Length];
    //    MeshRenderer parentMesh = parentGO.AddComponent<MeshRenderer>();
    //    parentMesh.material = parentGO.GetComponentInChildren<MeshRenderer>().material;

    //    int i = 0;
    //    while (i < meshFilters.Length)
    //    {
    //        combine[i].mesh = meshFilters[i].sharedMesh;
    //        combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
    //        meshFilters[i].gameObject.SetActive(false);

    //        i++;
    //    }
    //    parentGO.AddComponent<MeshFilter>();
    //    parentGO.transform.GetComponent<MeshFilter>().mesh = new Mesh();
    //    parentGO.transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);
    //    parentGO.SetActive(true);
    //}
}
