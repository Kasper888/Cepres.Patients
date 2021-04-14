using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Abp.Timing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cepres.Patients.Patients
{
  public class Visit : Entity, IHasCreationTime
  {
    public int PatientId { get; protected set; }
    public Patient Patient { get; set; }
    [MaxLength(800)]
    public string Description { get; set; }
    [Required(AllowEmptyStrings = false), MinLength(3), MaxLength(100)]
    public string Disease { get; set; }
    [Range(0, double.MaxValue)]
    public double Fees { get; set; }
    public DateTime CreationTime { get; set; }
  }
}
