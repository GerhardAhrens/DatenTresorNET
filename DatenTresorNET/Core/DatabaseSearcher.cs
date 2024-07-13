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
        public DatabaseSearcher(string databaseLocation, string password)
        {
            this.DatabaseLocation = databaseLocation;
            this.Passwort = password;
        }

         ~DatabaseSearcher()
        {
            this.Dispose(disposing: false);
        }

        public List<DatabaseParameter> DatabaseNamesSource { get; private set; }

        public DatabaseParameter DatabaseNameSelected { get; private set; }

        private string DatabaseLocation { get; set; }

        public string Passwort { get; private set; }

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
                    ConnectionString dbconn = null;
                    using (DBConnectionBuilder builder = new DBConnectionBuilder())
                    {
                        dbconn = builder.GetConnection(db, this.Passwort);
                    }

                    LiteDatabase litedb = new LiteDatabase(dbconn);
                    ILiteCollection<DatabaseInformation> databaseInformationCollection = litedb.GetCollection<DatabaseInformation>(typeof(DatabaseInformation).Name);
                    DatabaseInformation dbinfo = databaseInformationCollection.FindAll().First();

                    DatabaseParameter dbparam = new DatabaseParameter();
                    if (dbs.Count() == 1)
                    {
                        dbparam.Default = true;
                    }
                    else
                    {
                        dbparam.Default = false;
                    }

                    dbparam.DatabaseFolder = System.IO.Path.GetDirectoryName(db);
                    dbparam.DatabaseName = System.IO.Path.GetFileName(db);
                    dbparam.Description = dbinfo.Description;
                    dbparam.PasswordHash = this.Passwort;
                    this.DatabaseNamesSource.Add(dbparam);
                    litedb.Dispose();
                    litedb = null;
                }

                using (ApplicationSettings settings = new ApplicationSettings())
                {
                    if (settings.IsExitSettings() == true)
                    {
                        settings.Load();
                    }

                    if (settings.Databases?.Count() != dbs.Count())
                    {
                        if (settings.Databases == null)
                        {
                            settings.Databases = new List<DatabaseParameter>();
                        }

                        settings.Databases?.Clear();
                        if (this.DatabaseNamesSource != null)
                        {
                            foreach (DatabaseParameter item in this.DatabaseNamesSource)
                            {
                                DatabaseParameter dp = new DatabaseParameter();
                                dp.DatabaseName = item.DatabaseName;
                                dp.DatabaseFolder = this.DatabaseLocation;
                                dp.Description = item.Description;
                                dp.PasswordHash = item.PasswordHash;
                                settings.Databases.Add(dp);
                            }

                            settings.Save();
                        }
                    }

                    foreach (DatabaseParameter item in this.DatabaseNamesSource)
                    {
                        if (settings.Databases.Count() == 1)
                        {
                            item.Default = true;
                            break;
                        }
                    }

                    this.DatabaseNamesSource = this.DatabaseNamesSource.OrderBy(x => x.Default).ToList();
                    if (this.DatabaseNamesSource.FirstOrDefault(f => f.Default == true) != null)
                    {
                        this.DatabaseNameSelected = this.DatabaseNamesSource.FirstOrDefault(f => f.Default == true);
                    }
                    else
                    {
                        this.DatabaseNameSelected = this.DatabaseNamesSource.FirstOrDefault();
                    }
                }

                result = true;
            }
            else
            {
                StatusbarContent.DatabaseInfo = "Keine Datenbank vorhanden!";
                Task.Delay(5000);
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
