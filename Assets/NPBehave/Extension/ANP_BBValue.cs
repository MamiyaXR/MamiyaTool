using System;

namespace NPBehave {
    public abstract class ANP_BBValue {
        public abstract Type NP_BBValueType { get; }
        public abstract void SetValueFrom(ANP_BBValue anpBbValue);
    }
}