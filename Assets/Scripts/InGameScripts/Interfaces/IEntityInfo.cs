﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.InGameScripts.Interfaces
{
    public interface IEntityInfo
    {
        public int Id { get; }

        public bool IsLoaded { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public float MaxHealth { get; set; }

        public float Health { get; set; }

        public float Experience { get; set; }
    }
}
