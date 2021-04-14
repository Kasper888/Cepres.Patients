using Abp.Timing;
using System;
using System.ComponentModel.DataAnnotations;

namespace Cepres.Patients.Attributes
{
  public class DateInPastAttribute : RangeAttribute
  {
    public DateInPastAttribute(byte maxPastYears)
      : base(typeof(DateTime), Clock.Now.AddYears(-maxPastYears).ToString(), Clock.Now.ToString()) { }
  }
}
