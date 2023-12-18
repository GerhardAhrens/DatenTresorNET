//-----------------------------------------------------------------------
// <copyright file="DatabaseInformation.cs" company="Lifeprojects.de">
//     Class: DatabaseInformation
//     Copyright © Lifeprojects.de 2023
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>Gerhard Ahrens@Lifeprojects.de</email>
// <date>23.11.2023 21:07:26</date>
//
// <summary>
// Klasse für 
// </summary>
//-----------------------------------------------------------------------

namespace DatenTresorNET.Model
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using DatenTresorNET.BaseFunction;

    public class DatabaseInformation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseInformation"/> class.
        /// </summary>
        public DatabaseInformation()
        {
        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Password { get; set; }

        public string Description { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime ModifiedOn { get; set; }

        public string Timestamp
        {
            get
            {
                string result = string.Empty;

                using (TimeStamp ts = new TimeStamp())
                {
                    result = ts.MaxEntry(this.CreatedOn, this.CreatedBy, this.ModifiedOn, this.ModifiedBy);
                }

                return result;
            }
        }
    }
}
