namespace PentaSaber
{
    public interface IInputController
    {
        public bool SaberAToggled { get; }
        public bool SaberBToggled { get; }
    }
    public interface ITestInputController : IInputController
    {
        public bool NoteAToggled { get; }
        public bool NoteBToggled { get; }
        public bool NeutralToggled { get; }
    }
}
