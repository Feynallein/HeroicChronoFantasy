using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill {
    public static int MAX_LOW = 1;
    public static int MAX_MED = 4;

    public enum SkillRange {
        low,
        med,
        high
    }

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

    public string GetRangeToString() {
        if (_Value <= MAX_LOW) return "low";
        if (_Value <= MAX_MED) return "med";
        else return "high";
    }
}