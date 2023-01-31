using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MamiyaTool
{
    [DisallowMultipleComponent]
    public class KinematicFollower : MonoBehaviour
    {
        [SerializeField] private EUpdateMethod updateMethod;
        public Transform target;
        public float f;
        public float z;
        public float r;

        private float k1 { get => z / Mathf.PI / f; }
        private float k2 { get => 1 / Mathf.Pow(2f * Mathf.PI * f, 2); }
        private float k3 { get => r * z / (2 * Mathf.PI * f); }
        private float deltaTime {
            get {
                switch(updateMethod) {
                    case EUpdateMethod.FixedUpdate:
                        return Time.fixedDeltaTime;
                    case EUpdateMethod.Update:
                    case EUpdateMethod.LateUpdate:
                        return Time.deltaTime;
                    default:
                        return 0;
                }
            }
        }

        private Vector3 targetPosCatch;
        private Vector3 speed;
        /******************************************************************
         *
         *
         *
         ******************************************************************/
        private void Start()
        {
            targetPosCatch = target.transform.position;
            speed = Vector3.zero;
        }
        private void FixedUpdate()
        {
            if(updateMethod == EUpdateMethod.FixedUpdate)
                Move();
        }
        private void Update()
        {
            if(updateMethod == EUpdateMethod.Update)
                Move();
        }
        private void LateUpdate()
        {
            if(updateMethod == EUpdateMethod.LateUpdate)
                Move();
        }
        /******************************************************************
         *
         *
         *
         ******************************************************************/
        private (float, float) MoveAxis(float yPre, float dyPre, float x, float xPre, float delta)
        {
            float k2_stable = Mathf.Max(k2, delta * delta / 2f + delta * k1 / 2, delta * k1);
            float y = yPre + dyPre * delta;
            float dx = (x - xPre) / delta;
            float a = (x + k3 * dx - y - k1 * dyPre) / k2_stable;
            float dy = dyPre + a * delta;
            return (y, dy);
        }
        private void Move()
        {
            (float, float) x = MoveAxis(transform.position.x, speed.x, target.position.x, targetPosCatch.x, deltaTime);
            (float, float) y = MoveAxis(transform.position.y, speed.y, target.position.y, targetPosCatch.y, deltaTime);
            (float, float) z = MoveAxis(transform.position.z, speed.z, target.position.z, targetPosCatch.z, deltaTime);

            transform.position = new Vector3(x.Item1, y.Item1, z.Item1);
            speed = new Vector3(x.Item2, y.Item2, z.Item2);
            targetPosCatch = target.position;
        }

        public enum EUpdateMethod
        {
            FixedUpdate = 0,
            Update,
            LateUpdate,
        }
    }
}