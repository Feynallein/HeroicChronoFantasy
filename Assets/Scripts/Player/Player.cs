using Redcode.Extensions;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour {
    [SerializeField] private List<Transform> _Targets = new();
    [SerializeField] private float _MinWait;
    [SerializeField] private float _MaxWait;

    private NavMeshAgent _Agent;
    private bool _DestinationReached = false;
    private int _CurrentTargetIndex;
    private float _ElapsedTime;
    private float _WaitingTime;

    void Start() {
        _Agent = GetComponent<NavMeshAgent>();
        _Agent.updateRotation = false;
        _Agent.updateUpAxis = false;
    }

    void Update() {
        if(Mouse.current.leftButton.wasPressedThisFrame) { //todo: check if mouse isn't out of the house
            Vector3 mousePosition = Mouse.current.position.ReadValue();
            Vector3 target = Camera.main.ScreenToWorldPoint(mousePosition.WithZ(10));
            ChangeTarget(target);
        }

        if (!_Agent.pathPending && !_DestinationReached) {
            if (_Agent.remainingDistance <= _Agent.stoppingDistance) {
                if (!_Agent.hasPath || _Agent.velocity.sqrMagnitude == 0f) {
                    _DestinationReached = true;
                    _WaitingTime = Random.Range(_MinWait, _MaxWait);
                }
            }
        }

        if(_ElapsedTime >= _WaitingTime && _DestinationReached) {
            _CurrentTargetIndex = Random.Range(0, _Targets.Count);
            ChangeTarget(_Targets[_CurrentTargetIndex].position);
            _ElapsedTime = 0;
        }

        if(_DestinationReached) _ElapsedTime += Time.deltaTime;

    }

    private void ChangeTarget(Vector3 target) {
        _Agent.SetDestination(target);
        _DestinationReached = false;
        _ElapsedTime = 0;
    }
}
