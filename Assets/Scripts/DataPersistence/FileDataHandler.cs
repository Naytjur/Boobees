using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class FileDataHandler
{
    private string dataDirPath = "";
    private string dataFileName = "";


    public FileDataHandler(string dataDirPath, string dataFileName)
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
    }

    public GameData Load()
    {
        //Path.Combine is apaprently to account for different OS' file separators cause / is not universal
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        GameData loadedData = null;
        if (File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";
                using ( FileStream stream = new FileStream(fullPath,FileMode.Open))
                {
                    using ( StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch (Exception e)
            {
                Debug.LogError("Error trying to load data from file:" + fullPath + "\n" + e);
            }
        }
        return loadedData;
    }

    public void Save(GameData data)
    {
        //Path.Combine is apaprently to account for different OS' file separators cause / is not universal
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        try
        {
            //make irectory if it doesnt exist
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));


            //serialize into json
            string dataToStore = JsonUtility.ToJson(data,true);

            //write to system

            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }

        }
        catch (Exception e)
        {
            Debug.LogError("Error trying to save to data file:" + fullPath + "\n" + e);
        }
    }
}
