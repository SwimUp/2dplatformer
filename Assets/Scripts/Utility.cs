using System.Xml.Serialization;
using System.IO;

public static class Utility {

    /* Работа с данными */
    /* Получение(десереализация) данных из произвольного xml файла
     * @arg1 Путь до файла
     * @arg2 Тип данных
    */
    public static object DeserializeData(string Path, System.Type DataType)
    {
        XmlSerializer formatter = new XmlSerializer(DataType);

        using (FileStream fs = new FileStream(Path, FileMode.Open))
        {
            object data = formatter.Deserialize(fs);

            return data;
        }
    }
    public static object DeserializeData(string Path, System.Type DataType, FileMode Mode)
    {
        XmlSerializer formatter = new XmlSerializer(DataType);

        using (FileStream fs = new FileStream(Path, Mode))
        {
            object data = formatter.Deserialize(fs);

            return data;
        }
    }
    /* Запись(сериализация) произвольных данных в произвольный xml файл
     * @arg1 Путь до файла
     * @arg2 Тип данных
    */
    public static void SerializeData(string Path, object Data)
    {
        XmlSerializer formatter = new XmlSerializer(Data.GetType());

        using (FileStream fs = new FileStream(Path, FileMode.OpenOrCreate))
        {
            formatter.Serialize(fs, Data);
        }

    }
    public static void SerializeData(string Path, object Data, FileMode Mode)
    {
        XmlSerializer formatter = new XmlSerializer(Data.GetType());

        using (FileStream fs = new FileStream(Path, Mode))
        {
            formatter.Serialize(fs, Data);
        }

    }
}




/*
public static void SerializeData(string Path, object[] Data)
{
    XmlSerializer formatter = new XmlSerializer(Data.GetType());

    using (FileStream fs = new FileStream(Path, FileMode.OpenOrCreate))
    {
        formatter.Serialize(fs, Data);
    }

}
public static void SerializeData(string Path, object[] Data, FileMode Mode)
{
    XmlSerializer formatter = new XmlSerializer(Data.GetType());

    using (FileStream fs = new FileStream(Path, Mode))
    {
        formatter.Serialize(fs, Data);
    }

}
    public static object DeserializeDataArray(string Path)
    {
        XmlSerializer formatter = new XmlSerializer(typeof(object));

        using (FileStream fs = new FileStream(Path, FileMode.Open))
        {
            object data = formatter.Deserialize(fs);

            return data;
        }
    }
    public static object[] DeserializeDataArray(string Path, FileMode Mode)
    {
        XmlSerializer formatter = new XmlSerializer(typeof(object[]));

        using (FileStream fs = new FileStream(Path, Mode))
        {
            object[] data = (object[])formatter.Deserialize(fs);

            return data;
        }
    }
*/