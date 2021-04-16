using Abp.Domain.Services;
using System.Collections.Generic;

namespace Cepres.Patients.Patients
{
  public interface IPatientManager : IDomainService
  {
    PatientStatistics GetPatientStatistics(int patientId);
    void AssertUniqueNationalId(int patientId, string nationalId);
  }
}