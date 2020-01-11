﻿using Fantome.Libraries.League.IO.MapGeometry;
using HelixToolkit.Wpf.SharpDX;
using HelixToolkit.Wpf.SharpDX.Model.Scene;
using Imaging.DDSReader;
using Microsoft.Win32;
using Ripple.BIN;
using Ripple.Content;
using SharpDX;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;

using mPoint3D = System.Windows.Media.Media3D.Point3D;
using mVector3D = System.Windows.Media.Media3D.Vector3D;

namespace Ripple
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public SceneNodeGroupModel3D MainModelGroup 
        {
            get => this._mainModelGroup;
            set
            {
                this._mainModelGroup = value;
                NotifyPropertyChanged();
            }
        }
        public DefaultEffectsManager EffectsManager { get; set; }
        public PerspectiveCamera Camera { get; set; }

        private SceneNodeGroupModel3D _mainModelGroup;
        
        public MGEOFile MapGeometry { get; private set; }
        public MapData MapData { get; private set; }
        public Dictionary<uint, object> MapBIN { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public MainWindow()
        {
            InitializeComponent();

            this.EffectsManager = new DefaultEffectsManager();
            this.Camera = CreateCamera();
            this.Viewport.DataContext = this;

            this.MapBIN = ContentLoader.LoadMapBIN(@"C:\Users\Crauzer\Desktop\New folder\data\Maps\MapGeometry\SR\Base_SRX.materials.bin");
            this.MapData = new MapData(this.MapBIN);
        }
            

        private void MenuItemOpen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog()
            {
                Multiselect = false,
                Filter = "Map Geometry files (*.mapgeo)|*.mapgeo"
            };

            if (dialog.ShowDialog() == true)
            {
                this.MapGeometry = new MGEOFile(dialog.FileName);
                this.MainModelGroup = ContentLoader.LoadMapMGEO(this.MapGeometry);
            }
        }

        private PerspectiveCamera CreateCamera()
        {
            return new PerspectiveCamera()
            {
                Position = new mPoint3D(0, 0, 5),
                LookDirection = new mVector3D(-0, -0, -5),
                UpDirection = new mVector3D(0, 1, 0),
                NearPlaneDistance = 0.5,
                FarPlaneDistance = 10000000
            };
        }

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    } 
}

