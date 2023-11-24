
//-----------------------------------------------------------------------
// <copyright file="RepositoryBase.cs" company="Lifeprojects.de">
//     Class: RepositoryBase
//     Copyright © Lifeprojects.de 2022
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>gerhard.ahrens@lifeprojects.de</email>
// <date>27.06.2022</date>
//
// <summary>
// Basisklasse zur Erstellung von Repository Klassen
// </summary>
//-----------------------------------------------------------------------

namespace DatenTresorNET.DataRepository
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Windows;
    using DatenTresorNET.Core;

    using LiteDB;

    public abstract class RepositoryBase<TEntity> : IDisposable
    {
        private bool classIsDisposed = false;
        private string password = null;

        protected RepositoryBase(DatabaseParameter database)
        {
            database.IsArgumentNull("database");

            if (string.IsNullOrEmpty(database.PasswordHash) == true)
            {
                this.password = string.Empty;
            }
            else
            {
                this.password = database.PasswordHash;
            }

            string databaseFile = Path.Combine(database.DatabaseFolder,database.DatabaseName);
            this.ConnectionDB = this.Connection(databaseFile, this.password);

            string collectionName = typeof(TEntity).Name;

            try
            {
                this.DatabaseIntern = new LiteDatabase(this.ConnectionDB);
                this.DatabaseIntern.UserVersion = 3;
                if (this.DatabaseIntern != null)
                {
                    this.CollectionIntern = this.DatabaseIntern.GetCollection<TEntity>(collectionName);
                    if (this.CollectionIntern != null)
                    {
                        this.IsDatabaseOpen = true;
                        this.DatabaseName = databaseFile;
                    }
                }
            }
            catch (IOException ex) when (ex.HResult == -2147024864)
            {
                throw new Exception(ex.Message);
            }
            catch (LiteException ex) when (ex.Message == "Invalid password")
            {
                MessageBox.Show($"Falsches Passwort zum öffnenen der Datenbank.\n{databaseFile}", "Simple Password Manager");
                Environment.Exit(-1);
            }
            catch (LiteException ex) when (ex.Message == "This file is not encrypted")
            {
                MessageBox.Show($"Datenbank ist nicht verschlüsselt.\n{databaseFile}", "Simple Password Manager");
                Environment.Exit(-1);
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}", ex);
            }
        }

        public ConnectionString ConnectionDB { get; set; }

        public string DatabaseName { get; private set; }

        public LiteDatabase DatabaseIntern { get; private set; }

        public ILiteCollection<TEntity> CollectionIntern { get; private set; }

        public bool IsDatabaseOpen { get; private set; } = false;

        public virtual int Count()
        {
            int result = 0;

            try
            {
                if (this.CollectionIntern != null)
                {
                    result = this.CollectionIntern.Count();
                }
            }
            catch (Exception ex)
            {
                string errorText = ex.Message;
                throw;
            }

            return result;
        }

        public virtual int Count(Expression<Func<TEntity, bool>> predicate)
        {
            int result = 0;

            try
            {
                if (this.CollectionIntern != null)
                {
                    result = this.CollectionIntern.Count(predicate);
                }
            }
            catch (Exception ex)
            {
                string errorText = ex.Message;
                throw;
            }

            return result;
        }

        public virtual void Add(TEntity entity)
        {
            BsonValue result = null;

            try
            {
                if (this.CollectionIntern != null && entity != null)
                {
                    result = this.CollectionIntern.Insert(entity);
                }
            }
            catch (Exception ex)
            {
                string errorText = ex.Message;
                throw;
            }
        }

        public virtual bool Update(TEntity entity)
        {
            bool result = false;

            try
            {
                if (this.CollectionIntern != null && entity != null)
                {
                    result = this.CollectionIntern.Update(entity);
                }
            }
            catch (Exception ex)
            {
                string errorText = ex.Message;
                throw;
            }

            return result;
        }

        public virtual int DeleteAll()
        {
            int result = 0;
            long rebuildResult = 0;

            try
            {
                if (this.CollectionIntern != null)
                {
                    result = this.CollectionIntern.DeleteAll();
                    rebuildResult = this.DatabaseIntern.Rebuild();
                }
            }
            catch (Exception ex)
            {
                string errorText = ex.Message;
                throw;
            }

            return result;
        }
        public virtual bool Delete(Guid id)
        {
            bool result = false;

            try
            {
                if (this.CollectionIntern != null && id != Guid.Empty)
                {
                    result = this.CollectionIntern.Delete(id);
                }
            }
            catch (Exception ex)
            {
                string errorText = ex.Message;
                throw;
            }

            return result;
        }

        public virtual int DeleteMany(Expression<Func<TEntity, bool>> predicate)
        {
            int result = 0;

            try
            {
                if (this.CollectionIntern != null)
                {
                    result = this.CollectionIntern.DeleteMany(predicate);
                }
            }
            catch (Exception ex)
            {
                string errorText = ex.Message;
                throw;
            }

            return result;
        }

        public virtual TEntity GetById(Guid id)
        {
            TEntity result = default(TEntity);

            try
            {
                if (this.CollectionIntern != null)
                {
                    result = this.CollectionIntern.FindById(id);
                }
            }
            catch (Exception ex)
            {
                string errorText = ex.Message;
                throw;
            }

            return result;
        }

        public virtual IEnumerable<TEntity> List(Expression<Func<TEntity, bool>> predicate)
        {
            IEnumerable<TEntity> result = null;

            try
            {
                if (this.CollectionIntern != null)
                {
                    result = this.CollectionIntern.Find(predicate);
                }
            }
            catch (Exception ex)
            {
                string errorText = ex.Message;
                throw;
            }

            return result;
        }

        public virtual IEnumerable<TEntity> List()
        {
            IEnumerable<TEntity> result = null;

            try
            {
                if (this.CollectionIntern != null)
                {
                    result = this.CollectionIntern.FindAll();
                }
            }
            catch (Exception ex)
            {
                string errorText = ex.Message;
                throw;
            }

            return result;
        }

        public virtual bool Exist(BsonExpression predicate)
        {
            bool result = false;

            try
            {
                if (this.CollectionIntern != null)
                {
                    result = this.CollectionIntern.Exists(predicate);
                }
            }
            catch (Exception ex)
            {
                string errorText = ex.Message;
                throw;
            }

            return result;
        }

        public virtual bool Exist(Expression<Func<TEntity, bool>> predicate)
        {
            bool result = false;

            try
            {
                if (this.CollectionIntern != null)
                {
                    result = this.CollectionIntern.Exists(predicate);
                }
            }
            catch (Exception ex)
            {
                string errorText = ex.Message;
                throw;
            }

            return result;
        }

        #region Implement Dispose
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool classDisposing = false)
        {
            if (this.classIsDisposed == false)
            {
                if (classDisposing == true)
                {
                    this.IsDatabaseOpen = false;
                }
            }

            this.classIsDisposed = true;
        }
        #endregion Implement Dispose

        private ConnectionString Connection(string databaseFile)
        {
            return this.Connection(databaseFile, string.Empty);
        }

        private ConnectionString Connection(string databaseFile, string password)
        {
            ConnectionString conn = new ConnectionString(databaseFile);
            conn.Connection = ConnectionType.Shared;
            conn.Password = password;

            return conn;
        }
    }
}
