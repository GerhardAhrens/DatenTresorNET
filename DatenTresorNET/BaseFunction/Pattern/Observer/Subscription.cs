/*
 * <copyright file="Subscription.cs" company="Lifeprojects.de">
 *     Class: Subscription
 *     Copyright © Lifeprojects.de 2022
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>developer@lifeprojects.de</email>
 * <date>15.10.2022</date>
 * <Project>EasyPrototypingNET</Project>
 *
 * <summary>
 * Klasse zur Ausführung einer Benachrichtigung die über den Publish angestoßen wurde
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

namespace DatenTresorNET.BaseFunction.Pattern
{
    using System;

    public class Subscription<TMessage> : ISubscription<TMessage> where TMessage : IPayload
    {
        public Subscription(IEventAggregator eventAggregator, Action<TMessage> action)
        {
            if (eventAggregator == null)
            {
                throw new ArgumentNullException("eventAggregator");
            }

            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            this.EventAggregator = eventAggregator;
            this.Action = action;
        }

        public Action<TMessage> Action { get; private set; }

        public IEventAggregator EventAggregator { get; private set; }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.EventAggregator.UnSubscribe(this);
            }
        }
    }
}