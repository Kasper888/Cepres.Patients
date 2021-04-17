using Abp.Domain.Repositories;
using Abp.Timing;
using Abp.UI;
using Accord.Statistics;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cepres.Patients.Patients
{
  public class PatientStatistics
  {
    private const double OutliersPercent = .2;
    //Composition over inheritance principle
    protected Patient patient;
    protected PatientStatistics()
    {
    }
    public static PatientStatistics Create(Patient patient, Dictionary<int, string> similarPatientsInDisease = null)
    {
      var patientStatics = new PatientStatistics();
      patientStatics.patient = patient;
      if (patient.Visits.Any())
      {
        patientStatics.SetTopVisitMonth();
        patientStatics.SimilarPatientsInDisease = similarPatientsInDisease;
      }

      return patientStatics;
    }
    public virtual string Name => patient.Name;
    public virtual DateTime? FifthVisit => patient.Visits.Skip(4).FirstOrDefault()?.CreationTime;
    public virtual int? Age => Clock.Now.Year - patient.BirthDate?.Year;
    public virtual double BillsAverage => patient.Visits.Average(v => v.Fees);
    public virtual double BillsAverageWithoutOutliers => patient.Visits.Select(v => v.Fees).ToArray().TruncatedMean(OutliersPercent);
    public virtual int VisitsCount => patient.Visits.Count;
    public virtual string TopVisitMonth { get; protected set; }
    public virtual Dictionary<int, string> SimilarPatientsInDisease { get; protected set; }

    protected virtual void SetTopVisitMonth()
    {
      var topVisitMonthNumber = patient.Visits.GroupBy(v => v.CreationTime.Month).OrderBy(g => g.Count()).Last().Key;
      TopVisitMonth = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(topVisitMonthNumber);
    }
  }
}
