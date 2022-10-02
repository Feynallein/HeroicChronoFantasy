using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Redcode.Extensions;

public class Sequence : MiniGame {
    [SerializeField] private GameObject _ArrowPrefab;
    [SerializeField] private int _SequenceLength;
    [SerializeField] private int _MaxArrowInLine;
    [SerializeField] private float _YOffset;
    [SerializeField] private float _XOffset;
    [SerializeField, ColorUsage(true, true)] private Color _GoodColor;

    private List<string> _StringSeq = new();
    private List<GameObject> _ArrowSeq = new();
    private int _Cursor;

    protected override void OnEnable() {
        base.OnEnable();
        _Cursor = 0;
        _StringSeq.Clear();
        _ArrowSeq.Clear();
        GenerateSequence();
        PrintSequence();
    }

    protected override void Update() {
        base.Update();
        if (Keyboard.current.anyKey.wasPressedThisFrame) {
            if ((Keyboard.current.leftArrowKey.wasPressedThisFrame || Keyboard.current.aKey.wasPressedThisFrame) && _StringSeq[_Cursor] == "left") GoodInput();
            else if ((Keyboard.current.rightArrowKey.wasPressedThisFrame || Keyboard.current.dKey.wasPressedThisFrame) && _StringSeq[_Cursor] == "right") GoodInput();
            else if ((Keyboard.current.upArrowKey.wasPressedThisFrame || Keyboard.current.wKey.wasPressedThisFrame) && _StringSeq[_Cursor] == "up") GoodInput();
            else if ((Keyboard.current.downArrowKey.wasPressedThisFrame || Keyboard.current.sKey.wasPressedThisFrame) && _StringSeq[_Cursor] == "down") GoodInput();
            else BadInput();
        }
    }

    private void BadInput() {
        LevelManager.Instance.MiniGameCallback(false);
    }
    
    private void GoodInput() {
        _ArrowSeq[_Cursor].GetComponent<Image>().color = _GoodColor;
        _Cursor++;
        if (_Cursor == _SequenceLength) LevelManager.Instance.MiniGameCallback(true);
    }

    private void GenerateSequence() {
        for(int i = 0; i < _SequenceLength; i++) {
            _StringSeq.Add(IntToSeq(Random.Range(0, 4)));
        }
    }

    private string IntToSeq(int val) {
        return val switch {
            0 => "left",
            1 => "up",
            2 => "right",
            3 => "down",
            _ => "",
        };
    }

    private float SeqStrToAngle(string str) {
        return str switch {
            "left" => -90,
            "up"  => 180,
            "right"  => 90,
            "down"  => 0,
            _ => 0,
        };
    }

    private void PrintSequence() {
        int y = -1;
        for(int i = 0; i < _SequenceLength; i++) {
            int x = i % _MaxArrowInLine;
            if (x == 0) y++;
            GameObject go = Instantiate(_ArrowPrefab, transform.position.WithXY(transform.position.x + x * _XOffset, transform.position.y - y * _YOffset), transform.rotation, transform);
            go.transform.Rotate(go.transform.forward, SeqStrToAngle(_StringSeq[i]));
            _ArrowSeq.Add(go);
        }
    }
}
