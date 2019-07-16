using System;

namespace Core.Helper
{
    [Serializable]
    struct ProgressInfo
    {
        private int _collectedStars;
        private bool _isFinished;

        public bool IsFinished => _isFinished;

        public int CollectedStars => _collectedStars;

        public ProgressInfo(int collectedStars, bool isFinished)
        {
            _collectedStars = collectedStars;
            _isFinished = isFinished;
        }

        public override string ToString()
        {
            return $"[Stars = {CollectedStars}, Finished = {_isFinished}]";
        }
    }
}