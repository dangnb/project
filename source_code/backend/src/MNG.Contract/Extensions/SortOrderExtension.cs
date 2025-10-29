using MNG.Contract.Enumerations;

namespace MNG.Contract.Extensions;
public static class SortOrderExtension
{
    public static SortOrder ConvertStringToSortOder(string? sortOrder)
         => !string.IsNullOrWhiteSpace(sortOrder)
            ? sortOrder.ToLower(System.Globalization.CultureInfo.CurrentCulture).Equals("asc", StringComparison.Ordinal) ? SortOrder.Ascending : SortOrder.Descending
            : SortOrder.Descending;

    public static IDictionary<string, SortOrder> ConvertStringToSortOderV2(string? sortColumnAndOrder)
    {
        var result = new Dictionary<string, SortOrder>();
        if (!string.IsNullOrWhiteSpace(sortColumnAndOrder))
        {
            string[] sortColumnAndOrderAfterSlit = sortColumnAndOrder.Trim().Split("_");
            if (sortColumnAndOrderAfterSlit.Length > 0)
            {
                foreach (string sort in sortColumnAndOrderAfterSlit)
                {
                    if (sort.Contains('-'))
                    {
                        var property = sortColumnAndOrder.Split('-');
                        var key = ProductExtension.GetSortProductProperty(property[0]);
                        var value = ConvertStringToSortOder(property[1]);
                        result.TryAdd(key, value);
                    }
                    else
                        throw new FormatException("Sort conditon should be follow by format: Column1-Asc ...");
                }
            }
            else
            {
                if (sortColumnAndOrder.Contains('-'))
                {
                    var property = sortColumnAndOrder.Split('-');
                    var key = ProductExtension.GetSortProductProperty(property[0]);
                    result.Add(key, ConvertStringToSortOder(property[1]));
                }
                else
                    throw new FormatException("Sort conditon should be follow by format: Column1-Asc ...");

             
            }
        }
        return result;
    }
}
