using UnityEngine;
using System;

namespace Duskvern
{
    public static class SaveUtils
    {
        public static void Init()
        {
            SaveDataModule.Init();
        }

        public static void Update(float deltaTime)
        {
            SaveDataModule.OnUpdate(deltaTime);
        }

        public static void LoadAllData()
        {
            SaveDataModule.LoadSaveData();
        }

        public static void SaveAllData()
        {
            SaveDataModule.SaveData();
        }

        public static void SaveData<T>() where T : ISaveData
        {
            SaveDataModule.SaveData<T>();
        }

        public static T GetData<T>() where T : ISaveData
        {
            var data = SaveDataModule.GetData<T>();
            return data;
        }

        public static void DeleteAllData() 
        {
            SaveDataModule.ClearAllSaveData();
        }

        public static void DeleteData<T>() where T : ISaveData
        {
            SaveDataModule.ClearSaveData<T>();
        }
    }
}

