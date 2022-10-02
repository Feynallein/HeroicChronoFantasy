using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Singleton<PlayerController> {
    [SerializeField] private float _Speed;
    [SerializeField] private Animator _Animator;
    [SerializeField] private StudioEventEmitter _Emitter;

    private PlayerInputActions _Actions;
    private Rigidbody2D _Rigidbody;

    protected override void Awake() {
        base.Awake();

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
        //_Emitter.SetParameter("IsMoving", inputDirection != Vector2.zero ? 1 : 0);
        if (inputDirection != Vector2.zero) {
            if (!_Emitter.IsPlaying()) _Emitter.Play();
            float angle = Mathf.Atan2(inputDirection.y, inputDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        } else _Emitter.Stop();
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
}
