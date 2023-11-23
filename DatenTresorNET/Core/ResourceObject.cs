//-----------------------------------------------------------------------
// <copyright file="ResourceObject.cs" company="Lifeprojects.de">
//     Class: ResourceObject
//     Copyright © Lifeprojects.de 2023
// </copyright>
//
// <Framework>7.0</Framework>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>13.07.2023 18:35:49</date>
//
// <summary>
// Klasse für 
// </summary>
//-----------------------------------------------------------------------

namespace DatenTresorNET.Core
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    public static class ResourceObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceObject"/> class.
        /// </summary>
        static ResourceObject()
        {
        }

        public static TResult GetAs<TResult>(string resourceName)
        {
            TResult resourceContent = default(TResult);

            if (HasKey(resourceName) == false)
            {
                return default(TResult);
            }

            try
            {
                if (typeof(TResult) == typeof(Brush))
                {
                    object ressorce = (TResult)Application.Current.TryFindResource(resourceName);
                    resourceContent = (TResult)Convert.ChangeType(ressorce, typeof(TResult));
                }
                else if (typeof(TResult) == typeof(DrawingImage))
                {
                    object ressorce = (TResult)Application.Current.TryFindResource(resourceName);
                    resourceContent = (TResult)Convert.ChangeType(ressorce, typeof(TResult));
                }
                else if (typeof(TResult) == typeof(Viewbox))
                {
                    object ressorce = (TResult)Application.Current.TryFindResource(resourceName);
                    resourceContent = (TResult)Convert.ChangeType(ressorce, typeof(TResult));
                }
                else if (typeof(TResult) == typeof(Geometry))
                {
                    object ressorce = (TResult)Application.Current.TryFindResource(resourceName);
                    resourceContent = (TResult)Convert.ChangeType(ressorce, typeof(TResult));
                }
                else if (typeof(TResult) == typeof(System.Windows.Style))
                {
                    object ressorce = (TResult)Application.Current.TryFindResource(resourceName);
                    resourceContent = (TResult)Convert.ChangeType(ressorce, typeof(TResult));
                }
                else if (typeof(TResult) == typeof(string))
                {
                    object ressorce = (TResult)Application.Current.TryFindResource(resourceName);
                    resourceContent = (TResult)Convert.ChangeType(ressorce, typeof(TResult));
                }
                else
                {
                    throw new NotSupportedException($"{resourceName} kann nicht nach {typeof(TResult).Name} konvertiert werden. ");
                }
            }
            catch
            {
                return default(TResult);
            }

            return resourceContent;
        }

        public static bool HasKey(string objectKey)
        {
            bool result = false;

            if (Application.Current.Resources.Contains(objectKey) == true)
            {
                result = true;
            }

            return result;
        }
    }
}
