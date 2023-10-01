﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DofusGPS
{
    internal class Map
    {
        static int MinX { get => -93; }
        static int MaxX{ get => 49; }
        static int MinY { get => -99; }
        static int MaxY { get => 60; }
        static int Width { get => MaxX-MinX; }
        static int Height { get => MaxY-MinY; }
        int[,] Matrix { get; set; }
        public Map()
        {
            // Création de la map sous forme de tableau 2D (0 par défaut)
            Matrix = new int[Height, Width];
        }
    }
}