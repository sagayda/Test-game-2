﻿using Assets.Scripts.InGameScripts.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.InGameScripts
{
    public class EntityInfo : IEntityInfo
    {
        public int Id { get; }

        public bool IsLoaded { get; }

        public string Name { get; }

        public string Description { get; }

        public uint Health { get; }

        public uint Experience { get; }

        public EntityInfo(int id, string name, string description, uint health, uint experience, bool isLoaded = false)
        {
            Id = id;
            IsLoaded = isLoaded;
            Name = name;
            Description = description;
            Health = health;
            Experience = experience;
        }
    }
}
