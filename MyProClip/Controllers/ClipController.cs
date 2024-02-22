using Microsoft.AspNetCore.Mvc;

namespace MyProClip.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClipController : ControllerBase
    {
        // GET: api/<ClipController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<ClipController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ClipController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ClipController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ClipController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
