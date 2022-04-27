using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal
{
    public class DataWrapper<T>
    {
        public static DataWrapper<T> Create(int recordsTotal, int recordsFiltered, IEnumerable<T> data)
        {
            return new DataWrapper<T>
            {
                RecordsTotal = recordsTotal,
                RecordsFiltered = recordsFiltered,
                Data = data
            };
        }
        public int RecordsTotal { get; set; }
        public int RecordsFiltered { get; set; }
        public IEnumerable<T> Data { get; set; }
    }
}
