using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using System.Collections.ObjectModel;

namespace CLWD.Helpher
{
    public static class ObjectCopier
    {
        //private static ObservableCollection<T> DeepCopy<T>(ObservableCollection<T> list)
        //where T : ICloneable
        //{
        //    ObservableCollection<T> newList = new ObservableCollection<T>();
        //    foreach (T rec in list)
        //    {
        //        newList.Add((T)rec.Clone());
        //    }
        //    return newList;
        //}


        /// <summary>
        /// Perform a deep Copy of the object.
        /// </summary>
        /// <typeparam name="T">The type of object being copied.</typeparam>
        /// <param name="source">The object instance to copy.</param>
        /// <returns>The copied object.</returns>
        public static T Clone<T>(T source)
        {
            if (!typeof(T).IsSerializable)
            {
                throw new ArgumentException("The type must be serializable.", "source");
            }

            // Don't serialize a null object, simply return the default for that object
            if (Object.ReferenceEquals(source, null))
            {
                return default(T);
            }

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new MemoryStream();
            using (stream)
            {
                formatter.Serialize(stream, source);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }
        }
    }    
}
