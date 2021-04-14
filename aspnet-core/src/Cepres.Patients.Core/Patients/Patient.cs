using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Cepres.Patients.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Extensions;
using Abp.UI;
using Abp.Timing;
using Accord.Statistics;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Cepres.Patients.Patients
{
  public class Patient : Entity
  {
    public Patient()
    {
      Visits = new HashSet<Visit>();
    }
    [Required(AllowEmptyStrings = false), MinLength(2), MaxLength(50)]
    public string Name { get; set; }
    [Required(AllowEmptyStrings = false), MinLength(14), MaxLength(14), RegularExpression(@"^\d{14}$")]
    public string NationalId { get; set; }
    [DataType(DataType.Date), DateInPast(120)]
    public DateTime? BirthDate { get; set; }
    [EmailAddress, MaxLength(254)]
    public string Email { get; set; }
    public virtual HashSet<Visit> Visits { get; set; }
  }
}
