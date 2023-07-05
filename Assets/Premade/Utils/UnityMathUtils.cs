using Unity.Mathematics;
using UnityEngine;

    public static class UnityMathUtils
    {
        private const int Zero = 0;
        private const int HalfCircleDegree = 180;
        private static readonly Vector3 UpVector = Vector3.up; 
        
        public static float GetRollAngleOfLineTowardsTarget(Vector3 lineStartPoint, Vector3 lineEndPoint, Vector3 target)
        {
            var lineStartPointToTargetVector2D = (target - lineStartPoint).ToVector2XZ();
            var targetToLineStartPointDistance2D = lineStartPointToTargetVector2D.magnitude;
            var lineVector = lineEndPoint - lineStartPoint;
            var lineVectorWithLengthOf2dDistance = lineVector.ChangeLength(targetToLineStartPointDistance2D);
            var lineEndPointToTargetVector = target - lineEndPoint;
            var isLinePointedAwayFromTarget = Vector2.Dot(lineStartPointToTargetVector2D, lineVector.ToVector2XZ()) < Zero;
            var lineEndPointToTargetVectorRotated180 = lineEndPointToTargetVector.RotateByAngleAroundAxis(HalfCircleDegree, UpVector);
            var lineEndPointToTargetVectorPointingTowardsTarget = isLinePointedAwayFromTarget ? lineEndPointToTargetVectorRotated180 : lineEndPointToTargetVector;
            var vectorRotatedByTheDesiredAngle = lineEndPointToTargetVectorPointingTowardsTarget - lineVectorWithLengthOf2dDistance;
            
            // the axis is lineEndPointToTargetVector because this way if the vectorRotatedByTheDesiredAngle is from different side of it - the angle sign will change
            var desiredAngle = Vector3.SignedAngle(Vector3.up, vectorRotatedByTheDesiredAngle, lineEndPointToTargetVector);
            return desiredAngle;
        }
        
        public static float Remap(
            float minValueOnRangeA,
            float maxValueOnRangeA,
            float minValueOnRangeB,
            float maxValueOnRangeB,
            float valueOnRangeA)
        {
            return math.remap(minValueOnRangeA, maxValueOnRangeA, minValueOnRangeB, maxValueOnRangeB, valueOnRangeA);
        }
    }

