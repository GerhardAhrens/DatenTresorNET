/*
 * <copyright file="MessageQuestion.cs" company="Lifeprojects.de">
 *     Class: MessageQuestion
 *     Copyright © Lifeprojects.de 2024
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>gerhard.ahrens@lifeprojects.de</email>
 * <date>27.04.2024</date>
 * <Project>EasyPrototypingNET</Project>
 *
 * <summary>
 * Beschreibung zur Klasse
 * </summary>
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by the Free Software Foundation, 
 * either version 3 of the License, or (at your option) any later version.
 * This program is distributed in the hope that it will be useful,but WITHOUT ANY WARRANTY; 
 * without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.You should have received a copy of the GNU General Public License along with this program. 
 * If not, see <http://www.gnu.org/licenses/>.
*/

namespace DatenTresorNET.Core
{
    using System;

    public enum MessageQuestion : int
    {
        None = 0,
        Yes = 1,
        No = 2,
        Cancel = 3,
        Ok = 4,
        Delete = 5,
        Add = 6,
        NoPassword = 7,
        NoDatabase = 8,
        DatabaseInfo = 9,
    }
}
