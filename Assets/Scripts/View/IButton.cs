using UnityEngine.Events;

namespace Core.View
{
    public interface IButton
    {
        void SetAction(UnityAction action);
    }
}