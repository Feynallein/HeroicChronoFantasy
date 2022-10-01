namespace Curves {
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// This class references parametrics equations to use as value for the CurveCalculation.MathFunction delegate
    /// </summary>
    public class ParametricEquations {
        /** Base versions **/
        static Vector3 Sin(float t, float a) {
            return new Vector3(t, Mathf.Sin(t) * a, 0);
        }

        static Vector3 Lemniscate(float t, int a) {
            return new Vector3((a * Mathf.Cos(t)) / (1 + Mathf.Pow(Mathf.Sin(t), 2)), (a * Mathf.Sin(t) * Mathf.Cos(t)) / (1 + Mathf.Pow(Mathf.Sin(t), 2)), 0);
        }
        

        /** Specific versions **/
        public static Vector3 Sin(float t) {
            return Sin(t, 1);
        }

        public static Vector3 Sin3(float t) {
            return Sin(t, 3);
        }

        public static Vector3 Lemniscate5(float t) {
            return Lemniscate(t, 5);
        }

        public static Vector3 Lemniscate(float t) {
            return Lemniscate(t, 1);
        }
    }
}








// old backup if needed


/*        /// <summary>
        /// Constructor
        /// <c> Curve </c>
        /// Instantiate the class using points
        /// </summary>
        /// <param name="points">The list of points</param>
        /// <param name="isClosed">True if the curve is closed</param>
        public Curve(List<Vector3> points, bool isClosed = false) {
            _Points = points;
            _Closed = isClosed;

            Vector3 currentPoint;
            Vector3 previousPoint = points[0];

            for(int i = 0; i < points.Count - 1; i++) {
                currentPoint = points[i];
                _Length += Vector3.Distance(previousPoint, currentPoint);
                _CumulatedLength.Add(_Length);

                for (int j = _Indexes.Count; j <= _Length; j++) {
                    _Indexes.Add(Mathf.Max(i, 0));
                }

                previousPoint = currentPoint;
            }

            //todo: HAS TO BE TESTED
            if (isClosed) { 
                Vector3 firstPoint = _Points[0];
                _Length += Vector3.Distance(previousPoint, firstPoint);

                for (int i = _Indexes.Count; i <= _Length; i++) {
                    _Indexes.Add(Mathf.Max(_Points.Count, 0)); // maybe _Points.Count - 1
                }
            }

            _IsValid = true;
        }
*/
