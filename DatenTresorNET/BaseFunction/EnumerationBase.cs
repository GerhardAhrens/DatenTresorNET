/*
 * <copyright file="EnumerationBase.cs" company="Lifeprojects.de">
 *     Class: EnumerationBase
 *     Copyright © Lifeprojects.de 2022
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>developer@lifeprojects.de</email>
 * <date>21.11.2022</date>
 * <Project>EasyPrototypingNET</Project>
 *
 * <summary>
 * Abstrakte Klasse zur Erstellung einer Basisklasse Enumeration
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

namespace DatenTresorNET.BaseFunction
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public abstract class EnumerationBase : IComparable, IEnumerationBase
    {
        private readonly int key;
        private readonly string value;
        private readonly string description;

        protected EnumerationBase()
        {
        }

        protected EnumerationBase(int key, string value, string description)
        {
            this.key = key;
            this.value = value;
            this.description = description;
        }

        public int Key
        {
            get { return this.key; }
        }

        public string Value
        {
            get { return this.value; }
        }

        public string Description
        {
            get { return this.description; }
        }

        public static IEnumerable<T> ToEnumerable<T>()
        {
            var type = typeof(T);
            ConstructorInfo[] constructors = type.GetConstructors();
            if (constructors.Count() != 1 && constructors.Count() != 2)
            {
                throw new Exception($"Conctructor in '{typeof(T).Name}' not found.");
            }

            FieldInfo[] fields = type.GetFields();

            foreach (var fieldInfo in fields)
            {
                if (fieldInfo.IsInitOnly == true)
                {
                    T instance = (T)Activator.CreateInstance(typeof(T));
                    if (instance != null)
                    {
                        object locatedValue = fieldInfo.GetValue(instance);
                        yield return (T)locatedValue;
                    }
                }
            }
        }

        public static Dictionary<int,string> ToDictionary<T>() where T : class
        {

            var type = typeof(T);
            ConstructorInfo[] constructors = type.GetConstructors();
            if (constructors.Count() != 1 && constructors.Count() != 2)
            {
                throw new Exception($"Conctructor in '{typeof(T).Name}' not found.");
            }

            Dictionary<int, string> resultValues = new Dictionary<int, string>();
            FieldInfo[] fields = type.GetFields();

            foreach (var fieldInfo in fields)
            {
                if (fieldInfo.IsInitOnly == true)
                {
                    T instance = (T)Activator.CreateInstance(typeof(T));
                    if (instance != null)
                    {
                        IEnumerationBase locatedValue = fieldInfo.GetValue(instance) as IEnumerationBase;
                        resultValues.Add(locatedValue.Key, locatedValue.Value);
                    }
                }
            }

            return resultValues;
        }

        public override string ToString()
        {
            return this.Value;
        }

        public override bool Equals(object obj)
        {
            var otherValue = obj as EnumerationBase;

            if (otherValue == null)
            {
                return false;
            }

            var typeMatches = GetType().Equals(obj.GetType());
            var valueMatches = key.Equals(otherValue.Key);

            return typeMatches && valueMatches;
        }

        public override int GetHashCode()
        {
            return this.Key.GetHashCode();
        }

        public static T FromKey<T>(int value) where T : EnumerationBase, new()
        {
            var matchingItem = Parse<T, int>(value, "value", item => item.Key == value);

            return matchingItem;
        }

        public static T FromName<T>(string value) where T : EnumerationBase, new()
        {
            var matchingItem = Parse<T, string>(value, "value", item => item.Value.ToLower() == value.ToLower());
            return matchingItem;
        }

        public int CompareTo(object other)
        {
            return this.Key.CompareTo(((EnumerationBase)other).Key);
        }

        private static T Parse<T, K>(K value, string description, Func<T, bool> predicate) where T : EnumerationBase, new()
        {
            T matchingItem = ToEnumerable<T>().FirstOrDefault(predicate);

            if (matchingItem == null)
            {
                var message = $"'{value}' is not a valid {description} in {typeof(T).Name}";
                throw new ApplicationException(message);
            }

            return matchingItem;
        }
    }
}
