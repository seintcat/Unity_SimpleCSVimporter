using System.Collections.Generic;
using System.Net.Http;
using UnityEngine;

public static class CSVimporter
{
    public static Dictionary<Vector2Int, string> ReadCsvRaw(string data)
    {
        Dictionary<Vector2Int, string> sheetRaw = new Dictionary<Vector2Int, string>();
        Vector2Int size = Vector2Int.zero;

        // DoubleQuote
        data = data
            .Replace("\"\"\",", "_DQ_\",")
            .Replace(",\"\"\"", ",\"_DQ_")
            .Replace("\"\"", "_DQ_");

        // Enter, Comma
        while(data.Contains('"'))
        {
            int index = data.IndexOf("\"");
            string unwrapData = data.Substring(0, index);
            data = data.Substring(index + 1);
            index = data.IndexOf("\"");
            string wrapped = data.Substring(0, index);
            data = unwrapData + wrapped.Replace("\n", "_CR_").Replace(",", "_Del_") + data.Substring(index + 1);
        }
        string[] rows = data.Split('\n');
        size.y = rows.Length;
        for (int y = 0; y < rows.Length; ++y)
        {
            string[] cells = rows[y].Split(',');
            if (size.x < cells.Length)
            {
                size.x = cells.Length;
            }
            for (int x = 0; x < cells.Length; ++x)
            {
                // DoubleQuote, Enter, Comma
                sheetRaw.Add(new Vector2Int(x, y), cells[x].Replace("_Del_", ",").Replace("_CR_", "\n").Replace("_DQ_", "\""));
            }
        }
        sheetRaw.Add(new Vector2Int(-1, -1), $"{size.x},{size.y}");

        return sheetRaw;
    }
}