using EventsManager;
using Redcode.Extensions;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using SDD.Events;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour {
    [SerializeField] private float _MinDistance;
    [SerializeField] private float _MinWait;
    [SerializeField] private float _MaxWait;
    [SerializeField] private Animator _Animator;

    private NavMeshAgent _Agent;
    private bool _DestinationReached = false;
    private float _ElapsedTime;
    private float _WaitingTime;
    private Vector3 _TopLeftBound;
    private Vector3 _BottomRightBound;

    void Start() {
        _Agent = GetComponent<NavMeshAgent>();
        _Agent.updateRotation = false;
        _Agent.updateUpAxis = false;
    }

    public void SetBounds(Vector3 topLeft, Vector3 bottomRight) {
        _TopLeftBound = topLeft;
        _BottomRightBound = bottomRight;
    }

    void Update() {
        transform.position = transform.position.WithZ(0);

        Vector2 inputDirection = _Agent.velocity;
        if (inputDirection != Vector2.zero) {
            float angle = Mathf.Atan2(inputDirection.y, inputDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        _Animator.SetBool("IsMoving", inputDirection != Vector2.zero);

        if (!_Agent.pathPending && !_DestinationReached) {
            if (_Agent.remainingDistance <= _Agent.stoppingDistance) {
                if (!_Agent.hasPath || _Agent.velocity.sqrMagnitude == 0f) {
                    _DestinationReached = true;
                    _WaitingTime = Random.Range(_MinWait, _MaxWait);
                }
            }
        }

        if(_ElapsedTime >= _WaitingTime && _DestinationReached) {
            Vector3 dest = new();
            do {
                float x = Random.Range(_TopLeftBound.x, _BottomRightBound.x);
                float y = Random.Range(_TopLeftBound.y, _BottomRightBound.y);
                dest = new Vector3(x, y, 0);
            } while (Vector3.Distance(dest, transform.position) < _MinDistance);
            ChangeTarget(dest);
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
