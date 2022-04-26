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
        private const string DEFAULT_ID_COLUMN = "Id";

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

        public T ReadById(int id)
        {
            using (var reader = new StreamReader(_fileFullPath))
            using (var csv = new CsvReader(reader, _csvConfig))
            {
                csv.Context.RegisterClassMap(_classMap);

                while (csv.Read())
                {
                    var record = csv.GetRecord<T>();
                    if (record.GetType().GetProperty(DEFAULT_ID_COLUMN).GetValue(record).ToString() == id.ToString())
                    {
                        return record;
                    }
                }
            }
            throw new Exception("No record found");
        }

        public void Add(T record)
        {
            using (var writer = new StreamWriter(_fileFullPath, true))
            using (var csv = new CsvWriter(writer, _csvConfig))
            {
                csv.Context.RegisterClassMap(_classMap);
                
                csv.WriteRecord(record);
            }
        }

    }
}
