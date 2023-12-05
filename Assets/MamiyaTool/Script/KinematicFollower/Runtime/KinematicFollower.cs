using UnityEngine;

namespace MamiyaTool {
    [DisallowMultipleComponent]
    public class KinematicFollower : MonoBehaviour {
        [SerializeField] private EUpdateMethod updateMethod;
        public Transform target;
        public KinematicHandle handle;
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
         *      lifecycle
         *
         ******************************************************************/
        private void Start() {
            targetPosCatch = target == null ? transform.position : target.position;
            speed = Vector3.zero;
        }
        private void FixedUpdate() {
            if(updateMethod == EUpdateMethod.FixedUpdate)
                Move();
        }
        private void Update() {
            if(updateMethod == EUpdateMethod.Update)
                Move();
        }
        private void LateUpdate() {
            if(updateMethod == EUpdateMethod.LateUpdate)
                Move();
        }
        /******************************************************************
         *
         *      public method
         *
         ******************************************************************/
        public void SetTarget(Transform target) {
            this.target = target;
            targetPosCatch = target == null ? transform.position : target.position;
        }
        /******************************************************************
         *
         *      private method
         *
         ******************************************************************/
        private void Move() {
            if(target == null)
                return;
            (float, float) x = handle.Invoke(transform.position.x, speed.x, target.position.x, targetPosCatch.x, deltaTime);
            (float, float) y = handle.Invoke(transform.position.y, speed.y, target.position.y, targetPosCatch.y, deltaTime);
            (float, float) z = handle.Invoke(transform.position.z, speed.z, target.position.z, targetPosCatch.z, deltaTime);

            transform.position = new Vector3(x.Item1, y.Item1, z.Item1);
            speed = new Vector3(x.Item2, y.Item2, z.Item2);
            targetPosCatch = target.position;
        }
        /******************************************************************
         *
         *      define
         *
         ******************************************************************/
        public enum EUpdateMethod {
            FixedUpdate = 0,
            Update,
            LateUpdate,
        }
    }
}