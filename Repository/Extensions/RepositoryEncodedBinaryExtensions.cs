using Entities.Models;
using Repository.Extensions.Utility;
using System.Linq.Dynamic.Core;

namespace Repository.Extensions
{
    public static class RepositoryEncodedBinaryExtensions
    {
        public static IQueryable<EncodedBinary> FilterEncodedBinaries(this IQueryable<EncodedBinary> encodedBinaries, uint minKeyId, uint maxKeyId) =>
            encodedBinaries.Where(eb => (eb.KeyId >= minKeyId && eb.KeyId <= maxKeyId));
        
        // Search is temporalily disabled:
        //public static IQueryable<EncodedBinary> Search(this IQueryable<EncodedBinary> encodedBinaries, string searchTerm)
        //{
        //    if (string.IsNullOrWhiteSpace(searchTerm))
        //        return encodedBinaries;

        //    var lowerCaseTerm = searchTerm.Trim().ToLower();

        //    return encodedBinaries.Where(eb => eb.Data.dataToBase64String().ToLower().Contains(lowerCaseTerm));
        //}

        public static IQueryable<EncodedBinary> Sort(this IQueryable<EncodedBinary> encodedBinaries, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
                return encodedBinaries.OrderBy(eb => eb.KeyId);

            var orderQuery = OrderQueryBuilder.CreateOrderQuery<EncodedBinary>(orderByQueryString);

            if (string.IsNullOrWhiteSpace(orderQuery))
                return encodedBinaries.OrderBy(eb => eb.KeyId);

            return encodedBinaries.OrderBy(orderQuery);
        }

        //private static string dataToBase64String(this byte[] data) =>
        //    Convert.ToBase64String(data);

    }
}
