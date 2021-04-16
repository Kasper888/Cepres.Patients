using Abp.AspNetCore.Mvc.Controllers;
using Abp.Authorization;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.AspNetCore.OData.Routing.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cepres.Patients.Controllers
{
  [ODataRouting, DontWrapResult, ApiExplorerSettings(IgnoreApi = true)]
  public class ODataEntityController<TEntity> : PatientsControllerBase
          where TEntity : class, IEntity<int>
  {
    protected virtual string GetPermissionName { get; set; }

    protected virtual string GetAllPermissionName { get; set; }

    protected virtual string CreatePermissionName { get; set; }

    protected virtual string UpdatePermissionName { get; set; }

    protected IRepository<TEntity> Repository { get; private set; }

    protected ODataEntityController(IRepository<TEntity> repository)
    {
      Repository = repository;
    }

    [EnableQuery]
    public virtual IQueryable<TEntity> Get()
    {
      CheckPermission(GetAllPermissionName);

      return Repository.GetAll();
    }
    [EnableQuery]
    public virtual SingleResult<TEntity> Get(int key)
    {
      CheckPermission(GetPermissionName);

      var entity = Repository.GetAll().Where(e => e.Id == key);

      return SingleResult.Create(entity);
    }
    public virtual async Task<IActionResult> Post([FromBody]TEntity entity)
    {
      CheckPermission(CreatePermissionName);

      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      var createdEntity = await Repository.InsertAsync(entity);
      await UnitOfWorkManager.Current.SaveChangesAsync();

      return new CreatedODataResult<TEntity>(createdEntity);
    }
    public virtual async Task<IActionResult> Patch(int key, Delta<TEntity> entity)
    {
      CheckPermission(UpdatePermissionName);

      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      var dbLookup = await Repository.GetAsync(key);
      if (dbLookup == null)
      {
        return NotFound();
      }

      entity.Patch(dbLookup);

      return new UpdatedODataResult<TEntity>(dbLookup);
    }
    protected virtual void CheckPermission(string permissionName)
    {
      if (!string.IsNullOrEmpty(permissionName))
      {
        PermissionChecker.Authorize(permissionName);
      }
    }
  }
}
