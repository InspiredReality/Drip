namespace Drip.Phases
{
    public interface IPhaseController
    {
        void EnterPhase();
        void ExitPhase();
        bool IsPhaseComplete();
    }
}
