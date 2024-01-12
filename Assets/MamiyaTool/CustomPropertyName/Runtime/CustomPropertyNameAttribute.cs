using System;
using UnityEngine;

namespace MamiyaTool
{
    [AttributeUsage(validOn: AttributeTargets.Field)]
    public class CustomPropertyNameAttribute : PropertyAttribute
    {
        public readonly string displayName;
        public CustomPropertyNameAttribute(string displayName)
        {
            this.displayName = displayName;
        }
    }
}