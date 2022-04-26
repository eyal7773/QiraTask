using Dal;
using Dal.Maps;
using Microsoft.AspNetCore.Mvc;
using Models;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoicesController : ControllerBase
    {
        private CsvRepo<Invoice, InvoiceMap> _repo;

        public InvoicesController(CsvRepo<Invoice,InvoiceMap> repo)
        {
            _repo = repo;
        }
        
        [HttpGet]
        public IEnumerable<Invoice> Get()
        {
            return _repo.ReadAll();
        }

        [HttpGet("{id}")]
        public Invoice Get(int id)
        {
            return _repo.ReadById(id);
        }

        [HttpPost]
        public void Post([FromBody] Invoice invoice)
        {
            _repo.Add(invoice);
     
        }

        // PUT api/<InvoicesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<InvoicesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
