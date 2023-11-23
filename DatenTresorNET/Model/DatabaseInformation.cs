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

    public class DatabaseInformation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseInformation"/> class.
        /// </summary>
        public DatabaseInformation()
        {
        }

        public Guid Id { get; set; }
    }
}
