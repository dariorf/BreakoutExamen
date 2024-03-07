using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;

public static class SaveManager
{
    private static string path = Application.persistentDataPath + "/save.dat";
    private static List<int> records = new List<int>();

    public static void SaveRecord(int pooints)
    {
        bool isRecord = false;

        SortRecords();

        if (records.Count < 5 && !records.Contains(pooints))
        {
            isRecord = true;
            records.Add(pooints);
        }
        else
        {
            foreach (int record in records)
            {
                if (pooints == record)
                {
                    break;
                }

                if (pooints > record)
                {
                    records.RemoveAt(records.Count - 1);

                    records.Add(pooints);

                    isRecord = true;

                    break;
                }
            }
        }

        if (isRecord)
        {
            SortRecords();
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Create);

            formatter.Serialize(stream, records);
            stream.Close();
        }
    }

    public static List<int> LoadRecord()
    {
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            records = formatter.Deserialize(stream) as List<int>;  // equivalent to (RecordData)formatter.Deserialize(stream);
            stream.Close();
        }

        SortRecords();

        return records;
    }

    private static void SortRecords()
    {
        List<int> aux = new List<int>();
        aux.AddRange(records);
        aux.Sort();

        int j = aux.Count - 1;
        for (int i = 0; i < aux.Count; i++)
        {
            records[j] = aux[i];
            j--;
        }
    }
}
