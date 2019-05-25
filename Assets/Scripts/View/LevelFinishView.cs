using UnityEngine.UI;

namespace Core.View
{
    public class LevelFinishView : BaseView
    {
        private int _totalStars = 0;
        private int _collectedStars = 0;
        private Text _finishedText;

        public override void Init()
        {
            _finishedText = gameObject.GetComponentInChildren<Text>(true);
        }

        public void SetStarsFinished(int totalStars, int collectedStars)
        {
            _totalStars = totalStars;
            _collectedStars = collectedStars;

            _finishedText.text = $"Поздравляем! Уровень завершен!\nВы собрали {collectedStars} из {totalStars} звёзд.";
        }
    }
}
