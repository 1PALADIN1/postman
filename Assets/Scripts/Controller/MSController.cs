using Core.Enum;
using Core.Helper;
using Core.Model;
using System.IO;
using UnityEngine;

namespace Core.Controller
{
    /// <summary>
    /// Сериализатор игровых уровней
    /// </summary>
    public class MSController : BaseController
    {
        private GameStore _gameStore;
        private BaseModel[] _allModels;
        private string _saveFolder = LevelChecker.LevelsPath;
        private string _fileName = LevelChecker.FilePrefix;
        private string _levelFileExt = LevelChecker.LevelFileExt;

        //шаблоны
        private GameObject _boxPrefab;
        private GameObject _wallPrefab;
        private GameObject _starPrefab;
        private GameObject _finishPointPrefab;
        private GameObject _floorPrefab;

        public override void Init()
        {
            _gameStore = GameStore.Store;
            _allModels = _gameStore.AllModels;
        }

        public void SetPrefabs(params GameObject[] prefabs)
        {
            _boxPrefab = prefabs[0];
            _wallPrefab = prefabs[1];
            _starPrefab = prefabs[2];
            _finishPointPrefab = prefabs[3];
            _floorPrefab = prefabs[4];
        }
        
        //TODO debug DEV function
        public void SaveLevel(int levelNum)
        {
            _fileName = LevelChecker.FilePrefix + levelNum + _levelFileExt;

            if (!Directory.Exists(_saveFolder))
                Directory.CreateDirectory(_saveFolder);

            FileStream fileStream;

            if (!File.Exists(_saveFolder + _fileName))
                fileStream = File.Create(_saveFolder + _fileName);
            else
                fileStream = new FileStream(_saveFolder + _fileName, FileMode.Truncate);

            //запись объектов в файл
            using (StreamWriter streamWriter = new StreamWriter(fileStream))
            {
                foreach (var model in _allModels)
                {
                    Vector3 modelPosition = model.transform.position;
                    if (model.SerializeType != EntityType.None)
                    {
                        string extra = string.Empty;
                        if (model is MailBox) extra += "|" + (int)(model as MailBox).Color;
                        if (model is FinishPoint) extra += "|" + (int)(model as FinishPoint).Color;

                        streamWriter.WriteLine($"{(int)model.SerializeType}|{modelPosition.x}:{modelPosition.y}:{modelPosition.z}{extra}");
                    }
                }
            }

            fileStream.Close();

            //TODO debug info
            Debug.Log($"Уровень записан в файл {_fileName} по пути {_saveFolder}");
        }


        /// <summary>
        /// Загрузка уровня на сцену
        /// </summary>
        /// <param name="levelNum">Номер уровня</param>
        /// <param name="debugMode">Включить режим отладки (по умолчанию true)</param>
        public void LoadLevel(int levelNum, bool debugMode)
        {
            _fileName = $"{LevelChecker.FilePrefix}{levelNum}{_levelFileExt}";

            if (Application.platform != RuntimePlatform.Android)
            {
                if (!Directory.Exists(_saveFolder))
                {
                    //TODO debug info
                    Debug.Log($"Не удалось найти директорию {_saveFolder}");
                    return;
                }

                if (!File.Exists(_saveFolder + _fileName))
                {
                    //TODO debug info
                    Debug.Log($"Не удалось найти игровой уровень {_saveFolder + _fileName}");
                    return;
                }
            }

            GameObject wallsDir = GameObject.Find("Walls");
            GameObject starsDir = GameObject.Find("Stars");
            GameObject boxesDir = GameObject.Find("Boxes");
            GameObject fpsDir = GameObject.Find("Finish");
            GameObject floorDir = GameObject.Find("Floor");

            Vector3 tmpVector3 = new Vector3();

            string[] lines = null;
            if (Application.platform == RuntimePlatform.Android)
            {
                BetterStreamingAssets.Initialize();
                lines = BetterStreamingAssets.ReadAllLines(_saveFolder + _fileName);
            }
            else
            {
                lines = File.ReadAllLines(_saveFolder + _fileName);
            }

            foreach (var model in lines)
            {
                if (model.Trim() == string.Empty) continue;

                string[] data = model.Split('|');
                if (data.Length == 0) continue;

                int.TryParse(data[0].Trim(), out int entityTypeInt);
                EntityType entity = (EntityType) entityTypeInt;

                int.TryParse(data[1].Split(':')[0].Trim(), out int xPos);
                int.TryParse(data[1].Split(':')[1].Trim(), out int yPos);
                int.TryParse(data[1].Split(':')[2].Trim(), out int zPos);

                tmpVector3.Set(xPos, yPos, zPos);

                switch (entity)
                {
                    case EntityType.Box:
                        var boxInst = GameObject.Instantiate(_boxPrefab, tmpVector3, Quaternion.identity, boxesDir.transform);
                        int.TryParse(data[2].Trim(), out int boxColor);
                        boxInst.GetComponent<MailBox>().Color = (BoxColor) boxColor;
                        break;
                    case EntityType.Star:
                        GameObject.Instantiate(_starPrefab, tmpVector3, Quaternion.identity, starsDir.transform);
                        break;
                    case EntityType.Wall:
                        GameObject.Instantiate(_wallPrefab, tmpVector3, Quaternion.identity, wallsDir.transform);
                        break;
                    case EntityType.FinishPoint:
                        var finishInst = GameObject.Instantiate(_finishPointPrefab, tmpVector3, Quaternion.identity, fpsDir.transform);
                        int.TryParse(data[2].Trim(), out int finishColor);
                        finishInst.GetComponent<FinishPoint>().Color = (BoxColor) finishColor;
                        break;
                    case EntityType.Floor:
                        GameObject.Instantiate(_floorPrefab, tmpVector3, Quaternion.identity, floorDir.transform);
                        break;
                }
            }

            //подвязываем модели
            if (debugMode)
            {
                _gameStore.FillModelArrays();
                _gameStore.InitAllControllers();
            }

            //TODO debug info
            Debug.Log($"Уровень {_fileName} загружен из директории {_saveFolder}");
        }
    }
}