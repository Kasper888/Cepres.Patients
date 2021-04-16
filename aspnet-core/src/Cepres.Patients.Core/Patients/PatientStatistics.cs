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
    private const int HumanMaxAge = 120;
    private const double OutliersPercent = .2;

    protected PatientStatistics()
    {

    }
    public static PatientStatistics Create(string name, DateTime? birthDate, HashSet<Visit> visits, Dictionary<int, string> similarPatientsInDisease = null)
    {
      var patientStatics = new PatientStatistics();
      patientStatics.Name = name;
      patientStatics.CalculateAge(birthDate);
      if (visits.Any())
      {
        patientStatics.SimilarPatientsInDisease = similarPatientsInDisease;
        patientStatics.SetFifthVisit(visits);
        patientStatics.VisitTotalCount = visits.Count();
        patientStatics.BillsAverage = visits.Average(v => v.Fees);
        patientStatics.SetBillsAverageWithoutOutliers(visits);
        patientStatics.SetTopVisitMonth(visits);
      }

      return patientStatics;
    }
    public virtual string Name { get; protected set; }
    public virtual DateTime? FifthVisit { get; protected set; }
    public virtual int? Age { get; protected set; }
    public virtual double BillsAverage { get; protected set; }
    public virtual double BillsAverageWithoutOutliers { get; protected set; }
    public virtual int VisitTotalCount { get; protected set; }
    public virtual string TopVisitMonth { get; protected set; }
    public virtual Dictionary<int, string> SimilarPatientsInDisease { get; protected set; }

    protected virtual void CalculateAge(DateTime? birthDate)
    {
      if (birthDate == null)
      {
        Age = null;
        return;
      }
      Age = Clock.Now.Year - birthDate.Value.Year;
      AssertValidAge();
    }

    protected virtual void SetFifthVisit(IEnumerable<Visit> visits)
    {
      FifthVisit = visits.Skip(4).FirstOrDefault()?.CreationTime;
    }
    protected virtual void SetBillsAverageWithoutOutliers(IEnumerable<Visit> visits)
    {
      BillsAverageWithoutOutliers = visits.Select(v => v.Fees).ToArray().TruncatedMean(OutliersPercent);
    }
    protected virtual void SetTopVisitMonth(IEnumerable<Visit> visits)
    {
      var topVisitMonthNumber = visits.GroupBy(v => v.CreationTime.Month).OrderBy(g => g.Count()).Last().Key;
      TopVisitMonth = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(topVisitMonthNumber);
    }
    private void AssertValidAge()
    {
      if (Age < 1 || Age > HumanMaxAge)
      {
        throw new UserFriendlyException("Invalid birth date");
      }
    }
  }
}
