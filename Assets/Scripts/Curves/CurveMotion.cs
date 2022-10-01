namespace Curves {
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using System.Linq;

    public abstract class CurveMotion : MonoBehaviour {
        [Header("Control points attributes")]
        [Tooltip("The list of control points used to compute Bezier")]
        [SerializeField] protected List<Transform> _ControlPoints;
        [Tooltip("The number of point per meters")]
        [SerializeField] protected float _PointsDensity;

        [Header("Curve attributes")]
        [Tooltip("Min value of definition domain (multiplied by PI)")]
        [SerializeField] protected float _Min;
        [Tooltip("Max value of definition domain (multiplied by PI)")]
        [SerializeField] protected float _Max;
        [Tooltip("Number of points to set on the curve")]
        [SerializeField] protected int _NumberOfPoints;

        [Header("General attributes")]
        [Tooltip("List of objects to move")]
        [SerializeField] protected List<Transform> _BodyParts = new List<Transform>();
        [Tooltip("If the curve is closed")]
        [SerializeField] protected bool _IsClosed = false;
        [Tooltip("Speed of moving objects (in m/s)")]
        [SerializeField] protected float _TranslationSpeed;
        [Tooltip("Direction going (true = in the curve's sens)")]
        [SerializeField] protected bool _SearchDirection = true;
        [Tooltip("Repeat the movement")]
        [SerializeField] protected bool _Repeat = false;

        protected CurveCalculation.MathFunction _Equation;
        protected CurveCalculation _Curve;
        protected float _TranslatedDistance = 0;

        private void Awake() {
            _Equation = AwakeChild();

            if (_ControlPoints.Count != 0) _Curve = new CurveCalculation(_ControlPoints, _PointsDensity, _IsClosed);
            else if (_Equation != null) _Curve = new CurveCalculation(_Equation, _Min, _Max * Mathf.PI, _NumberOfPoints, _IsClosed);
            else throw new System.Exception("No curve given. Either put control points or return a curve model in the AwakeChild method.");
        }

        protected abstract CurveCalculation.MathFunction AwakeChild();

        protected bool BodyPartToIndex(List<Transform> parts, GameObject bodyPart, out int index) {
            index = -1;
            for(int i = 0; i < parts.Count; i++) {
                if (parts[i] == bodyPart.transform) {
                    index = i;
                    return true;
                }
            }
            return false;
        }

        protected void MoveOnCurve(List<Transform> parts, float distance, int direction) {
            if (_Curve == null || !_Curve.IsValid || parts.Count == 0) return;

            Vector3 position;
            float previousRadius = 0;
            Vector3 previousPosition = Vector3.zero;
            int previousIndex = 0;

            for (int i = 0; i < parts.Count; i++) {
                Transform movingObject = parts[i];
                float currentRardius = movingObject.GetComponent<SphereCollider>().radius * movingObject.localScale.x;
                int currentIndex;

                if (i == 0 && _Curve.GetPositionFromDistance(distance, out position, out currentIndex)) movingObject.position = new Vector3(position.x, position.y, transform.position.z);
                else if (_Curve.GetPositionOnCurve(previousPosition, previousRadius + currentRardius, previousIndex, direction, out position, out currentIndex)) movingObject.position = new Vector3(position.x, position.y, transform.position.z);

                previousRadius = currentRardius;
                previousPosition = position;
                previousIndex = currentIndex;
            }
        }
    }
}
