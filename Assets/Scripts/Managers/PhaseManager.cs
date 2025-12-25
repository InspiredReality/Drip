using UnityEngine;
using Drip.Phases;

namespace Drip.Managers
{
    public class PhaseManager : MonoBehaviour
    {
        [Header("Phase Controllers")]
        [SerializeField] private DropPhaseController dropPhaseController;
        [SerializeField] private DividePhaseController dividePhaseController;
        [SerializeField] private DousePhaseController dousePhaseController;

        private IPhaseController currentPhaseController;

        private void Awake()
        {
            InitializePhaseControllers();
        }

        private void InitializePhaseControllers()
        {
            if (dropPhaseController == null)
                dropPhaseController = GetComponent<DropPhaseController>();
            
            if (dividePhaseController == null)
                dividePhaseController = GetComponent<DividePhaseController>();
            
            if (dousePhaseController == null)
                dousePhaseController = GetComponent<DousePhaseController>();
        }

        public void SetPhase(GamePhase phase)
        {
            // Exit current phase
            if (currentPhaseController != null)
            {
                currentPhaseController.ExitPhase();
            }

            // Enter new phase
            switch (phase)
            {
                case GamePhase.Drop:
                    currentPhaseController = dropPhaseController;
                    break;
                case GamePhase.Divide:
                    currentPhaseController = dividePhaseController;
                    break;
                case GamePhase.Douse:
                    currentPhaseController = dousePhaseController;
                    break;
            }

            currentPhaseController?.EnterPhase();
        }

        public void CompletePhase()
        {
            if (currentPhaseController != null && currentPhaseController.IsPhaseComplete())
            {
                GameManager.Instance.AdvancePhase();
            }
        }
    }
}
