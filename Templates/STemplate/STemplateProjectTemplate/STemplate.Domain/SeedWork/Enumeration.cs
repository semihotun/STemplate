﻿using System.Reflection;
namespace STemplate.Domain.SeedWork
{
    /// <summary>
    /// Enumaration class
    /// </summary>
#nullable disable
    public abstract class Enumeration : IComparable
    {
        public string Name { get; set; }
        public int Id { get; set; }
        protected Enumeration(int id, string name) => (Id, Name) = (id, name);
        public override string ToString() => Name;
        public static IEnumerable<T> GetAll<T>() where T : Enumeration =>
            typeof(T).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                     .Select(x => x.GetValue(null))
                     .Cast<T>();
        public override bool Equals(object obj)
        {
            if (obj is not Enumeration otherValue)
            {
                return false;
            }
            var typeMatches = GetType().Equals(obj.GetType());
            var valueMatches = Id.Equals(otherValue.Id);
            return typeMatches && valueMatches;
        }
        public override int GetHashCode() => Id.GetHashCode();
        public static int AbsoluteDifference(Enumeration firstValue, Enumeration secondValue)
        {
            var absoluteDifference = Math.Abs(firstValue.Id - secondValue.Id);
            return absoluteDifference;
        }
        public static T FromValue<T>(int value) where T : Enumeration
        {
            var matchingItem = Parse<T, int>(value, "value", item => item.Id == value);
            return matchingItem;
        }
        public static T FromDisplayName<T>(string displayName) where T : Enumeration
        {
            var matchingItem = Parse<T, string>(displayName, "display name", item => item.Name == displayName);
            return matchingItem;
        }
        public static T Parse<T, K>(K value, string description, Func<T, bool> predicate) where T : Enumeration
        {
            var matchingItem = GetAll<T>().FirstOrDefault(predicate);
            return (T)matchingItem ?? throw new InvalidOperationException($"'{value}' is not valid {description} in {typeof(T)}");
        }
        public int CompareTo(object other) => Id.CompareTo(((Enumeration)other).Id);
    }
}
