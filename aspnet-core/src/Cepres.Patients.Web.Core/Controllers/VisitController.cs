
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
  public class VisitController : ODataEntityController<Visit>
  {
    public VisitController(IRepository<Visit> repo) : base(repo)
    {
      
    }
  }
}
