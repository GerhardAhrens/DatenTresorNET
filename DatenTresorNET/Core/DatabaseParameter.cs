//-----------------------------------------------------------------------
// <copyright file="DatabaseParameter.cs" company="Lifeprojects.de">
//     Class: DatabaseParameter
//     Copyright © Lifeprojects.de 2023
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>Gerhard Ahrens@Lifeprojects.de</email>
// <date>23.11.2023 19:59:09</date>
//
// <summary>
// Klasse für 
// </summary>
//-----------------------------------------------------------------------

namespace DatenTresorNET.Core
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class DatabaseParameter
    {
        public bool Default { get; set; }

        public string DatabaseName { get; set; }

        public string DatabaseFolder { get; set; }

        public string Description { get; set; }
    }
}
