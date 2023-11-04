using System.Collections;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace SimpleSaveSystem{
    public enum Serializer{Json, Binary}
    public static class SaveManager{
        public const string SAVEFILE_DIRECTOR = "/saves/";
        public const string GLOBALFILE_NAME = "Global.sav";
        public const string SAVEFILE_NAME = "Data.sav";
        public static GlobalSaveData globalSaveData;
        public static void CleanGameState(){
            string folderPath = Application.persistentDataPath + SAVEFILE_DIRECTOR;
            string filePath = folderPath + SAVEFILE_NAME;

            if(File.Exists(filePath)){
                File.Delete(filePath);
            }
        }
        public static void SaveGameState(){
            string folderPath = Application.persistentDataPath + SAVEFILE_DIRECTOR;

        //To save, we first Load
            var saveData = Load<Dictionary<System.Guid, object>>(folderPath, SAVEFILE_NAME, Serializer.Binary);
            if(saveData == null) saveData = new Dictionary<System.Guid, object>();

        //Capture State
            ISaveable[] saveables = Service.FindComponentsOfTypeIncludingDisable<ISaveable>();
            foreach(var saveable in saveables){
                saveData[saveable.guid] = saveable.CaptureState();
            }
            Debug.Log($"{saveables.Length} saveables in scene are saved.");

        //Save to file
            Save(folderPath, SAVEFILE_NAME, saveData, Serializer.Binary);

        //Save Global
            Save(folderPath, GLOBALFILE_NAME, globalSaveData, Serializer.Json);
        }

        public static void LoadGameState(){
            string path = Application.persistentDataPath + SAVEFILE_DIRECTOR;
            var saveData = Load<Dictionary<System.Guid, object>>(path, SAVEFILE_NAME, Serializer.Binary);
            if(saveData == null){
                Debug.LogWarning("No Valid Save Data");
                return;
            }

            ISaveable[] saveables = Service.FindComponentsOfTypeIncludingDisable<ISaveable>();
            foreach(ISaveable saveable in saveables){
                if(saveData.TryGetValue(saveable.guid, out var value)){
                    saveable.RestoreState(value);
                }
            }
            Debug.Log($"{saveables.Length} saveables in scene are loaded.");
        }
        
        public static void Initialize(){
            string folderPath = Application.persistentDataPath + SAVEFILE_DIRECTOR;
            string globalFilePath = Application.persistentDataPath + SAVEFILE_DIRECTOR + GLOBALFILE_NAME;

        // Create save folder if not found
            if(!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);

        // Create global save file if not found
            if(!File.Exists(globalFilePath)){
                globalSaveData = new GlobalSaveData();
                Save(folderPath, GLOBALFILE_NAME, globalSaveData, Serializer.Json);
            }
            else{
                globalSaveData = Load<GlobalSaveData>(folderPath, GLOBALFILE_NAME, Serializer.Json);
                if(globalSaveData==null) globalSaveData = new GlobalSaveData();
            }
        }
    #region Save and Load File
        static void Save<T>(string folderPath, string fileName, T saveData, Serializer serializer){
            if(!Directory.Exists(folderPath)){
                Directory.CreateDirectory(folderPath);
            }
            SerializeData<T>(folderPath+fileName, saveData, serializer);
        }
        static T Load<T>(string folderPath, string fileName, Serializer serializer){
            if(!File.Exists(folderPath+fileName)){
                return default(T);
            }

            var data = DeserializeData<T>(folderPath+fileName, serializer);
            return data;
        }
    #endregion

    #region Serialization
        static void SerializeData<T>(string filePath, T saveData, Serializer serializer){
            switch(serializer){
                case Serializer.Json:
                    string data = JsonConvert.SerializeObject(saveData);
                    File.WriteAllText(filePath, data);
                    break;
                default:
                    FileStream file = File.Open(filePath, FileMode.Create);
                    BinaryFormatter formatter = GetBinaryFormatter();
                    formatter.Serialize(file, saveData);
                    file.Close();
                    break;
            }
        }
        static T DeserializeData<T>(string filePath, Serializer serializer){
            switch(serializer){
                case Serializer.Json:
                    string saveData = File.ReadAllText(filePath);

                    try{
                        return JsonConvert.DeserializeObject<T>(saveData);
                    }
                    catch{
                        Debug.LogError("Save File Corrupted");
                        return default(T);
                    }
                default:
                    FileStream file = File.Open(filePath, FileMode.Open);
                    BinaryFormatter formatter = GetBinaryFormatter();

                    T data;
                    try{
                        data = (T)formatter.Deserialize(file);
                    }
                    catch{
                        Debug.LogError("Save File Corrupted");
                        data = default(T);
                    }
                    file.Close();

                    return data;
            }
        }
    #endregion
        // Formerly in SerializationManager - moved here to be shared by all Platform objects
        static BinaryFormatter GetBinaryFormatter()
        {
            BinaryFormatter formatter = new BinaryFormatter();

            SurrogateSelector selector = new SurrogateSelector();
            Vector3SerializationSurrogate vector3Surrogate = new Vector3SerializationSurrogate();
            QuaternionSerializationSurrogate quaternionSurrogate = new QuaternionSerializationSurrogate();

            selector.AddSurrogate(typeof(Vector3), new StreamingContext(StreamingContextStates.All), vector3Surrogate);
            selector.AddSurrogate(typeof(Quaternion), new StreamingContext(StreamingContextStates.All), quaternionSurrogate);

            formatter.SurrogateSelector = selector;

            return formatter;
        }
    }
}