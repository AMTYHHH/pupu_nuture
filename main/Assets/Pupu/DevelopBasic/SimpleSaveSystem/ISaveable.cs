namespace SimpleSaveSystem{
    public interface ISaveable
    {
        System.Guid guid{get;}
        void RestoreState(object state);
        object CaptureState();
    }
}
