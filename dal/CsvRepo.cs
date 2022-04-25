using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using CsvHelper;
using CsvHelper.Configuration;

namespace Dal
{
    public class CsvRepo<T, TMap> where TMap : ClassMap<T>
    {
        private string _fileFullPath;
        private TMap _classMap;
        private CsvConfiguration _csvConfig;

        public CsvRepo(string fileFullPath, TMap classMap)
        {
            if (!File.Exists(fileFullPath))
            {
                throw new FileNotFoundException(null,fileFullPath);
            }

            _fileFullPath = fileFullPath;
            _classMap = classMap;
            _csvConfig = new CsvConfiguration(CultureInfo.CurrentCulture)
            {
                HasHeaderRecord = false, Delimiter = ";"
                
            };
            
            

            

        }

        public List<T> ReadAll() 
        {
            using var streamReader = File.OpenText(_fileFullPath);
            using var csvReader = new CsvReader(streamReader, _csvConfig);
            csvReader.Context.RegisterClassMap(_classMap);

            var records = new List<T>(csvReader.GetRecords<T>());

            return records;

        }
    }
}
