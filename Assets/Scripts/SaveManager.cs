using Assets.Scripts.InGameScripts;
using Assets.Scripts.InGameScripts.World.Absctract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Assets.Scripts
{
    public static class SaveManager
    {
        public static void SaveLocation(WorldLocation worldLocation)
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();

            using (FileStream fs = new("Location.bs", FileMode.OpenOrCreate))
            {
                binaryFormatter.Serialize(fs, worldLocation);
            }

        }

        public static WorldLocation LoadLocation()
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();

            using (FileStream fs = new("Location.bs", FileMode.OpenOrCreate))
            {
                return (WorldLocation)binaryFormatter.Deserialize(fs);
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
            gameWorld.ClearPlayers();

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
