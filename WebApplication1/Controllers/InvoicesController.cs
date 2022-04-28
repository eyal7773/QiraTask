using Dal;
using Dal.Maps;
using Microsoft.AspNetCore.Mvc;
using Models;
using System.Collections.Generic;
using WebApi.DTO;
using WebApi.DTO.Extensions;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoicesController : ControllerBase
    {
        private IRepo<Invoice> _repo;

        public InvoicesController(IRepo<Invoice> repo)
        {
            _repo = repo;
        }

        /// <summary>
        /// Get all invoices, paginated, sorted.
        /// </summary>
        /// <remarks>
        /// `sort` string example: "Id desc" - means sort by Id descending.
        /// `filterColumn` + `filterTerm` - used to filter, for example: `filterColumn`='Amount' and `filterTerm`='100' - means filter by Amount=100.
        /// </remarks>
        /// <param name="pageLength">how many records in each page</param>
        /// <param name="startRecord">which record to start with</param>
        /// <param name="sort">`sort` string example: "Id desc" - means sort by Id descending.</param>
        /// <returns>Page object with list of records.</returns>
        [HttpGet]
        public DataWrapper<Invoice> Get([FromQuery] int pageLength = 10,
                                        [FromQuery] int startRecord = 0,
                                        [FromQuery] string sort = "",
                                        [FromQuery] string filterColumn = "",
                                        [FromQuery] string filterTerm = "")
        {
            return _repo.GetAll(pageLength, startRecord, sort,filterColumn,filterTerm);
        }

        [HttpGet("{id}")]
        public Invoice Get(int id)
        {
            return _repo.GetById(id);
        }

        [HttpPost]
        public void Post([FromBody] InvoiceParams invoiceToAdd)
        {
            
            _repo.Add(invoiceToAdd.ToInvoice());
     
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] InvoiceParams invoiceToChange)
        {
            _repo.Update(invoiceToChange.ToInvoice(), id);
        }
        

        // Not requested in spec.
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
