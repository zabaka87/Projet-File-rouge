﻿namespace ProjetExemple.Models
{
    public class Personnage
    {
        public uint numeroPersonnage { get; set; }
        public string? nomPersonnage { get; set; }
        public byte pointsAttaquePersonnage { get; set; }
        public byte pointsDefense { get; set; }
        public uint pointsVie { get; set; }
        public uint lignePersonnage { get; set; }
        public uint colonnePersonnage { get; set; }
    }
}
