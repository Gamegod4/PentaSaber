namespace PentaSaber
{
    public interface IInputController
    {
        //0 default
        //1 secondary
        //3 tertiary
        public int SaberAState { get; }
        public int SaberBState { get; }
    }
}
