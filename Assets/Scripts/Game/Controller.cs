using UnityEngine;

namespace Game
{
    public class Controller : MonoBehaviour
    {
        // This controller class is used to keep references to all controllers

        public static DisplayController DisplayController;
        public static DragController DragController;
        public static TagController TagController;
        public static UIScoreController PlayerUIScoreController;
        public static UIScoreController OpponentUIScoreController;
        public static RoundResultsController RoundResultsController;

        void Start()
        {
            DisplayController = GetComponent<DisplayController>();
            DragController = GetComponent<DragController>();
            TagController = GetComponent<TagController>();
            PlayerUIScoreController = GetComponents<UIScoreController>()[0];
            OpponentUIScoreController = GetComponents<UIScoreController>()[1];
            RoundResultsController = GetComponent<RoundResultsController>();
        }
    }
}
