
using Abp.AutoMapper;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Web.Models;
using Cepres.Patients.Authorization;
using Cepres.Patients.Patients;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cepres.Patients.Controllers
{
  public class PatientController : ODataEntityController<Patient>
  {
    private readonly IPatientManager _patientManager;

    public PatientController(IRepository<Patient> repo, IPatientManager patientManager) : base(repo)
    {
      GetPermissionName = PermissionNames.Patient_Get;
      GetAllPermissionName = PermissionNames.Patient_List;
      UpdatePermissionName = PermissionNames.Patient_Update;
      CreatePermissionName = PermissionNames.Patient_Create;
      _patientManager = patientManager;
    }
    public async override Task<IActionResult> Patch(int key, Delta<Patient> entity)
    {
      if (entity.TryGetPropertyValue(nameof(Patient.NationalId), out object nationalId))
      {
        _patientManager.AssertUniqueNationalId(key, (string)nationalId);
      }
      return await base.Patch(key, entity);
    }
    public async override Task<IActionResult> Post([FromBody] Patient entity)
    {
      _patientManager.AssertUniqueNationalId(entity.Id, entity.NationalId);

      return await base.Post(entity);
    }
  }
}
