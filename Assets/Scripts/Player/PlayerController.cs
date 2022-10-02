using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class PlayerController : MonoBehaviour {
    [SerializeField] private float _Speed;
    [SerializeField] private Animator _Animator;

    private PlayerInputActions _Actions;
    private Rigidbody2D _Rigidbody;

    private void Awake() {
        _Actions = new PlayerInputActions();
        _Rigidbody = GetComponent<Rigidbody2D>();
    }

    private void OnEnable() {
        _Actions.Player.Enable();
    }

    private void OnDisable() {
        _Actions.Player.Disable();
    }

    private void FixedUpdate() {
        Vector2 inputDirection = _Actions.Player.Move.ReadValue<Vector2>();
        if (inputDirection != Vector2.zero) {
            float angle = Mathf.Atan2(inputDirection.y, inputDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        _Rigidbody.velocity = inputDirection * _Speed;
        _Animator.SetBool("IsMoving", inputDirection != Vector2.zero);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        if(enemy != null) {
            LevelManager.Instance.MiniGameTime();
            Destroy(collision.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other) {
    }
}
