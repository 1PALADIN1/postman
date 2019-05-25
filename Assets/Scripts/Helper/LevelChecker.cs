using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Core.Helper
{
    public class LevelChecker
    {
        private List<string> _levels;
        private List<int> _levelsNum;

        /// <summary>
        /// Список уровней в числовом представлении
        /// </summary>
        public List<int> LevelsInt
        {
            get => _levelsNum;
        }
        
        /// <summary>
        /// Путь до папки с уровнями
        /// </summary>
        public static string LevelsPath
        {
            get => Application.dataPath + "/StreamingAssets/Levels/";
        }

        /// <summary>
        /// Префикс для файлов уровней
        /// </summary>
        public static string FilePrefix
        {
            get => "level";
        }

        /// <summary>
        /// Расширение файлов с уровнями
        /// </summary>
        public static string LevelFileExt
        {
            get => ".lvl";
        }

        public LevelChecker()
        {
            _levels = new List<string>();
            _levelsNum = new List<int>();
            Refresh();
        }

        /// <summary>
        /// Переподгружает уровни
        /// </summary>
        public void Refresh()
        {
            int levelNum = 0;

            foreach (var level in Directory.GetFiles(LevelsPath))
            {
                if (level.EndsWith(".lvl"))
                {
                    _levels.Add(level.Substring(level.IndexOf("level")));
                    levelNum++;
                    _levelsNum.Add(levelNum);
                }
            }
        }

        /// <summary>
        /// Проверка на наличие следующего уровня
        /// </summary>
        /// <param name="currentLevelNumber">Номер текущего уровня</param>
        /// <returns>Возвращает true, если есть следующий уровень, иначе - false</returns>
        public bool HasNextLevel(int currentLevelNumber)
        {
            currentLevelNumber++;
            if (currentLevelNumber > _levelsNum.Count - 1)
                return false;
            return true;
        }
    }
}