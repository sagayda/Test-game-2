using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace WorldGeneration.Core
{
    public static class ParametersSave
    {
        private const string PARAMETERS_PATH = @"Assets\Resources\";
        private const string DEFAULT_GENERATOR_PARAMETERS_PATH = @"Assets\Resources\GeneratorParameters.bs";

        private static readonly IFormatter _formatter = new BinaryFormatter();

        public static GeneratorParameters Default => LoadDefault();

        public static GeneratorParameters LoadDefault()
        {
            using FileStream fileStream = new(DEFAULT_GENERATOR_PARAMETERS_PATH, FileMode.OpenOrCreate);

            return (GeneratorParameters)_formatter.Deserialize(fileStream);

        }

        public static void SaveAsDefault(GeneratorParameters parameters)
        {
            //if (parameters == null)
            //    throw new ArgumentNullException(nameof(parameters), "Parameters is null!");

            using FileStream fileStream = new(DEFAULT_GENERATOR_PARAMETERS_PATH, FileMode.OpenOrCreate);

            _formatter.Serialize(fileStream, parameters);
        }

        public static void SaveParameters<T>(T parameters, SaveSlot slot) where T : struct, IParameters
        {
            string path = GetPath(typeof(T), slot);

            using FileStream fileStream = new(path, FileMode.OpenOrCreate, FileAccess.Write);

            _formatter.Serialize(fileStream, parameters);
        }

        public static T? LoadParameters<T>(SaveSlot slot) where T : struct, IParameters
        {
            string path = GetPath(typeof(T), slot);

            try
            {
                using FileStream fileStream = new(path, FileMode.OpenOrCreate);

                return _formatter.Deserialize(fileStream) as T?;
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogError(e.Message);
                return null;
            }
        }

        private static string GetPath(Type type, SaveSlot slot)
        {
            return $"{PARAMETERS_PATH}{type.Name}-{slot}.params";
        }

        public enum SaveSlot
        {
            Default,
            Test,
            One,
            Two,
            Three,
        }
    }

}
