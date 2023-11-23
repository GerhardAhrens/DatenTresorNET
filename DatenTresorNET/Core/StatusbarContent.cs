//-----------------------------------------------------------------------
// <copyright file="StatusbarContent.cs" company="Lifeprojects.de">
//     Class: StatusbarContent
//     Copyright © Lifeprojects.de" 2023
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de"</author>
// <email>developer@lifeprojects.de</email>
// <date>18.08.2023 07:03:18</date>
//
// <summary>
// Klasse für 
// </summary>
//-----------------------------------------------------------------------

namespace DatenTresorNET.Core
{
    using System;

    public class StatusbarContent
    {
        private static string notification = string.Empty;
        private static string databaseInfo = string.Empty;
        private static string defaultDatabase = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="StatusbarContent"/> class.
        /// </summary>
        static StatusbarContent()
        {
            User = $"{Environment.UserDomainName}/{Environment.UserName}";
            CurrentDate = $"{DateTime.Now.ToShortDateString()}";
            NotificationChanged += (sender, e) => { return; };
            DatabaseInfoChanged += (sender, e) => { return; };
            DefaultDatabaseChanged += (sender, e) => { return; };
        }

        public static string User { get; }

        public static string CurrentDate { get; set; }

        public static string CurrentClient { get; set; }

        public static string Notification
        {
            get { return notification; }
            set
            {
                notification = value;
                OnNotificationChanged(EventArgs.Empty);
            }
        }

        public static string DatabaseInfo
        {
            get { return databaseInfo; }
            set
            {
                databaseInfo = value;
                OnDatabaseInfoChanged(EventArgs.Empty);
            }
        }

        public static string DefaultDatabase
        {
            get { return defaultDatabase; }
            set
            {
                defaultDatabase = value;
                OnDefaultDatabaseChanged(EventArgs.Empty);
            }
        }
        // Declare a static event representing changes to your static property
        public static event EventHandler NotificationChanged;
        public static event EventHandler DatabaseInfoChanged;
        public static event EventHandler DefaultDatabaseChanged;

        // Raise the change event through this static method
        protected static void OnNotificationChanged(EventArgs e)
        {
            EventHandler handler = NotificationChanged;

            if (handler != null)
            {
                handler(null, e);
            }
        }

        protected static void OnDatabaseInfoChanged(EventArgs e)
        {
            EventHandler handler = DatabaseInfoChanged;

            if (handler != null)
            {
                handler(null, e);
            }
        }

        protected static void OnDefaultDatabaseChanged(EventArgs e)
        {
            EventHandler handler = DefaultDatabaseChanged;

            if (handler != null)
            {
                handler(null, e);
            }
        }
    }
}
