//-----------------------------------------------------------------------
// <copyright file="DatabaseSearcher.cs" company="Lifeprojects.de">
//     Class: DatabaseSearcher
//     Copyright © Lifeprojects.de 2024
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>Gerhard Ahrens@Lifeprojects.de</email>
// <date>12.02.2024 15:23:17</date>
//
// <summary>
// Klasse für 
// </summary>
//-----------------------------------------------------------------------

namespace DatenTresorNET.Core
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    public class DatabaseSearcher
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseSearcher"/> class.
        /// </summary>
        public DatabaseSearcher(string databaseLocation)
        {
            this.DatabaseLocation = databaseLocation;
        }

        public List<DatabaseParameter> DatabaseNamesSource { get; set; }

        private string DatabaseLocation { get; set; }

        public async Task<bool> SearchDatabaseAsync()
        {
            return await Task.Run(() =>
            {
                return this.SearchDatabase();
            });
        }

        private bool SearchDatabase()
        {
            bool result = false;

            if (Directory.Exists(this.DatabaseLocation) == false)
            {
                Directory.CreateDirectory(this.DatabaseLocation);
            }

            var dbs = Directory.EnumerateFiles(this.DatabaseLocation, "*.db", SearchOption.TopDirectoryOnly);
            if (dbs.Any() == true)
            {
                if (this.DatabaseNamesSource == null)
                {
                    this.DatabaseNamesSource = new List<DatabaseParameter>();
                }

            }

            return result;
        }
    }
}
