using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill {
    private int _Value;
    private string _Name;

    public Skill(string name) {
        _Value = 0;
        _Name = name;
    }

    public int Value { get { return _Value; } }

    public string Name { get { return _Name; } }

    public void IncrementValue(int incr) {
        _Value += incr;
    }
}