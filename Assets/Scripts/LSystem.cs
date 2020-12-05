using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LSystem : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject branchPrefab;
    public GameObject leafPrefab;
    public GameObject branchLinePrefab;
    public GameObject leafLinePrefab;
    public Material branchMaterial;
    public Material leafMaterial;

    [Header("LSystem Parameters")]
    public Vector3[] treePositions;
    [Range(0,1)]
    public float scaleFactor;
    public int iterations;
    public float theta;
    public float phi;
    public string axiom;

    [Header("Rules")]
    public string[] rulesArray;

    private List<Rule> rules;
    private string sentence;
    private Turtle turtle;

    private GameObject parentGO;
    private GameObject[] branchesParentGO;
    private GameObject[] leavesParentGO;
    private LinkedList<GameObject> branchesGO;
    private LinkedList<GameObject> leavesGO;

    // Start is called before the first frame update
    void Start()
    {
        rules = new List<Rule>();

        #region RULES
        rules.Add(new Rule('X', rulesArray));
        rules.Add(new Rule('F', new string[] { "FF" }));

        //rules.Add(new Rule('F', new string[] { "FF-[-F+F+F][^F&F&F]+[+F-F-F][&F^F^F]", "FF-[-F+F+F]+[&F^F^F]", "FF-[^F&F&F]+[+F-F-F]" }));

        //rules.Add(new Rule('X', new string[] { "F[+X][^X]F[-X][&X][X]", "F[+X][^X][X]", "F[-X][&X][X]" }));
        //rules.Add(new Rule('F', new string[] { "FF" }));

        //rules.Add(new Rule('X', new string[] { "F[+X][^X]F[-X][&X]FX", "F[+X][^X]FX", "F[-X][&X]FX" }));
        //rules.Add(new Rule('F', new string[] { "FF" }));
        #endregion RULES

        for (int i = 0; i < treePositions.Length; i++)
        {
            DrawTree(treePositions[i]);
        }

        //InvokeRepeating("DrawTree", 0f, 3f);
    }

    void DrawTree(Vector3 position, bool erase = false)
    {
        turtle = new Turtle(position, 1f, 90f, 90f, 5f);

        if (parentGO != null && erase)
        {
            Destroy(parentGO);
        }

        parentGO = new GameObject("parentTree");
        parentGO.transform.position = Vector3.zero;
        branchesGO = new LinkedList<GameObject>();
        leavesGO = new LinkedList<GameObject>();

        sentence = axiom;

        for (int i = 0; i < iterations; i++)
        {
            Generate();
        }

        MoveTurtle();

        branchesParentGO = InitObjectsParents(branchMaterial);
        leavesParentGO = InitObjectsParents(leafMaterial);
        MoveObjectsToParents(branchesParentGO, branchesGO);
        MoveObjectsToParents(leavesParentGO, leavesGO);

        foreach (GameObject branchesParent in branchesParentGO)
        {
            CombineMeshes(branchesParent);
        }

        foreach (GameObject leavesParent in leavesParentGO)
        {
            CombineMeshes(leavesParent);
        }
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
                    //DrawBranchLine(startPos, endPos);
                    DrawBranch(startPos, endPos);
                    turtle.MultiplyWidth(scaleFactor);
                    break;
                case '+':
                    turtle.RotateTheta(theta);
                    turtle.MultiplyWidth(scaleFactor * 0.75f);
                    break;
                case '-':
                    turtle.RotateTheta(-theta);
                    turtle.MultiplyWidth(scaleFactor * 0.75f);
                    break;
                case '&':
                    turtle.RotatePhi(phi);
                    turtle.MultiplyWidth(scaleFactor * 0.75f);
                    break;
                case '^':
                    turtle.RotatePhi(-phi);
                    turtle.MultiplyWidth(scaleFactor * 0.75f);
                    break;
                case '[':
                    turtle.Push();
                    break;
                case ']':
                    //DrawLeafLine(turtle.CrtPosition, turtle.CrtPosition + turtle.GetRotatedTranslation());
                    DrawLeaf(turtle.CrtPosition);
                    turtle.Pull();
                    break;
                default:
                    break;
            }
        }
    }

    void DrawBranch(Vector3 startPos, Vector3 endPos)
    {
        Vector3 position = (startPos + endPos) * 0.5f;
        GameObject branchGO = Instantiate(branchPrefab, position, Quaternion.Euler(-1 * (turtle.CrtPhi -90f), 0, turtle.CrtTheta - 90f));
        branchGO.transform.localScale = Vector3.one * turtle.CrtTranslation * 0.5f;
        branchGO.transform.parent = parentGO.transform;
        branchesGO.AddLast(branchGO);
    }

    void DrawLeaf(Vector3 position)
    {
        GameObject leafGO = Instantiate(leafPrefab, position, Quaternion.identity);
        leafGO.transform.localScale = Vector3.one * Random.Range(0.75f, 1.5f);
        leafGO.transform.parent = parentGO.transform;
        leavesGO.AddLast(leafGO);
    }

    void DrawBranchLine(Vector3 startPos, Vector3 endPos)
    {
        GameObject branchLine = Instantiate(branchLinePrefab, parentGO.transform);
        LineRenderer line = branchLine.GetComponent<LineRenderer>();
        line.SetPosition(0, startPos);
        line.SetPosition(1, endPos);
        line.startWidth = Mathf.Max(turtle.CrtWidth, 0.5f);
        line.endWidth = Mathf.Max(turtle.CrtWidth, 0.5f);
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

    GameObject[] InitObjectsParents(Material mat)
    {
        GameObject[] objParentGO = new GameObject[10];
        for (int i = 0; i < objParentGO.Length; i++)
        {
            objParentGO[i] = new GameObject("objectParent" + i);
            objParentGO[i].AddComponent<MeshFilter>();
            MeshRenderer renderer = objParentGO[i].AddComponent<MeshRenderer>();
            renderer.material = mat;
        }

        return objParentGO;
    }

    void MoveObjectsToParents(GameObject[] objParentGO, LinkedList<GameObject> objList)
    {
        int offset = (int)Mathf.Ceil((objList.Count * 1.0f) / objParentGO.Length);

        for (int i = 0; i < objParentGO.Length; i++)
        {
            objParentGO[i].transform.parent = parentGO.transform;

            for (int j = 0; j < offset; j++)
            {
                if(objList.Count <= 0)
                {
                    break;
                }
                objList.First.Value.transform.parent = objParentGO[i].transform;
                objList.RemoveFirst();
            }
        }
    }
    
    void CombineMeshes(GameObject obj)
    {
        Vector3 position = obj.transform.position;
        obj.transform.position = Vector3.zero;

        MeshFilter[] meshFilters = obj.GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length - 1];

        int i = 0;
        while (i < meshFilters.Length - 1)
        {
            combine[i].mesh = meshFilters[i + 1].sharedMesh;
            combine[i].transform = meshFilters[i + 1].transform.localToWorldMatrix;
            meshFilters[i + 1].gameObject.SetActive(false);

            i++;
        }

        for (int j = 0; j < combine.Length; j++)
        {
            if(combine[j].mesh == null)
            {
                Debug.Log(j);
            }
        }

        obj.transform.GetComponent<MeshFilter>().mesh = new Mesh();
        obj.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);
        obj.gameObject.SetActive(true);

        obj.transform.position = position;
    }
}
