using Core.Enum;

namespace Core.Model
{
    public interface ISelectable
    {
        void Select();
        void Unselect();
        bool IsSelected();
        void SetAction(GameAction action);

        /// <summary>
        /// Можно ли выбрать объект из пулла
        /// </summary>
        /// <returns>Возвращает true, если объект можно выбрать, в противном случае возвращает false</returns>
        bool CanSelect();

        /// <summary>
        /// Занят ли выбранный объект (нельзя проводить процедуру выбора следующего объекта)
        /// </summary>
        /// <returns>Возвращает true, если объект занят, в противном случае возвращает false</returns>
        bool IsBusy();
    }
}