using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rule
{
    public char C { get; set; }
    public string[] Subs { get; set; }

    public Rule(char _c, string[] _subs)
    {
        C = _c;
        Subs = _subs;
    }
}
