using Core.Enum;

namespace Core.Model
{
    public interface ISelectable
    {
        void Select();
        void Unselect();
        bool IsSelected();
        void SetAction(GameAction action);
        bool CanSelect();
    }
}