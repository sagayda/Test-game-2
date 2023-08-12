using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Assets.Scripts.Model.WorldGeneration
{
    public class WorldGeneratorParametersFactory
    {
        const string DEFAULT_PARAMETERS_PATH = @"Parameters\";
        const string DEFAULT_GENERATOR_PARAMETERS_PATH = @"Parameters\GeneratorParameters.bs";

        private readonly IFormatter _formatter = new BinaryFormatter();

        public GeneratorParameters Default => LoadDefault();

        public GeneratorParameters LoadDefault()
        {
            using FileStream fileStream = new(DEFAULT_GENERATOR_PARAMETERS_PATH, FileMode.OpenOrCreate);

            return (GeneratorParameters)_formatter.Deserialize(fileStream);

        }

        public void SaveAsDefault(GeneratorParameters parameters)
        {
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters), "Parameters is null!");

            using FileStream fileStream = new(DEFAULT_GENERATOR_PARAMETERS_PATH, FileMode.OpenOrCreate);

            _formatter.Serialize(fileStream, parameters);
        }

        public void SaveNoiseParameters<T>(T parameters) where T : ISavableGeneratorParameter
        {
            string path = $@"{DEFAULT_PARAMETERS_PATH}{typeof(T).Name}.bs";

            using FileStream fileStream = new(path, FileMode.OpenOrCreate, FileAccess.Write);

            _formatter.Serialize(fileStream, parameters);
        }

        public T LoadNoiseParameters<T>() where T : ISavableGeneratorParameter
        {
            string path = $@"{DEFAULT_PARAMETERS_PATH}{typeof(T).Name}.bs";

            using FileStream fileStream = new(path, FileMode.OpenOrCreate);

            return (T)_formatter.Deserialize(fileStream);
        }
    }
}
