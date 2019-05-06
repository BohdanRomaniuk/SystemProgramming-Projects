using Excel.Models;

namespace Excel.Helpers
{
    public static class DataModelHelper
    {
        public static bool IsEmpty(this DataModel row)
        {
            var props = row.GetType().GetProperties();
            foreach (var prop in props)
            {
                if (prop.GetValue(row) != null || !string.IsNullOrEmpty((string)prop.GetValue(row)))
                {
                    return false;
                }
            }
            return true;
        }

        public static void SetValue(this DataModel row, string columnName, string columnValue)
        {
            var propertyInfo = row.GetType().GetProperty(columnName);
            propertyInfo?.SetValue(row, columnValue, null);
        }

        public static double GetValue(this DataModel row, string propertyName)
        {
            int.TryParse(row.GetType().GetProperty(propertyName)?.GetValue(row)?.ToString(), out var result);
            return result;
        }
    }
}
