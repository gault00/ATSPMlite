﻿using System.Collections.Generic;

namespace ATSPMWatchDog
{
    public class Application
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public List<ApplicationSettings> ApplicationSettings { get; set; }
    }
}