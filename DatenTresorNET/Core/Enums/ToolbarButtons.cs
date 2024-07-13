/*
 * <copyright file="ToolbarButtons.cs" company="Lifeprojects.de">
 *     Class: ToolbarButtons
 *     Copyright © Lifeprojects.de 2024
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>gerhard.ahrens@lifeprojects.de</email>
 * <date>26.04.2024</date>
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

    public enum ToolbarButtons : int
    {
        None = 0,
        ApplicationExit = 1,
        AddDatabase = 2,
        DeleteDatabase = 3,
        StartDatabase = 4,
        InfoDatabase = 5,
        AddFunctions = 6,
        ChangePassword = 7,
    }
}
