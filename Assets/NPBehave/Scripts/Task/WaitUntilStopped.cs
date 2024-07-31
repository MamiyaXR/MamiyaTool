namespace NPBehave
{
    public class WaitUntilStopped : Task
    {
        private bool successWhenStopped;
        public WaitUntilStopped(bool successWhenStopped = false) : base("WaitUntilStopped")
        {
            this.successWhenStopped = successWhenStopped;
        }

        protected override void DoCancel()
        {
            this.Stopped(successWhenStopped);
        }
    }
}