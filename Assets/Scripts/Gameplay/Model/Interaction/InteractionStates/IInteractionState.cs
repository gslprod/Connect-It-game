using UnityEngine;

namespace ConnectIt.Model.InteractionStates
{
    public interface IInteractionState
    {
        public void OnClick(Vector2 positionOnScreen);
    }
}
