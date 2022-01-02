﻿using System.Windows;
using Mapsui;
using Mapsui.Utilities;
using Mapsui.Layers;
using Mapsui.Providers;
using System.Collections.Generic;
using Mapsui.Projection;
using Mapsui.Styles;
using System.IO;
using System.Reflection;
using System.Windows.Controls;
using Mapsui.UI.Wpf;
using System.Diagnostics;

namespace WineCellar;

public partial class GeographicView : Window
{
    private struct Cords
    {
        public int id;
        public double x, y;
    }

    private List<Feature> points = new List<Feature>();
    private List<Cords> cords = new List<Cords>();
    private bool wineClicked = false;
    private double clickBoundry = 0.05;

    public GeographicView()
    {
        InitializeComponent();

        AddPoints();                        // AddPoints first!!
        MyMapControl.Map = CreateMap();
    }

    public Map CreateMap()
    {
        var map = new Map();

        map.Layers.Add(OpenStreetMap.CreateTileLayer());
        map.Layers.Add(CreatePointLayer(points));

        return map;
    }

    public void AddPoints()
    {
        // dummy data
        points.Add(CreatePoint(6.079559, 52.500767, 3));
        points.Add(CreatePoint(4.4210539, 44.0171384, 4));
        points.Add(CreatePoint(0.4328022, 44.7959213, 8));
        points.Add(CreatePoint(2.1774322, 41.3828939, 6));
        points.Add(CreatePoint(-96.3574856, 19.4766160, 7));
    }

    private MemoryLayer CreatePointLayer(List<Feature> points)
    {
        MemoryProvider memoryProvider = new MemoryProvider(points);

        return new MemoryLayer
        {
            Name = "Points",
            DataSource = memoryProvider,
            IsMapInfoLayer = true,
            Style = null
        };
    }

    private SymbolStyle PointStyle()
    {
        SymbolStyle pointStyle = new SymbolStyle()
        {
            //BitmapId = GetBitmapIdForEmbeddedResource("pointer.jpg"),
            SymbolScale = 0.3,
            Fill = new Brush(Color.FromString("Red"))
        };

        return pointStyle;
    }

    private int GetBitmapIdForEmbeddedResource(string imagePath)
    {
        var assembly = typeof(GeographicView).GetTypeInfo().Assembly;
        var image = assembly.GetManifestResourceStream(imagePath);
        return BitmapRegistry.Instance.Register(image);
    }

    private Feature CreatePoint(double longitude, double latitude, int id)
    {
        var feature = new Feature();
        var point = SphericalMercator.FromLonLat(longitude, latitude);

        feature.Styles.Add(PointStyle());
        feature.Geometry = point;

        var pCords = new Cords();
        pCords.id = id;
        pCords.x = longitude; 
        pCords.y = latitude;

        cords.Add(pCords);

        return feature;
    }

    void GeographicView_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
        if (!wineClicked)
        {

            MainWindow window = new MainWindow();
            Application.Current.MainWindow = window;
            window.Show();
        }
    }

    private void MyMapControl_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        var screenPosition = e.GetPosition(MyMapControl);
        var worldPosition = MyMapControl.Viewport.ScreenToWorld(screenPosition.X, screenPosition.Y);
        var pointPosition = SphericalMercator.ToLonLat(worldPosition.X, worldPosition.Y);

        foreach (var item in cords)
        {
            if ((pointPosition.X > (item.x - clickBoundry) && pointPosition.X < (item.x + clickBoundry)) && (pointPosition.Y > (item.y - clickBoundry) && pointPosition.Y < (item.y + clickBoundry)))
            {
                wineClicked = true;

                DetailedView detailedView = new DetailedView(item.id);
                Application.Current.MainWindow = detailedView;
                detailedView.Show();
                Close();
            }
        }
    }
}