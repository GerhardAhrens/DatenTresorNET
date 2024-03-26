//-----------------------------------------------------------------------
// <copyright file="SwitchDialogEventArgs.cs" company="Lifeprojects.de">
//     Class: SwitchDialogEventArgs
//     Copyright © Lifeprojects.de 2021
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>29.05.2021</date>
//
// <summary>
// Argument Class zum Darstellen des Wechseln zwischen zwei Dialogen
// </summary>
//-----------------------------------------------------------------------

namespace DatenTresorNET.Core
{
    using System;

    using DatenTresorNET.BaseFunction.Pattern;

    /// <summary>
    /// Argument beim Wechslem zwischen den UserControl-Dialogen
    /// </summary>
    /// <typeparam name="IViewModel"></typeparam>
    public class MessageEventArgs : EventArgs, IPayload
    {
        public Type Sender { get; set; }

        public string Text { get; set; }
    }
}
