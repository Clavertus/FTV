using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class EditorMethods : Editor
{
    const string extension = ".cs";
    public static void WriteToEnum<T>(string path, string name, ICollection<T> data)
    {
        //Delete the file if a file with the given name alredy exists 
        if (File.Exists(path + name + extension))
        {
            DeleteEnum(path, extension);
        }
        using (StreamWriter file = File.CreateText(path + name + extension))
        {
            //writing the code in the .cs file with the help of StreamWriter
            file.WriteLine("public enum " + name + " \n{");

            int i = 0;
            //adding every element of the data list as an enum element whilst giving it a value that may be used as an index
            foreach (var line in data)
            {
                string lineRep = line.ToString().Replace(" ", string.Empty);
                if (!string.IsNullOrEmpty(lineRep))
                {
                    file.WriteLine(string.Format("\t{0} = {1},",
                        lineRep, i));
                    i++;
                }
            }

            file.WriteLine("\n}");
        }
        //this imports an Asset at the specified path
        AssetDatabase.ImportAsset(path + name + extension);
    }

    public static void DeleteEnum(string path, string name)
    {
        File.Delete(path + name + extension);
    }
}
