using System.Collections;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Text.RegularExpressions;

namespace games_infrastructure_pricecharting_api.PricingUpdate.Parsers;

public class CsvParser<TRecord> : IEnumerable<TRecord>
    where TRecord : class, new()
{
    private readonly Stream? _inputStream;
    private readonly string? _inputFile;
    private Dictionary<int, PropertyInfo>? _mappings;

    public CsvParser(
        string inputFile,
        Dictionary<int, PropertyInfo>? mappings = null)
    {
        _inputFile = inputFile;
        _mappings = mappings;
    }

    public CsvParser(
        Stream inputStream,
        Dictionary<int, PropertyInfo>? mappings = null)
    {
        _inputStream = inputStream;
        _mappings = mappings;
    }

    private const string MatchingPattern = ",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))";

    public IEnumerator<TRecord> GetEnumerator()
    {
        using var reader = _inputStream is not null ? new StreamReader(_inputStream!) : new StreamReader(_inputFile!);

        SetHeaderMappings(reader.ReadLine());

        while (!reader.EndOfStream)
        {
            string record = reader.ReadLine();

            string[] components = Regex.Split(record, MatchingPattern);

            var result = new TRecord();

            for (int idx = 0; idx < components.Length; idx++)
            {
                var property = _mappings[idx];

                property.SetValue(result, Convert.ChangeType(components[idx], property.PropertyType));
            }

            yield return result;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    private void SetHeaderMappings(string header)
    {
        var properties = typeof(TRecord).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        string[] components = Regex.Split(header, MatchingPattern);

        _mappings = (from p in properties
            let column = p.GetCustomAttribute(typeof(ColumnAttribute)) as ColumnAttribute
            let matched = (from c in components
                where (column != null && c == column.Name) ||
                      (column == null && c.Equals(p.Name, StringComparison.InvariantCultureIgnoreCase))
                select c).SingleOrDefault()
            where matched != null
            select new
            {
                Index = Array.IndexOf(components, matched),
                Property = p
            }).ToDictionary(x => x.Index, x => x.Property);
    }

    IEnumerator<TRecord> IEnumerable<TRecord>.GetEnumerator()
    {
        return GetEnumerator();
    }
}