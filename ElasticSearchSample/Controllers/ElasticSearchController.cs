using ElasticSearchSample.Models;
using ElasticSearchSample.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace ElasticSearchSample.Controllers
{
    [Route("api/elasticSearch/")]
    [ApiController]
    public class ElasticSearchController : ControllerBase
    {
        private readonly IConfiguration _config;

        public ElasticSearchController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet]
        [Route("getByTerm/{term}")]
        public ActionResult<string> Get(string term)
        {
            var repo = new ElasticSearchRepositorie(_config);
            var result = repo.GetByTerm(term);

            return Ok(result.Documents);
        }

        [HttpGet]
        [Route("getById/{id}")]
        public ActionResult<string> Get(int id)
        {
            var repo = new ElasticSearchRepositorie(_config);
            var result = repo.GetById(id);

            return Ok(result.Documents);
        }

        [HttpPost]
        public void Post([FromBody] Content content)
        {
            var repo = new ElasticSearchRepositorie(_config);
            repo.Insert(content);
        }
    }
}
