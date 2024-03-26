﻿/*
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
 * Interface Klasse zum IEventAggregator
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

    public interface IEventAggregator
    {
        void Publish<TPayload>(TPayload payload) where TPayload : IPayload;

        ISubscription<TPayload> Subscribe<TPayload>(Action<TPayload> action) where TPayload : IPayload;

        void UnSubscribe<TPayload>(ISubscription<TPayload> subscription) where TPayload : IPayload;

        void ClearAllSubscriptions();

        void ClearAllSubscriptions(Type[] exceptMessages);
    }
}
