﻿using ZoneRpg.Database;
using ZoneRpg.Shared;

namespace ZoneRpg.Ui;
public class UiManager
{
    private DatabaseManager _db;

    public UiManager(DatabaseManager db)
    {
        _db = db;
    }
    public void Draw(Zone zone)
    {
        Console.WriteLine();
        Console.WriteLine("                Zone: "+zone.Name);
        for (int i = 0; i < zone.Width; i++)
        {
            Console.Write("-");
        }
        Console.WriteLine("");

        for (int i = 0; i < zone.Height+1; i++)
        {
            Console.Write("|");
            for (int j = 0; j < zone.Width-2; j++)
            {
                Console.Write(" ");
            }
            Console.WriteLine("|");
        }
        for (int i = 0; i < zone.Width; i++)
        {
            Console.Write("-");

        }
    }
    public void DrawEntity(Zone zone)
    {
        foreach (var item in zone.entities)
        {
            Console.SetCursorPosition(item.X,item.Y);
            Console.WriteLine(item.Symbol);
        }
    }
}
