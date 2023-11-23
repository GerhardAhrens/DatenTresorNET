
//-----------------------------------------------------------------------
// <copyright file="DatabaseManager.cs" company="Lifeprojects.de">
//     Class: DatabaseManager
//     Copyright © Lifeprojects.de 2022
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>gerhard.ahrens@lifeprojects.de</email>
// <date>01.07.2022</date>
//
// <summary>
// Klasse für 
// </summary>
//-----------------------------------------------------------------------

namespace Solarertrag.DataRepository
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.Versioning;

    using DatenTresorNET.BaseFunction.Pattern;
    using DatenTresorNET.Model;

    using LiteDB;

    [SupportedOSPlatform("windows")]
    public sealed class DatabaseManager : DisposableBase
    {
        public DatabaseManager(string databaseFile)
        {
            this.DatabaseFile = databaseFile;
        }

        private string DatabaseFile { get; set; }

        private ConnectionString ConnectionDB { get; set; }

        private LiteDatabase DatabaseIntern { get; set; }

        private ILiteCollection<DatabaseInformation> DatabaseInformationCollection { get; set; }

        public Result<bool> ExistDatabase()
        {
            bool result = false;
            long ticks = 0;

            try
            {
                using (ObjectRuntime objectRuntime = new ObjectRuntime())
                {
                    FileInfo fileInfoe = new FileInfo(this.DatabaseFile);
                    if (fileInfoe.Exists == true)
                    {
                        result = true;
                    }

                    ticks = objectRuntime.ResultMilliseconds();
                }
            }
            catch (Exception ex)
            {
                return Result<bool>.FailureResult(ex);
            }

            return Result<bool>.SuccessResult(result, true, ticks);
        }

        public Result<bool> CreateNewDatabase()
        {
            bool result = false;
            long ticks = 0;

            try
            {
                using (ObjectRuntime objectRuntime = new ObjectRuntime())
                {
                    this.ConnectionDB = this.Connection(this.DatabaseFile);

                    this.DatabaseIntern = new LiteDatabase(this.ConnectionDB);
                    this.DatabaseIntern.UserVersion = 3;
                    if (this.DatabaseIntern != null)
                    {
                        this.DatabaseInformationCollection = this.DatabaseIntern.GetCollection<DatabaseInformation>(nameof(DatabaseInformation));

                        DatabaseInformation workUserInfo = new DatabaseInformation();
                        workUserInfo.Id = Guid.NewGuid();
                        this.DatabaseInformationCollection.Insert(workUserInfo);
                    }

                    ticks = objectRuntime.ResultMilliseconds();
                }

                result = true;
            }
            catch (Exception ex)
            {
                return Result<bool>.FailureResult(ex);
            }

            return Result<bool>.SuccessResult(result, true, ticks);
        }

        public Result<bool> OpenDatabase()
        {
            bool result = false;
            long ticks = 0;

            try
            {
                using (ObjectRuntime objectRuntime = new ObjectRuntime())
                {
                    this.ConnectionDB = this.Connection(this.DatabaseFile);

                    this.DatabaseIntern = new LiteDatabase(this.ConnectionDB);
                    if (this.DatabaseIntern != null)
                    {
                        this.DatabaseInformationCollection = this.DatabaseIntern.GetCollection<DatabaseInformation>(nameof(DatabaseInformation));

                        IEnumerable<DatabaseInformation> effortProjectInfo = this.DatabaseInformationCollection.FindAll().ToList();
                     }

                    ticks = objectRuntime.ResultMilliseconds();
                }

                result = true;
            }
            catch (Exception ex)
            {
                return Result<bool>.FailureResult(ex);
            }

            return Result<bool>.SuccessResult(result, true, ticks);
        }

        public Result<bool> Close()
        {
            bool result = false;

            try
            {
                Result<bool> existResult = this.ExistDatabase();
                if (existResult != null && existResult.Success == true)
                {
                    this.DatabaseInformationCollection = null;

                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }

                result = true;

                Result<bool>.SuccessResult(result, true);
            }
            catch (Exception ex)
            {
                return Result<bool>.FailureResult(ex);
            }

            return Result<bool>.SuccessResult(result, false);
        }

        public Result<bool> Delete(string databaseFile)
        {
            bool result = false;
            try
            {
                result = true;

                Result<bool>.SuccessResult(result, true);
            }
            catch (Exception ex)
            {
                return Result<bool>.FailureResult(ex);
            }

            return Result<bool>.SuccessResult(result, false);
        }

        public override void DisposeManagedResources()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        private ConnectionString Connection(string databaseFile)
        {
            ConnectionString conn = new ConnectionString(databaseFile);
            conn.Connection = ConnectionType.Shared;

            return conn;
        }
    }
}
