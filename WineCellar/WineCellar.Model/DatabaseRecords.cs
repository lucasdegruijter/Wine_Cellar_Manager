﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WineCellar.Model;

public record WineRecord(int Id, string Name, decimal Buy, decimal Sell, int Type, int Country, string Picture, int Year, int Content, decimal Alcohol, int Rating, string Description);
public record CountryRecord(int Id, string Name);
public record TypeRecord(int Id, string Name);
public record NoteRecord(int Id, string Name);
public record StorageLocationRecord(int WineId, string Shelf, int Row, int Col);