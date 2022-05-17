using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Base.Extensions.Utilites
{
    public static class Vec3Uts
    {
        /// <summary>
        /// Retorna um vetor de magnitude 1, com a orientação em XZ do objeto
        /// </summary>
        /// <param name="_objectTransform"></param>
        /// <returns></returns>
        public static Vector3 GetObjectXZOrientation(Transform _objectTransform) { return Vec2Uts.Vec2ToVec3XZ(Vec2Uts.RotateVec2(Vector2.right, 360 - (FloatUts.FloatModulus(_objectTransform.rotation.eulerAngles.y + 270, 360)))).normalized; }

        public static Vector3 SetYGetVec3(Vector3 _vec3, float _y) { return new Vector3(_vec3.x, _y, _vec3.z); }
        public static Vector3 GetMPoint(Vector3 _a, Vector3 _b) { return new Vector3((_a.x + _b.x) / 2f, (_a.y + _b.y) / 2f, (_a.z + _b.z) / 2f); }
        public static Vector2 Vec3XZToVec2(Vector3 _vec3) { return new Vector2(_vec3.x, _vec3.z); }

        public static Vector3 LerpAndMoveTo(Vector3 _vecToMove, Vector3 _target, float _linearSpeed, float _lerpSpeed, float _commonMultiplier = 1f)
        {
            Vector3 finalPos;

            finalPos = Vector3.Lerp(_vecToMove, _target, _commonMultiplier * _lerpSpeed);
            finalPos = Vector3.MoveTowards(finalPos, _target, _commonMultiplier * _linearSpeed);

            return finalPos;
        }
    }
    public static class Vec2Uts
    {
        public static Vector3 Vec2ToVec3XZ(Vector2 _vec2) { return new Vector3(_vec2.x, 0, _vec2.y); }
        public static Vector3 Vec2ToVec3XZ(Vector2 _vec2, float _y) { return new Vector3(_vec2.x, _y, _vec2.y); }
        public static Vector2 RotateVec2(Vector2 _vec2, float _angle)
        {
            float vecMagnitude = _vec2.magnitude;
            float normalizedAngle = (Mathf.Abs(_angle) > 360 ? _angle - ((int)(_angle / 360) * 360) : _angle);
            float newAngle = normalizedAngle < 0 ? 360 - normalizedAngle : normalizedAngle;
            float currentAngle = Vector2.Angle(_vec2.normalized, Vector2.right);
            
            currentAngle = _vec2.normalized.y < 0 ? 360 - currentAngle : currentAngle;

            newAngle = currentAngle + newAngle;

            return (new Vector2(Mathf.Cos(Mathf.Deg2Rad * newAngle), Mathf.Sin(Mathf.Deg2Rad * newAngle))) * vecMagnitude;
        }
    }
    public static class FloatUts
    {
        public static float FloatModulus(float _toDivide, float _divider) { return (_toDivide - ((int)(_toDivide / _divider) * _divider)); }

    }
    public static class PhysicsUts
    {
        public static RaycastHit[] Raycast2dCone(Vector3 _castPos, Vector2 _dir, float _coneAngle, float _angleCastFrequency, int _layerMask)
        {
            List<RaycastHit> hits = new List<RaycastHit>();
            int numOfRaycasts = (int)(_coneAngle / _angleCastFrequency);
            

            for (int i = 0; i < numOfRaycasts; i++)
            {
                RaycastHit hit;
                if (Physics.Raycast(_castPos, Vec2Uts.Vec2ToVec3XZ(Vec2Uts.RotateVec2(_dir, (-(_coneAngle / 2)) + (_angleCastFrequency * i)), _castPos.y), out hit, float.PositiveInfinity, _layerMask))
                    hits.Add(hit);
            }
            return hits.ToArray();
        }
    }
}
