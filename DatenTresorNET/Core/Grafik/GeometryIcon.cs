//-----------------------------------------------------------------------
// <copyright file="GeometryIcon.cs" company="Lifeprojects.de">
//     Class: GeometryIcon
//     Copyright © Lifeprojects.de 2023
// </copyright>
//
// <author>Gerhard Ahrens - NRM Netzdienste Rhein-Main GmbH</author>
// <email>gerhard.ahrens@lifeprojects.de</email>
// <date>07.12.2023 08:04:30</date>
//
// <summary>
// Klasse für 
// </summary>
//-----------------------------------------------------------------------

namespace DatenTresorNET.Core.Grafik
{
    using DatenTresorNET.BaseFunction;

    public class GeometryIcon : EnumerationBase
    {
        public static readonly GeometryIcon FolderOpen = new IconFolderOpen();
        public static readonly GeometryIcon FolderClose = new IconFolderClose();
        new public static readonly GeometryIcon Description = new IconDescription();
        public static readonly GeometryIcon Tag = new IconTag();
        public static readonly GeometryIcon File = new IconFile();
        public static readonly GeometryIcon Escalator = new IconEscalator();
        public static readonly GeometryIcon Rect = new IconRect();
        public static readonly GeometryIcon Monitor = new IconMonitor();
        public static readonly GeometryIcon Check = new IconCheck();

        /// <summary>
        /// Initializes a new instance of the <see cref="GeometryIcon"/> class.
        /// </summary>
        public GeometryIcon()
        {
        }

        public GeometryIcon(int value, string name, string description = null) : base(value, name, description)
        {
        }

        #region Class with Geometry Icon
        public class IconFolderOpen : GeometryIcon
        {
            public IconFolderOpen() : base(1, "M6.1,10L4,18V8H21A2,2 0 0,0 19,6H12L10,4H4A2,2 0 0,0 2,6V18A2,2 0 0,0 4,20H19C19.9,20 20.7,19.4 20.9,18.5L23.2,10H6.1M19,18H6L7.6,12H20.6L19,18Z")
            {
            }
        }

        public class IconFolderClose : GeometryIcon
        {
            public IconFolderClose() : base(2, "M20,18H4V8H20M20,6H12L10,4H4C2.89,4 2,4.89 2,6V18A2,2 0 0,0 4,20H20A2,2 0 0,0 22,18V8C22,6.89 21.1,6 20,6Z")
            {
            }
        }

        public class IconDescription : GeometryIcon
        {
            public IconDescription() : base(3, "M20,20H4A2,2 0 0,1 2,18V6A2,2 0 0,1 4,4H20A2,2 0 0,1 22,6V18A2,2 0 0,1 20,20M4,6V18H20V6H4M6,9H18V11H6V9M6,13H16V15H6V13Z")
            {
            }
        }

        public class IconTag : GeometryIcon
        {
            public IconTag() : base(4, "M21.41 11.58L12.41 2.58A2 2 0 0 0 11 2H4A2 2 0 0 0 2 4V11A2 2 0 0 0 2.59 12.42L11.59 21.42A2 2 0 0 0 13 22A2 2 0 0 0 14.41 21.41L21.41 14.41A2 2 0 0 0 22 13A2 2 0 0 0 21.41 11.58M13 20L4 11V4H11L20 13M6.5 5A1.5 1.5 0 1 1 5 6.5A1.5 1.5 0 0 1 6.5 5Z")
            {
            }
        }

        public class IconFile : GeometryIcon
        {
            public IconFile() : base(5, "M6,2A2,2 0 0,0 4,4V20A2,2 0 0,0 6,22H18A2,2 0 0,0 20,20V8L14,2H6M6,4H13V9H18V20H6V4M8,12V14H16V12H8M8,16V18H13V16H8Z")
            {
            }
        }

        public class IconEscalator : GeometryIcon
        {
            public IconEscalator() : base(6, "M20 8H18.95L6.95 20H4C2.9 20 2 19.11 2 18C2 16.9 2.9 16 4 16H5.29L7 14.29V10C7 9.45 7.45 9 8 9H9C9.55 9 10 9.45 10 10V11.29L17.29 4H20C21.11 4 22 4.89 22 6C22 7.11 21.11 8 20 8M8.5 5C9.33 5 10 5.67 10 6.5C10 7.33 9.33 8 8.5 8C7.67 8 7 7.33 7 6.5C7 5.67 7.67 5 8.5 5M20.17 15.66L14.66 21.17L12.83 19.34L18.34 13.83L16.5 12H22V17.5L20.17 15.66Z")
            {
            }
        }

        public class IconRect : GeometryIcon
        {
            public IconRect() : base(7, "M4,6V19H20V6H4M18,17H6V8H18V17Z")
            {
            }
        }

        public class IconMonitor : GeometryIcon
        {
            public IconMonitor() : base(8, "M21,16V4H3V16H21M21,2A2,2 0 0,1 23,4V16A2,2 0 0,1 21,18H14V20H16V22H8V20H10V18H3C1.89,18 1,17.1 1,16V4C1,2.89 1.89,2 3,2H21M5,6H14V11H5V6M15,6H19V8H15V6M19,9V14H15V9H19M5,12H9V14H5V12M10,12H14V14H10V12Z")
            {
            }
        }

        public class IconCheck : GeometryIcon
        {
            public IconCheck() : base(8, "M9,20.42L2.79,14.21L5.62,11.38L9,14.77L18.88,4.88L21.71,7.71L9,20.42Z")
            {
            }
        }
        #endregion Class with Right
    }
}
