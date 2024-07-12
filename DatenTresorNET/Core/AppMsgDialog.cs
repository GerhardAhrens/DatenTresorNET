/*
 * <copyright file="AppMsgDialog.cs" company="Lifeprojects.de">
 *     Class: AppMsgDialog
 *     Copyright © Lifeprojects.de 2024
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>gerhard.ahrens@lifeprojects.de</email>
 * <date>12.07.2024 17:19:43</date>
 * <Project>CurrentProject</Project>
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

    using DatenTresorNET.BaseFunction;
    using DatenTresorNET.Controls;

    public static class AppMsgDialog
    {
        public static NotificationBoxButton HinweisOk(this INotificationService @this)
        {
            bool? resultDialog = null;

            Tuple<string, string, double> msgText = new Tuple<string, string, double>("Datenbankinformation", $"Eingabe korrigieren", 18);
            Tuple<NotificationBoxButton, object> resultTag = new Tuple<NotificationBoxButton, object>(NotificationBoxButton.None, null);

            @this.ShowDialog<MessageOK>(msgText, (result, tag) =>
            {
                resultDialog = result;
                if (tag != null)
                {
                    resultTag = (Tuple<NotificationBoxButton, object>)tag;
                }
            });

            return resultTag.Item1;
        }
    }
}
