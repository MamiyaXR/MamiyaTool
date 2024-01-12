namespace MamiyaTool
{
    public class Decorator : Behavior
    {
        protected Behavior child;
        /******************************************************************
         * 
         * 
         * 
         ******************************************************************/
        public Decorator(Behavior child)
        {
            this.child = child;
        }
    }
}