namespace Curves {
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using System.Linq;

    public class CurveCalculation {
        List<Transform> _ControlPoints = new List<Transform>();
        List<Vector3> _Points = new List<Vector3>();
        List<float> _CumulatedLength = new List<float>();

        List<int> _Indexes = new List<int>();

        float _Length = 0;
        bool _Closed;
        bool _IsValid = false;

        public bool IsValid {  get { return _IsValid; } }

        public int NPoints { get { return _Points == null ? -1 : _Points.Count; } }

        public float Length { get { return _Length; } }

        public float LengthIdx(int idx) {
            return _CumulatedLength[idx];
        }

        public delegate Vector3 MathFunction(float t);

        /// <summary>
        /// Constructor
        /// <c> Curve </c>
        /// Instantiate the class using control points
        /// </summary>
        /// <param name="controlPoints">The list of control points</param>
        /// <param name="ptsDensity">The density (points per meters) of points to compute</param>
        /// <param name="isClosed">True if the curve is closed</param>
        public CurveCalculation(List<Transform> controlPoints, float ptsDensity, bool isClosed = false) { //todo: fix when is closed = true
            _ControlPoints = controlPoints;
            _Closed = isClosed;

            List<Vector3> positions = _ControlPoints.Select(item => item.position).ToList();
            
            if (isClosed) { 
                Vector3 ctrlPt0 = positions[0];
                Vector3 ctrlPt1 = positions[1];
                Vector3 ctrlPtNMinus1 = positions[positions.Count - 1];
                positions.Add(ctrlPt0);
                positions.Add(ctrlPt1);
                positions.Insert(0, ctrlPtNMinus1);
            }

            Vector3 previousPoint = Vector3.zero;

            for (int i = 1; i < positions.Count - 2; i++) {
                Vector3 P0 = positions[i - 1];
                Vector3 P1 = positions[i];
                Vector3 P2 = positions[i + 1];
                Vector3 P3 = positions[i + 2];
                float distance = Vector3.Distance(P1, P2);
                int nPts = (int) Mathf.Max(3, distance * ptsDensity);
                if(previousPoint == Vector3.zero) previousPoint = ComputeBezierPos(P0, P1, P2, P3, 0);
                int flooredLength;

                for (int j = 0; j < nPts; j++) {
                    int nPtsDenominator = (i == positions.Count - 3) && !_Closed ? nPts - 1 : nPts;
                    float k = (float) j / nPtsDenominator;
                    Vector3 currentPoint = ComputeBezierPos(P0, P1, P2, P3, k);
                    _Length += Vector3.Distance(currentPoint, previousPoint);
                    flooredLength = Mathf.FloorToInt(_Length);
                    _CumulatedLength.Add(_Length);

                    for (int n = _Indexes.Count; n <= flooredLength; n++) {
                        _Indexes.Add(Mathf.Max(_Points.Count - 1, 0));
                    }

                    previousPoint = currentPoint;
                    _Points.Add(currentPoint);
                }
            }

            _IsValid = true;
        }

        /// <summary>
        /// Constructor
        /// <c> Curve </c>
        /// Instantiate the class using a function (parametric equation)
        /// </summary>
        /// <param name="function">The function which will be the curve</param>
        /// <param name="tMin">Min value</param>
        /// <param name="tMax">Max value</param>
        /// <param name="nbPoints">Total number of points</param>
        /// <param name="isClosed">True if the function forms a closed curve</param>
        public CurveCalculation(MathFunction function, float tMin, float tMax, int nbPoints, bool isClosed = false) {
            Vector3 currentPoint;
            Vector3 previousPoint = function(tMin);
            _Closed = isClosed;
            int flooredTotalLength;
            float step = (tMax - tMin) / (nbPoints - 1);

            if (nbPoints < 2 || tMin == tMax) return;

            for (int j = 0; j <= nbPoints - 1; j++) {
                float t = tMin + step * j;
                currentPoint = function(t);
                _Length += Vector3.Distance(previousPoint, currentPoint);
                flooredTotalLength = Mathf.FloorToInt(_Length);
                _CumulatedLength.Add(_Length);

                for (int i = _Indexes.Count; i <= flooredTotalLength; i++) {
                    _Indexes.Add(Mathf.Max(_Points.Count - 1, 0));
                }

                _Points.Add(currentPoint);
                previousPoint = currentPoint;
            }

            if (_Closed) {
                Vector3 firstPoint = _Points[0];
                _Length += Vector3.Distance(previousPoint, firstPoint);
                _Points.Add(firstPoint);
                flooredTotalLength = Mathf.FloorToInt(_Length);

                for (int i = _Indexes.Count; i <= flooredTotalLength; i++) {
                    _Indexes.Add(Mathf.Max(_Points.Count - 1, 0));
                }
            }

            _IsValid = true;
        }

        Vector3 ComputeBezierPos(Vector3 a, Vector3 b, Vector3 c, Vector3 d, float t) {
            return (.5f * ((-a + 3f * b - 3f * c + d) * (t * t * t) + (2f * a - 5f * b + 4f * c - d) * (t * t) + (-a + c) * t + 2f * b));
        }

        /// <summary>
        /// Method 
        /// <c> GetPositionFromDistance </c>
        /// This method calculate the index the point right before the position. 
        /// </summary>
        /// <param name="dist">The current distance where we whant to know the point in the array</param>
        /// <param name="pos">Out parameter: will store the value of the position using linear interpolation</param>
        /// <returns>Return true if the curve is valid</returns>
        public bool GetPositionFromDistance(float dist, out Vector3 pos, out int segmentIndex) {
            pos = Vector3.zero;
            segmentIndex = 0;
            if (!_IsValid) return false;

            int flooredDistance;
            float distance = dist;
            int idx;

            if (_Closed) {
                while (distance < 0) distance += _Length;
                distance %= _Length;
            }
            else distance = Mathf.Clamp(distance, 0, _Length);

            flooredDistance = Mathf.FloorToInt(distance);
            idx = _Indexes[Mathf.Clamp(flooredDistance, 0, _Indexes.Count - 1)];

            while (_CumulatedLength[idx] < distance) idx++;
            if (idx > 0) idx--;

            Vector3 previousPoint = _Points[idx];
            Vector3 nextPoint = _Points[idx + 1];

            float previousPointLength = _CumulatedLength[idx];
            float nextPointLength = _CumulatedLength[idx + 1];

            segmentIndex = idx;
            pos = previousPoint + (nextPoint - previousPoint) * ((distance - previousPointLength) / (nextPointLength - previousPointLength));
           
            return true;
        }

        public bool GetPositionOnCurve(Vector3 center, float radius, int startIndex, int direction, out Vector3 pos, out int segmentIndex) {
            pos = Vector3.zero;
            return GetSphereSplineIntersection(center, radius, startIndex, direction, out float distance, out segmentIndex) && GetPositionFromDistance(distance, out pos, out segmentIndex);
        }

        public bool GetSphereSplineIntersection(Vector3 center, float radius, int startIndex, int direction, out float distance, out int segmentIndex) {
            segmentIndex = startIndex;
            distance = 0;

            if (!_IsValid) return false;

            float delta;
            float t;
            int idx = startIndex;
            int failsafe = 0;
            Vector3 ABLength;

            do {
                idx += direction;
                if (idx < 0 && direction == -1) idx = _Points.Count - 2;
                if (idx > _Points.Count - 2 && direction == 1) idx = 0;

                Vector3 A = _Points[idx];
                Vector3 B = _Points[idx + 1];
                Vector3 AB = B - A;
                Vector3 CenterA = A - center;

                float a = Vector3.Dot(AB, AB);
                float b = 2 * Vector3.Dot(CenterA, AB);
                float c = Vector3.Dot(CenterA, CenterA) - Mathf.Pow(radius, 2);

                ABLength = AB;

                delta = Mathf.Pow(b, 2) - 4 * a * c;

                if (delta >= 0) t = (-b + Mathf.Sqrt(delta) * direction) / (2 * a);
                else t = -45;

                failsafe++;
            } while ((t == -45 || !(0 < t && t < 1)) && failsafe < _Points.Count);

            if (failsafe >= _Points.Count) return false;

            segmentIndex = idx;

            distance = _CumulatedLength[idx] + t * ABLength.magnitude;

            return true;
        }
    }
}
