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
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;

    [DebuggerDisplay("DatabaseName:{this.DatabaseName}; Default: {this.Default}")]
    public class DatabaseParameter : IEqualityComparer<DatabaseParameter>
    {
        public bool Default { get; set; }

        public string DatabaseName { get; set; }

        public string DatabaseFolder { get; set; }

        public string Fullname { get { return Path.Combine(this.DatabaseFolder,this.DatabaseName); } }

        public string Description { get; set; }

        public string PasswordHash { get; set; }

        public bool Equals(DatabaseParameter x, DatabaseParameter y)
        {
            return x.DatabaseName == y.DatabaseName;
        }

        public int GetHashCode([DisallowNull] DatabaseParameter obj)
        {
            return obj.DatabaseName.GetHashCode();
        }
    }
}
