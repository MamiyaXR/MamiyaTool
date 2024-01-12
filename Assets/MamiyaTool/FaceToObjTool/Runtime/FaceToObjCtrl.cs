using UnityEngine;

namespace MamiyaTool {
    public class FaceToObjCtrl : MonoBehaviour {
        [SerializeField] private Transform target;
        [SerializeField] private EDirType faceAxisType;
        [SerializeField] private EDirType alignAxisType;
        [SerializeField] private EAlignType alignType;
        /**************************************************************************
         * 
         *      lifecycle
         * 
         **************************************************************************/
        private void LateUpdate() {
            if(target == null)
                return;

            Vector3 sFaceAxis = GetDir(faceAxisType);
            Vector3 wFaceAxis = transform.TransformDirection(sFaceAxis);
            Vector3 fwFaceAxis = target.position - transform.position;
            Vector3 fForward = Quaternion.FromToRotation(wFaceAxis, fwFaceAxis) * transform.forward;

            Vector3 sAlignAxis = GetDir(alignAxisType);
            Vector3 wAlignAxis = transform.TransformDirection(sAlignAxis);
            Vector3 fwAlignAxis = GetWAlignAxis(alignType, sAlignAxis, target);
            Vector3 fUp = Quaternion.FromToRotation(wAlignAxis, fwAlignAxis) * transform.up;

            Vector3 fRight = Vector3.Cross(fUp, fForward);
            switch(alignType) {
                case EAlignType.World: fForward = Vector3.Cross(fRight, fUp); break;
                case EAlignType.Target: fUp = Vector3.Cross(fForward, fRight); break;
                default: break;
            }

            transform.rotation = Quaternion.LookRotation(fForward, fUp);
        }
        /**************************************************************************
         * 
         *      public method
         * 
         **************************************************************************/
        public void SetTarget(Transform target) {
            this.target = target;
        }
        /**************************************************************************
         * 
         *      private method
         * 
         **************************************************************************/
        private Vector3 GetDir(EDirType type) {
            switch(type) {
                case EDirType.Forward: return Vector3.forward;
                case EDirType.Up: return Vector3.up;
                case EDirType.Right: return Vector3.right;
                case EDirType.Back: return Vector3.back;
                case EDirType.Down: return Vector3.down;
                case EDirType.Left: return Vector3.left;
                default: return Vector3.forward;
            }
        }
        private Vector3 GetWAlignAxis(EAlignType type, Vector3 sAlignAxis, Transform target) {
            switch(type) {
                case EAlignType.World: return sAlignAxis;
                case EAlignType.Target: return target.TransformDirection(sAlignAxis);
                default: return sAlignAxis;
            }
        }
        /**************************************************************************
         * 
         *      define
         * 
         **************************************************************************/
        private enum EDirType {
            Forward = 0,
            Up,
            Right,
            Back,
            Down,
            Left,
        }
        private enum EAlignType {
            World = 0,
            Target,
        }
    }
}