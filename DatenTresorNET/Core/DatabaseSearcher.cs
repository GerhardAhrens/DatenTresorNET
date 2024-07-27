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

    using DatenTresorNET.BaseFunction;
    using DatenTresorNET.Model;

    using LiteDB;

    public class DatabaseSearcher : IDisposable
    {
        private bool disposedValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseSearcher"/> class.
        /// </summary>
        public DatabaseSearcher(string databaseLocation)
        {
            this.DatabaseLocation = databaseLocation;
        }

         ~DatabaseSearcher()
        {
            this.Dispose(disposing: false);
        }

        public List<DatabaseParameter> DatabaseNamesSource { get; private set; }

        public DatabaseParameter DatabaseNameSelected { get; private set; }

        private string DatabaseLocation { get; set; }

        public async Task<bool> SearchDatabaseAsync()
        {
            return await Task.Run(() =>
            {
                return this.SearchDatabase();
            });
        }

        public bool SearchDatabase()
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

                foreach (string db in dbs)
                {
                    DatabaseParameter dbparam = new DatabaseParameter();
                    dbparam.DatabaseFolder = System.IO.Path.GetDirectoryName(db);
                    dbparam.DatabaseName = System.IO.Path.GetFileName(db);
                    dbparam.Description = System.IO.Path.GetFileNameWithoutExtension(db); ;
                    this.DatabaseNamesSource.Add(dbparam);
                }

                result = true;
            }
            else
            {
                StatusbarContent.DatabaseInfo = "Keine Datenbank vorhanden!";
            }

            return result;
        }

        #region Dispose 
        protected virtual void Dispose(bool disposing)
        {
            if (disposedValue == false)
            {
                if (disposing)
                {
                    // TODO: Verwalteten Zustand (verwaltete Objekte) bereinigen
                }

                // TODO: Nicht verwaltete Ressourcen (nicht verwaltete Objekte) freigeben und Finalizer überschreiben
                // TODO: Große Felder auf NULL setzen
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Ändern Sie diesen Code nicht. Fügen Sie Bereinigungscode in der Methode "Dispose(bool disposing)" ein.
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion Dispose
    }
}
