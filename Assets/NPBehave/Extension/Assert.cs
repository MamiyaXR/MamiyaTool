namespace NPBehave {
    public class Assert {
        private static void AssertInner(bool result, string errorMessage = "") {
            if(!result) {
                Log.Error($"NPBehave Assert Fail!!!: {errorMessage}");
            }
        }
        public static void AreEqual<T>(T l, T r, string errorMessage = "") {
            AssertInner(l.Equals(r), errorMessage);
        }
        public static void AreNotEqual<T>(T l, T r, string errorMessage = "") {
            AssertInner(!l.Equals(r), errorMessage);
        }
        public static void IsTrue(bool result, string errorMessage = "") {
            AssertInner(result, errorMessage);
        }
        public static void IsFalse(bool result, string errorMessage = "") {
            AssertInner(!result, errorMessage);
        }
        public static void IsNotNull<T>(T value, string errorMessage = "") where T : class {
            AssertInner(value != null, errorMessage);
        }
    }
}