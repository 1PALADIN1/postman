using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Core.Helper
{
    /// <summary>
    /// Класс, отвечающий за игровой прогресс
    /// </summary>
    public class ProgressLoader
    {
        //прогресс
        private Dictionary<int, ProgressInfo> _progress;
        private LevelChecker _levelChecker;

        //файл сохранения
        public string SaveFileFullPath => $"{Application.dataPath}/StreamingAssets/progress.sv";

        public int CanStartLevel => _progress.Count + 1;
        
        public ProgressLoader()
        {
            _progress = new Dictionary<int, ProgressInfo>();
            _levelChecker = new LevelChecker();
        }

        public void SaveGameProgress()
        {
            if (_progress.Count == 0)
                return;

            //TODO debug
            Debug.Log($"Сохранение прогресса. Количество уровней: {_progress.Count}");

            FileStream saveFile;
            if (!File.Exists(SaveFileFullPath)) saveFile = File.Create(SaveFileFullPath);
            else
                saveFile = File.Open(SaveFileFullPath, FileMode.Truncate);

            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(saveFile, _progress);
            saveFile.Close();

            //TODO debug
            Debug.Log("Прогресс сохранён");
        }

        /// <summary>
        /// Загрузка игрового прогресса
        /// </summary>
        public void LoadGameProgress()
        {
            if (!File.Exists(SaveFileFullPath))
                return;

            //TODO debug
            Debug.Log($"Загрузка игрового прогресса. Уровней в списке: {_progress.Count}");

            BinaryFormatter formatter = new BinaryFormatter();
            FileStream saveFile = File.Open(SaveFileFullPath, FileMode.Open);
            _progress = (Dictionary<int, ProgressInfo>) formatter.Deserialize(saveFile);
            saveFile.Close();

            //TODO debug
            Debug.Log($"Игровой прогресс загружен. Уровней в списке: {_progress.Count}");
        }

        /// <summary>
        /// Сохранение прогресса для уровня
        /// </summary>
        /// <param name="level">Номер уровня</param>
        /// <param name="stars">Количество собранных звёзд</param>
        public void SaveFinishLevel(int level, int stars)
        {
            if (_levelChecker.HasLevel(level))
            {
                if (_progress.ContainsKey(level)) _progress[level] = new ProgressInfo(stars, true);
                else
                    _progress.Add(level, new ProgressInfo(stars, true));
                SaveGameProgress();
            }
        }

        /// <summary>
        /// Количество собранных звёзд на уровне
        /// </summary>
        /// <param name="level">Номер уровня</param>
        /// <returns>Количество собранных звёзд</returns>
        public int StarsInLevel(int level)
        {
            if (_progress.ContainsKey(level))
                return _progress[level].CollectedStars;
            return 0;
        }
    }
}