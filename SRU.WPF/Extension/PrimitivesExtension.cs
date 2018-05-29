﻿using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Sru.Wpf.Extension
{
    public static class PrimitivesExtension
    {
        /// <summary>
        /// Converts any object to base 64 string
        /// This helps with persisting objects as string
        /// </summary>
        /// <typeparam name="T">The type of the object</typeparam>
        /// <param name="obj">The object to be converted</param>
        /// <returns></returns>
        public static string ToBase64String<T>(this T obj)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(ms, obj);
                ms.Position = 0;
                byte[] buffer = new byte[(int)ms.Length];
                ms.Read(buffer, 0, buffer.Length);
                return Convert.ToBase64String(buffer);
            }                
        }

        /// <summary>
        /// Converts a base 64 string back to object instance
        /// </summary>
        /// <typeparam name="T">Type of th eobj to be returned</typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T FromBase64String<T>(this string obj)
        {
            using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(obj)))
            {
                BinaryFormatter bf = new BinaryFormatter();
                return (T)bf.Deserialize(ms);
            }
        }

        /// <summary>
        /// Determines whether the object is equal to either of elements in a sequence.
        /// </summary>
        [Pure]
        public static bool IsEither<T>(this T obj, [NotNull] IEnumerable<T> enumerable,
            [NotNull] IEqualityComparer<T> comparer)
        {
            if (enumerable == null)
                throw new ArgumentNullException(nameof(enumerable));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return enumerable.Any(other => comparer.Equals(obj, other));
        }

        /// <summary>
        /// Determines whether the object is equal to either of elements in a sequence.
        /// </summary>
        [Pure]
        public static bool IsEither<T>(this T obj, [NotNull] IEnumerable<T> enumerable)
            => IsEither(obj, enumerable, EqualityComparer<T>.Default);

        /// <summary>
        /// Determines whether the object is equal to either of the parameters.
        /// </summary>
        [Pure]
        public static bool IsEither<T>(this T obj, params T[] objs)
            => IsEither(obj, (IEnumerable<T>)objs);
    }
}
