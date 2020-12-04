using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rule
{
    public char C { get; set; }
    public string[] Subs { get; set; }

    private readonly float p;

    public Rule(char _c, string[] _subs)
    {
        C = _c;
        Subs = _subs;
        p = 1f / Subs.Length;
    }

    public string GetRandomRule()
    {
        float r = Random.Range(0f, 1f);
        float pp = p;
        int k = 0;

        while(k < Subs.Length)
        {
            if(r <= pp)
            {
                return Subs[k];
            }
            k++;
            pp = p * (k + 1);
        }

        throw new System.Exception("Invalid probability on r = " + r + " , pp = " + pp);
    }
}
