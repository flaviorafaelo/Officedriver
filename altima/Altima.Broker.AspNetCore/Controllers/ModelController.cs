using Microsoft.AspNet.OData;

using Altima.Broker.Business;
using Altima.Broker.Data;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Altima.Broker.System;
using Altima.Broker.Metadata.Generator;

namespace Altima.Broker.AspNet.Mvc.Controllers
{
    [Route("api/[controller]")]
    public class ModelController<T> : ODataController/*ControllerBase*/ where T : BaseModel
    {
        private readonly IAsyncRepository<T> _repository;

        public ModelController(IAsyncRepository<T> Repository)
        {
            _repository = Repository;
        }

       // [HttpGet]
        [EnableQuery]
        public IQueryable<T> Get()
        {
            var value = _repository.ListAllAsync();

            return new EnumerableQuery<T>((IEnumerable<T>)value);
        }

         [HttpPost]
         public async Task<ActionResult<T>> Create([FromBody] T model)
         {
             await _repository.AddAsync(model);
             return Created("", model);
         }

         [HttpDelete]
         [Route("{id}")]
         ///<summary>
         ///isso é um teste
         ///</summary>
         public async Task<ActionResult> Delete(long id)
         {
             var deletedCount = await _repository.DeleteAsync(id);
             if (deletedCount == 0)
                 return NotFound();

             return NoContent();
         }

         [HttpGet]
         [Route("{id}")]
         public async Task<ActionResult<T>> Read(long id)
         {
             var model = await _repository.GetByIdAsync(id);
             if (model == null)
                 return NotFound();

             return Ok(model);
         }

         [HttpPut]
         [Route("{id}")]
         public async Task<ActionResult> Update(long id, [FromBody] T model)
         {
             var modifiedCount = await _repository.UpdateAsync(id, model);

             //if (_notificationContext.HasNotifications)
             //    return BadRequest(_notificationContext.Notifications);

             if (modifiedCount == 0)
                 return Conflict();

             return Ok();
         }

         //PACTH

         [HttpGet]
         [Route("[controller]/metadata")]
         public async Task<ActionResult<Model>> Metadata()
         {
             //pegar essa informação do cache em application
             return Ok(ModelGenerator.Create(typeof(T)));
         }

         [HttpGet]
         [Route("[controller]/metaview")]
         public async Task<ActionResult<View>> Metaview()
         {
             Model model = ModelGenerator.Create(typeof(T));
             return Ok(ViewGenerator.Create(model));
         }

     }
}