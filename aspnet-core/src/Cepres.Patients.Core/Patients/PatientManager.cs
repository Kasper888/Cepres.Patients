using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.Timing;
using Abp.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Cepres.Patients.Patients
{

  public class PatientManager : DomainService, IPatientManager
  {
    private const int MinimumSimilarDisease = 2;
    private readonly IRepository<Patient> _patientRepository;


    public PatientManager(IRepository<Patient> patientRepository)
    {
      _patientRepository = patientRepository;
    }
    /// <summary>
    ///  Get List of other patients with similar diseases (Similar diseases mean that the two patients have in common 2 or more diseases)
    /// </summary>
    /// <returns>Dictionary of Patient Id as key and Name as value</returns>
    protected virtual Dictionary<int, string> GetSimilarPatientsOrNull(Patient patient)
    {
      var diseases = patient.Visits.Select(v => v.Disease).ToHashSet();

      if (diseases.Count < MinimumSimilarDisease)
      {
        return null;
      }
      return _patientRepository.GetAll()
        .Where(p => p.Id != patient.Id && p.Visits.Select(v => v.Disease).Distinct().Count(d => diseases.Contains(d)) >= MinimumSimilarDisease)
        .Select(p => new { p.Id, p.Name })
        .ToDictionary(p => p.Id, p => p.Name);
    }
    public void AssertUniqueNationalId(int patientId, string nationalId)
    {
      if (_patientRepository.GetAll().Any(s => s.Id != patientId && s.NationalId == nationalId))
      {
        throw new UserFriendlyException("National Id is already taken!");
      }
    }
    public PatientStatistics GetPatientStatistics(int patientId)
    {
      var patient = _patientRepository.GetAllIncluding(p => p.Visits).First(p => p.Id == patientId);
      return PatientStatistics.Create(patient.Name, patient.BirthDate, patient.Visits, GetSimilarPatientsOrNull(patient));
    }
  }
}
