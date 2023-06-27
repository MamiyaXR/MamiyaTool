using UnityEditor;
using UnityEngine;

namespace MamiyaTool {
    public static class GizmosAuto {
        [DrawGizmo(GizmoType.Selected | GizmoType.NonSelected)]
        public static void DrawWireCube(BoxCollider boxCollider, GizmoType gizmosType) {
            var originColor = GUI.color;
            GUI.color = Color.white;
            //DrawWireCube(boxCollider.center, boxCollider.size, boxCollider.transform.rotation);
            Vector3 center = boxCollider.transform.localToWorldMatrix * boxCollider.center;
            center += boxCollider.transform.position;
            Vector3 size = Vector3.Scale(boxCollider.size, boxCollider.transform.localScale);
            GizmosExtensions.DrawWireCube(center, size, boxCollider.transform.rotation);
            GUI.color = originColor;
        }

        [DrawGizmo(GizmoType.Selected | GizmoType.NonSelected)]
        public static void DrawWireSphere(SphereCollider sphereCollider, GizmoType gizmosType) {
            var originColor = GUI.color;
            GUI.color = Color.white;
            //DrawWireSphere(sphereCollider.center, sphereCollider.radius, sphereCollider.transform.rotation);
            Vector3 center = sphereCollider.transform.localToWorldMatrix * sphereCollider.center;
            center += sphereCollider.transform.position;
            float radius = sphereCollider.radius * Mathf.Max(sphereCollider.transform.localScale.x,
                                                                            sphereCollider.transform.localScale.y,
                                                                            sphereCollider.transform.localScale.z);
            GizmosExtensions.DrawWireSphere(center, radius, sphereCollider.transform.rotation);
            GUI.color = originColor;
        }
    }
}