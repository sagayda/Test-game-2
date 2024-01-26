using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Assets.Scripts.Model.InGameScripts;
using WorldGeneration.Core.Locations;

namespace Resources
{
    public static class SaveManager
    {
        public static void SaveLocation(Location worldLocation)
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();

            using (FileStream fs = new("Location.bs", FileMode.OpenOrCreate))
            {
                binaryFormatter.Serialize(fs, worldLocation);
            }

        }

        public static Location LoadLocation()
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();

            using (FileStream fs = new("Location.bs", FileMode.OpenOrCreate))
            {
                return (Location)binaryFormatter.Deserialize(fs);
            }

        }

        public static void SavePlayer(Player player)
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();

            using (FileStream fs = new("Player.bs", FileMode.OpenOrCreate))
            {
                binaryFormatter.Serialize(fs, player);
            }

        }

        public static Player LoadPlayer()
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();

            using (FileStream fs = new("Player.bs", FileMode.OpenOrCreate))
            {
                return (Player)binaryFormatter.Deserialize(fs);
            }

        }

        public static void SaveGameWorld(GameWorld gameWorld)
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();

            using (FileStream fs = new("World.bs", FileMode.OpenOrCreate))
            {
                binaryFormatter.Serialize(fs, gameWorld);
            }
        }

        public static GameWorld LoadGameWorld()
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();

            using (FileStream fs = new("World.bs", FileMode.OpenOrCreate))
            {
                return (GameWorld)binaryFormatter.Deserialize(fs);
            }
        }
    }
}
