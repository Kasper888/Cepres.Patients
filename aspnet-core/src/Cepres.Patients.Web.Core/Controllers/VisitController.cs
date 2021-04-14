using Abp.AspNetCore.OData.Controllers;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Web.Models;
using Cepres.Patients.Authorization;
using Cepres.Patients.Patients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cepres.Patients.Controllers
{
  [DontWrapResult]
  public class VisitController : AbpODataEntityController<Visit>, IPerWebRequestDependency
  {
    public VisitController(IRepository<Visit> repo) : base(repo)
    {

    }
  }
}
