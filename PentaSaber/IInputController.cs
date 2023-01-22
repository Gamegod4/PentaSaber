namespace PentaSaber
{
    public interface IInputController
    {
        //0 default
        //1 secondary
        //2 tertiary
        public int SaberAState { get; }
        public int SaberBState { get; }
    }
}
