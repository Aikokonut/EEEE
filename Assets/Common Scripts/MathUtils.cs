using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using DG.Tweening;
using Sirenix.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace DL.Utils
{
    public static class MathUtils
    {
        public static float GetDistanceToCircle(Vector3 position, Vector3 direction, Vector3 center, float radius)
        {
            var viewDir = direction;
            var dirToCenter = center - position.Set(y: 0);

            var baseAngle = Vector3.Angle(viewDir, dirToCenter);
            var angle = baseAngle > 90 ? Vector3.Angle(-viewDir, dirToCenter) : baseAngle;
            angle = angle * Mathf.Deg2Rad;

            var distToDiameter = dirToCenter.magnitude * Mathf.Sin(angle);
            var chordHalfLength = Mathf.Sqrt(radius.Square() - distToDiameter.Square());
            var posLengthOnChord = Mathf.Sqrt(dirToCenter.magnitude.Square() - distToDiameter.Square());

            var aoeLength = baseAngle > 90 ?
                chordHalfLength * 2 - (chordHalfLength + posLengthOnChord) :
                chordHalfLength + posLengthOnChord;

            return aoeLength;
        }
        public static List<int> SeperateToAddend(int number, int maxVal, System.Random randObj)
        {
            var res = new List<int>();
            while (number > 0)
            {
                var randAddend = randObj.Next(1, maxVal + 1);
                if (randAddend > number) randAddend = number;
                number -= randAddend;
                res.Add(randAddend);
            }
            return res;
        }
        public static bool IsCircleInsideCircle(Vector3 smallPosition, float smallRadius, Vector3 bigPosition, float bigRadius)
        {
            var length = (bigPosition.Set(y: 0) - smallPosition.Set(y: 0)).magnitude;
            return bigRadius >= smallRadius && length <= (bigRadius - smallRadius);
        }
        public static bool IsCircleOutsideCircle(Vector3 smallPosition, float smallRadius, Vector3 bigPosition, float bigRadius)
        {
            var length = (bigPosition.Set(y: 0) - smallPosition.Set(y: 0)).magnitude;
            return length >= (bigRadius + smallRadius);
        }
        public static bool IsCircleTouchCircle(Vector3 smallPosition, float smallRadius, Vector3 bigPosition, float bigRadius)
        {
            var length = (bigPosition.Set(y: 0) - smallPosition.Set(y: 0)).magnitude;
            return length <= (smallRadius + bigRadius);
        }
        public static bool IsCircleOnCircle(Vector3 smallPosition, float smallRadius, Vector3 bigPosition, float bigRadius)
        {
            var length = (bigPosition.Set(y: 0) - smallPosition.Set(y: 0)).magnitude;
            return length <= (smallRadius + bigRadius) && length >= bigRadius - smallRadius;
        }
        public static int GetSum(IEnumerable<int> input)
        {
            int res = 0;
            input.ForEach(x => res += x);
            return res;
        }
        public static Vector3 GetCanvasWorldPosition(Vector3 pos, float zVal = 12.5f)
        {
            var canvasPos = Camera.main.WorldToViewportPoint(pos);
            canvasPos.z = zVal;
            canvasPos = Camera.main.ViewportToWorldPoint(canvasPos);
            return canvasPos;
        }
        public static Quaternion GetRandomRotation()
        {
            var x = Random.Range(0, 180);
            var y = Random.Range(0, 180);
            var z = Random.Range(0, 180);

            return Quaternion.Euler(new Vector3(x, y, z));
        }
        public static bool GetRate(float percent)
        {
            var val = Random.Range(1f, 100f);
            return val <= percent;
        }
        public static float SqrDistance(Vector3 from, Vector3 to)
            => (from - to).sqrMagnitude;
        public static float Square(this float target){
            return target * target;
        }
        public static float GetDistanceToPlane(Vector3 point, Vector3 planeNormal, Vector3 planePoint){
            var dirToPoint = point - planePoint;
            if(Vector3.Dot(dirToPoint, planeNormal) < 0) planeNormal *= -1;
            var distToPlane = dirToPoint.magnitude * Mathf.Cos(Vector3.Angle(dirToPoint, planeNormal) * Mathf.Deg2Rad);
            return distToPlane;
        }
        public static Vector3 GetProjectionOnPlane(Vector3 point, Vector3 planeNormal, Vector3 planePoint){
            var distToPlane = GetDistanceToPlane(point, planeNormal, planePoint);
            return point - planeNormal * distToPlane;
        }
        public static Vector3 GetIntersectionWithPlane(Vector3 point, Vector3 dir, Vector3 planeNormal, Vector3 planePoint){
            dir.Normalize();
            var distToPlane = GetDistanceToPlane(point, planeNormal, planePoint);
            Debug.Log(Vector3.Angle(planeNormal, dir));
            return point + dir * (distToPlane / Mathf.Abs(Mathf.Cos(Vector3.Angle(planeNormal, dir) * Mathf.Deg2Rad)));       
        }

        public static bool IsPointInsidePolygon(Vector2 point, List<Vector2> polygon){
            for (int i = 1; i < polygon.Count; i++)
            {
                var vert = polygon[i];
                var nextVert = polygon[i < polygon.Count ? i + 1 : 0];
                var lastVert = polygon[i - 1];

                var firstSegment = vert - lastVert;
                var secondSegment = vert - nextVert;

                var normalsFirstSegment = new List<Vector2>(){
                    new Vector2(-firstSegment.y, firstSegment.x),
                    new Vector2(firstSegment.y, -firstSegment.x),
                };

                var normalsSecondSegment = new List<Vector2>(){
                    new Vector2(-secondSegment.y, secondSegment.x),
                    new Vector2(secondSegment.y, -secondSegment.x),
                };

                var angle = Vector3.Angle(firstSegment, secondSegment);
                foreach(var m in normalsFirstSegment){
                    foreach(var n in normalsSecondSegment){

                    }
                }
            }

            return false;
        }
    }
}