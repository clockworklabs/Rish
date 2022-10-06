namespace RishUI.Events
{
    public abstract class Manipulator : UnityEngine.UIElements.Manipulator
    {
        protected sealed override void RegisterCallbacksOnTarget()
        {
            target.RegisterCallback<ResetEvent>(OnReset);
            Reset();
            
            RegisterCallbacks();
        }

        protected sealed override void UnregisterCallbacksFromTarget()
        {
            target.UnregisterCallback<ResetEvent>(OnReset);
            
            UnregisterCallbacks();
        }
        
        protected abstract void RegisterCallbacks();
        protected abstract void UnregisterCallbacks();

        private void OnReset(ResetEvent evt) => Reset();
        
        protected abstract void Reset();
    }
}