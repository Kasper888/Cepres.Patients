using Abp.Authorization;
using Cepres.Patients.Authorization;
using Cepres.Patients.Patients;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cepres.Patients.Reporting
{
  [AbpAuthorize(PermissionNames.Reporting)]
  public class ReportingAppService : PatientsAppServiceBase
  {
    private readonly IPatientManager _patientManager;

    public ReportingAppService(IPatientManager patientManager)
    {
      _patientManager = patientManager;
    }
    [AbpAuthorize(PermissionNames.Reporting_PatientStatistics)]
    public PatientStatistics GetPatientStatistics([Range(1, int.MaxValue)] int id)
    {
      return _patientManager.GetPatientStatistics(id);
    }
  }
}
