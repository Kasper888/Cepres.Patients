using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using Xunit;
using Abp.Application.Services.Dto;
using Default;
using System;
using System.Linq.Dynamic.Core;
using System.Linq;
namespace Cepres.Patients.Tests.OData
{
  public class OData_Tests : PatientsTestBase
  {
    [Fact]
    public void GetPatients_Test()
    {
      // Arrange
      var serviceRoot = "http://localhost:21021/odata/";
      var context = new Container(new Uri(serviceRoot));

     var patients = context.Patient.Take(1).First().Visits.OrderByDescending(v=>v.CreationTime).Single();

      // Assert
  
    }
  }
}
