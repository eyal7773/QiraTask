using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;
using System.Linq.Dynamic.Core;

namespace Dal
{
    public class CsvRepo<T, TMap> : IRepo<T> where TMap : ClassMap<T>
    {
        #region locals
        private string _fileFullPath;
        private TMap _classMap;
        private CsvConfiguration _csvConfig;
        // Next line is prepration for case that we need to use different ID column for each instance of CsvRepo
        private const string DEFAULT_ID_COLUMN = "Id";
        private const int MAX_RECORDS_IN_ONE_QUERY = 500;
        #endregion

        #region constructor
        public CsvRepo(string fileFullPath, TMap classMap)
        {
            if (!File.Exists(fileFullPath))
            {
                throw new FileNotFoundException(null, fileFullPath);
            }

            _fileFullPath = fileFullPath;
            _classMap = classMap;
            _csvConfig = new CsvConfiguration(CultureInfo.CurrentCulture)
            {
                HasHeaderRecord = false,
                Delimiter = ";",
                MissingFieldFound = null,
            };

        }
        #endregion

        #region private functions
        private List<T> GetAllRecordsFromFile()
        {
            using var streamReader = File.OpenText(_fileFullPath);
            using var csvReader = new CsvReader(streamReader, _csvConfig);
            csvReader.Context.RegisterClassMap(_classMap);

            var records = new List<T>(csvReader.GetRecords<T>());
            return records;
        }
        private IQueryable<T> SkipTakeAndOrder(int pageLength,
                                             int startRecord,
                                             IQueryable<T> set,
                                             string sort = "")
        {

            pageLength = (pageLength > MAX_RECORDS_IN_ONE_QUERY) ? MAX_RECORDS_IN_ONE_QUERY : pageLength;


            if (sort == "")
            {
                set = set.Skip(startRecord).Take(pageLength);
            }
            else
            {
                // Using `Dynamic Linq`.
                set = set.OrderBy(sort).Skip(startRecord).Take(pageLength).AsQueryable();
            }

            return set;
        }
        #endregion

        #region public functions

        // `sort` string example: "Id desc" - means sort by Id descending.
        public DataWrapper<T> GetAll(int pageLength = 10,
                                     int startRecord = 0,
                                     string sort = "",
                                     string filterColumn = "",
                                     string filterTerm = "")
        {
            var records = GetAllRecordsFromFile();

            if (filterTerm != "")
            {
                // Using DynamicLinq.
                records = records.AsQueryable<T>().Where($"{filterColumn} == {filterTerm}").ToList();
            }

            var skiped = SkipTakeAndOrder(pageLength, startRecord, records.AsQueryable(), sort);

            var result = DataWrapper<T>.Create(records.Count(), skiped.Count(), skiped);
            return result;
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

            var records = GetAllRecordsFromFile();

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
        #endregion

    }
}
