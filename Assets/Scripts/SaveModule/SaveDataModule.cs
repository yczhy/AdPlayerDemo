using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Duskvern
{
    public static class SaveDataModule
    {
        private static SaveDataConfig saveDataConfig;
        #region 流程

        public static void Init()
        {
            saveDataConfig = Resources.Load<SaveDataConfig>("SaveDataConfig");
            if (saveDataConfig == null)
            {
                Logger.LogError("SaveDataModule", "存档配置为空");
                return;
            }

            saveDataConfig.Init();
            // time = 0;
            LoadSaveData();
        }

        // private float time = 0;
        // private float timer = 3f;
        public static void OnUpdate(float deltaTime) // 暂时这里的代码先留着
        {
            // time += deltaTime;
            // if (time > timer)
            // {
            //     time = 0;
            //     SaveData();
            // }
        }

        #endregion

        #region 存档流程

        private static Dictionary<string, ISaveData> saveDataDict = new();
        private static Dictionary<Type, string> typeToKeyMap = new();

        public static void LoadSaveData() // 加载所有存档
        {
            var assembly = Assembly.GetExecutingAssembly();
            var saveDataTypes = assembly.GetTypes()
            .Where(type => typeof(ISaveData).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
            .ToList();

            foreach (var type in saveDataTypes)
            {
                var instance = (ISaveData)System.Activator.CreateInstance(type);
                var dataType = instance.GetType();
                var key = GetSaveKey(type);
                var data = LoadData(dataType, key, instance);
                saveDataDict.Add(key, data);
            }
        }

        private static ISaveData LoadData<T>(Type dataType, string key, T defaultInstance)
        {
            var method = saveDataConfig.GetType().GetMethod("Load").MakeGenericMethod(dataType);
            return (ISaveData)method.Invoke(saveDataConfig, new object[] { key, defaultInstance });
        }

        public static void SaveData() // 保存所有的存档
        {
            foreach (var pair in saveDataDict)
            {
                saveDataConfig.Save(pair.Key, pair.Value);
            }
        }

        public static void SaveData<T>() where T : ISaveData
        {
            string key = GetSaveKey(typeof(T));
            if (saveDataDict.TryGetValue(key, out var data))
            {
                saveDataConfig.Save(key, data);
            }
            else
            {
                Logger.LogError("SaveDataModule", "没有找到存档数据" + key);
            }
        }

        public static T GetData<T>() where T : ISaveData
        {
            string key = GetSaveKey(typeof(T));
            if (saveDataDict.ContainsKey(key))
            {
                return (T)saveDataDict[key];
            }
            else
            {
                var type = typeof(T);
                var instance = (ISaveData)System.Activator.CreateInstance(type);
                var data = LoadData(type, key, instance);
                saveDataDict[key] = data;
                return (T)data;
            }
        }

        public static string GetSaveKey(Type type)
        {
            if (!typeToKeyMap.TryGetValue(type, out var key))
            {
                // 如果没有缓存，获取键并缓存
                key = type.Name; // 或者使用其他方法获取存档键
                typeToKeyMap[type] = key;
            }
            return key;
        }

        #endregion

        #region 辅助方法

        [Button("清除指定的存档")]
        public static void ClearSaveData(string key)
        {
            Logger.Log("SaveDataModule", $"清除 {key} 的存档");
            saveDataConfig.Delete(key);
        }

        [Button("清除所有存档")]
        public static void ClearAllSaveData()
        {
            Logger.Log("SaveDataModule" , "清除所有的存档");
            saveDataConfig.DeleteAll();
        }
        #endregion
    }
}

