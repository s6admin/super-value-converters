using System.Collections.Generic;
using System.Linq;

namespace Our.Umbraco.SuperValueConverters.Extensions
{
    internal static class IEnumerableExtensions
    {	/* S6 This implementation seems flawed? If an Interface applies to more than one allowed DocType
		   but not ALL of the allowed DocTypes, then the property collection will only contain values that
		   inherit from the shared interface. Items that only inherit from IPublishedContent are excluded.
		*/
        public static IEnumerable<T> IntersectMany<T>(this IEnumerable<IEnumerable<T>> values)
        {
            IEnumerable<T> intersection = null;

            foreach (var value in values)
            {
                if (intersection == null)
                {
                    intersection = new List<T>(value);
                }
                else
                {
                    intersection.Intersect(value);
                }
            }

            return intersection ?? Enumerable.Empty<T>();
        }

		/* S6 Try an option that intersects ALL the allowed DocTypes 
		 * https://codereview.stackexchange.com/questions/61627/producing-the-intersection-of-several-sequences 
		 */
		public static IEnumerable<TSource> IntersectAll<TSource>(this IEnumerable<IEnumerable<TSource>> source)
		{
			using (var enumerator = source.GetEnumerator())
			{
				if (!enumerator.MoveNext())
					yield break;

				var set = new HashSet<TSource>(enumerator.Current);
				while (enumerator.MoveNext())
					set.IntersectWith(enumerator.Current);
				foreach (var item in set)
					yield return item;
			}
		}
	}
}