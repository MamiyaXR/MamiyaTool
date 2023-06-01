using UnityEngine;

namespace MamiyaTool
{
    public class InspectorButtonAttribute : PropertyAttribute
    {
        public readonly string methodName;
        public InspectorButtonAttribute(string methodName)
        {
            this.methodName = methodName;
        }
    }
}