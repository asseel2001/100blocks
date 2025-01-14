using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Unity.VisualScripting;
using UnityEngine;


public class BinaryDataStream 
{
   public static void save<T>(T serializedObject, string fileName)
   {
        string path = Application.persistentDataPath + "/saves/";
        Directory.CreateDirectory(path);

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream fileStream=new FileStream(path+ fileName + ".dat", FileMode.Create);

        try
        {
            formatter.Serialize(fileStream, serializedObject);

        }
        catch (SerializationException e)
        { 
            Debug.Log("save filed. error :"+ e.Message);
        }
        finally
        {
            fileStream.Close();
        }

   }

    public static bool Exist(string filename)
    {
        string path= Application.persistentDataPath + "/saves/";
        string fullfilename = filename + ".dat";
        return File.Exists(path + fullfilename);
    }

    public static T Read<T>(string FileName)
    {
        string path = Application.persistentDataPath + "/saves/";
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream fileStream = new FileStream(path+ FileName+ ".dat", FileMode.Open );
        T returnType= default(T);
        try
        {
            returnType = (T)formatter.Deserialize(fileStream);

        }
        catch(SerializationException e)
        {
            Debug.Log("read filed. error : " + e.Message);
        }
        finally
        {
            fileStream.Close();
        }
        return returnType;
    }
}
