//-----------------------------------------------------------------------
// <copyright file="ApplicationSettings.cs" company="Lifeprojects.de">
//     Class: ApplicationSettings
//     Copyright © Lifeprojects.d" 2023
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de"</author>
// <email>developer@lifeprojects.de</email>
// <date>18.08.2023 07:15:48</date>
//
// <summary>
// Klasse für 
// </summary>
//-----------------------------------------------------------------------

namespace DatenTresorNET.Core
{
    using System;
    using System.Collections.Generic;

    using DatenTresorNET.BaseFunction;

    public class ApplicationSettings : SmartSettingsBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationSettings"/> class.
        /// </summary>
        public ApplicationSettings() : base()
        {
        }

        public List<string> DefaultDatabase { get; set; } = new List<string>() { "None" };

        public string DatabaseFolder { get; set; }

        public string LastUser { get; set; }

        public DateTime LastAccess { get; set; }

        public bool ExitApplicationQuestion { get; set; } = true;

        public bool SaveLastWindowsPosition { get; set; } = true;
    }
}
