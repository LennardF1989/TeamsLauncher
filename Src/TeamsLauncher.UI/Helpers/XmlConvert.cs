using System.IO;
using System.Xml.Serialization;

namespace TeamsLauncher.UI.Helpers
{
    public static class XmlConvert
    {
        public static void SerializeObject<T>(string filePath, T objectToWrite, bool append = false) where T : new()
        {
            TextWriter writer = null;

            try
            {
                var serializer = new XmlSerializer(typeof(T));
                writer = new StreamWriter(filePath, append);
                serializer.Serialize(writer, objectToWrite);
            }
            catch
            {
                //Do nothing
            }
            finally
            {
                writer?.Close();
            }
        }

        public static T DeserializeObject<T>(string filePath) where T : new()
        {
            TextReader reader = null;

            try
            {
                var serializer = new XmlSerializer(typeof(T));
                reader = new StreamReader(filePath);
                return (T)serializer.Deserialize(reader);
            }
            catch
            {
                return default;
            }
            finally
            {
                reader?.Close();
            }
        }
    }
}
