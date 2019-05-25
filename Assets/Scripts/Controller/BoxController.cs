using Core.Enum;
using Core.Model;

namespace Core.Controller
{
    public class BoxController : BaseController
    {
        private GameStore _gameStore;
        private SelectController _selectController;

        public override void Init()
        {
            _gameStore = GameStore.Store;
            _selectController = _gameStore.GetController<SelectController>();
        }

        public void SetAction(GameAction boxAction)
        {
            ISelectable selectedBox = _selectController.SelectedObject;
            selectedBox.SetAction(boxAction);
        }
    }
}
