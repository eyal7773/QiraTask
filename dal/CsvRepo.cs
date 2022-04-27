using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;

namespace Dal
{
    public class CsvRepo<T, TMap> where TMap : ClassMap<T>
    {
        private string _fileFullPath;
        private TMap _classMap;
        private CsvConfiguration _csvConfig;
        // Next is prepration for case that we need to use different ID column for each instance of CsvRepo
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
                HasHeaderRecord = false, 
                Delimiter = ";",
                MissingFieldFound = null    ,
            };

        }

        public List<T> GetAll() 
        {
            using var streamReader = File.OpenText(_fileFullPath);
            using var csvReader = new CsvReader(streamReader, _csvConfig);
            csvReader.Context.RegisterClassMap(_classMap);

            var records = new List<T>(csvReader.GetRecords<T>());

            return records;

        }

        public T GetById(int id)
        {
            using (var reader = new StreamReader(_fileFullPath))
            using (var csv = new CsvReader(reader, _csvConfig))
            {
                csv.Context.RegisterClassMap(_classMap);

                while (csv.Read())
                {
                    var record = csv.GetRecord<T>();
                    // Because class is Generic, so we need to use reflection to get Id property.
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
                csv.NextRecord(); 
                csv.WriteRecord(record);
            }
        }
        public void Update(T newData, int id)
        {
            // In case of real world application, this solution is not the best one,
            // because it's not so effiecent to rewrite all records in the file.
            // And a better solution will be to update specific fields, which is not implemented here.
            // But I think, for this example, it's ok.

            var records = GetAll();

            // Find index is needed to get the object by ref from the list.
            // Otherwise, we can't swap the object with the new one.
            var index = records.FindIndex(r => r.GetType().GetProperty(DEFAULT_ID_COLUMN).GetValue(r).ToString() == id.ToString());
            
            if (index > -1)
            {
                // Swap the object with the new one.
                records[index] = newData;
                using (var writer = new StreamWriter(_fileFullPath))
                using (var csv = new CsvWriter(writer, _csvConfig))
                {
                    csv.Context.RegisterClassMap(_classMap);
                    csv.WriteRecords(records);
                }
            }
            else
            {
                throw new Exception("No record found");
            }
        }

    }
}
