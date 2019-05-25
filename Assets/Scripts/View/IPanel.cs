namespace Core.View
{
    public interface IPanel
    {
        float PaddingTop { get; }
        float PaddingBottom { get; }
        float PaddingLeft { get; }
        float PaddingRight { get; }
        float Width { get; }
        float Height { get; }

        BaseView[] GetAllChilds();
        IButton[] GetAllChildButtons();
        BaseView[] GetAllChildButtonsAsBase();
    }
}