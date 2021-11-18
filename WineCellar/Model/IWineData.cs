﻿namespace Model
{
    public class IWineData
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string Type { get; set; }
        public string OriginCountry { get; set; }
        public int Stock { get; set; }
        public string[] StorageLocation { get; set; }
        public double BuyPrice { get; set; }
        public double SellPrice { get; set; }
    }
}