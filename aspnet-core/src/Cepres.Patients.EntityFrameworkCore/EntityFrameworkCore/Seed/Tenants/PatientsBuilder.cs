using Abp.Timing;
using Cepres.Patients.Patients;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Cepres.Patients.EntityFrameworkCore.Seed
{
  internal class PatientsBuilder
  {
    private readonly PatientsDbContext _context;
    private readonly Random _rndm = new Random();
    private const string Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnpqrstuvwxyz";
    private const string CharsWithSpaces = "ABCDEFGHIJKLMNOPQRSTUVWXYZ      abcdefghijklmnpqrstuvwxyz";
    // MinutesInYear = Month * Day * Hour * Minute
    private const int MinutesInYear = 12 * 30 * 24 * 60;
    private const int HumanMaxAge = 120;
    private readonly string[] _diseases = new string[]
    {
      "COVID-19",
      "Diabetes",
      "Depression",
      "Flu",
      "Cancer",
      "Coma",
      "Poroma",
      "Eczema",
      "Malaria",
      "Headaches",
      "Anemia",
      "Acne",
      "Gliosis",
      "Obesity",
      "Itching",
      "Anxiety",
      "Asthma",
      "Faty",
      "Bunion",
      "Cough",
      "Croup",
      "Earache",
      "Poisoning",
      "Pleurisy",
      "Gout",
      "Mumps",
      "Noro-Virus",
      "Virus-A",
      "Virus-B",
      "Virus-C",
      "Virus-D",
      "Virus-E",
      "Virus-F",
      "Virus-G",
      "Virus-H",
      "Virus-I",
      "Virus-J",
      "Virus-K",
      "Virus-L",
      "Virus-M",
      "Virus-N",
      "Virus-P",
      "Virus-O",
      "Virus-Q",
      "Virus-R",
      "Virus-S",
      "Virus-T",
      "Virus-U",
      "Virus-V",
      "Virus-W",
      "Virus-X",
      "Virus-Y",
      "Virus-Z"
    };

    public PatientsBuilder(PatientsDbContext context)
    {
      _context = context;
    }

    public void Create()
    {
      CreatePaitentsAndVisits();
    }
    /// <summary>
    ///  Seed random Patients & Visists to cover all test cases including Patient Statistics Page.
    ///  It Generate 500 random Patients with Visists in one year back assuming the Patient have from 1 to 12 Visits during this year.
    /// </summary>
    private void CreatePaitentsAndVisits()
    {
      // Look for any Patients.
      if (_context.Patient.IgnoreQueryFilters().Any())
        // DB has been seeded
        return;
   
      var patients = Enumerable.Range(1, 500).Select(x => new Patient
      {
        Name = $"{RandomString(_rndm.Next(3, 8))} {RandomString(_rndm.Next(3, 8))}",
        BirthDate = Clock.Now.AddMinutes(-_rndm.Next(MinutesInYear, MinutesInYear * HumanMaxAge)),
        NationalId = $"6712358490{x + 1000}",
        Email = $"{RandomString(_rndm.Next(4, 20))}@Innotech.com",
        Visits = Enumerable.Range(1, _rndm.Next(1, 15)).Select(y => new Visit
        {
          CreationTime = Clock.Now.AddMinutes(-_rndm.Next(1, MinutesInYear)),
          Disease = _diseases[_rndm.Next(_diseases.Length)],
          Description = RandomString(_rndm.Next(20, 800), true),
          Fees = _rndm.Next(30, 3000) * 1.13d
        }).ToHashSet()
      });
      _context.Patient.AddRange(patients);
      _context.SaveChanges();
    }

    private string RandomString(int length, bool hasSpaces = false)
    {
      return new string(Enumerable.Repeat(hasSpaces ? CharsWithSpaces : Chars, length)
        .Select(s => s[_rndm.Next(s.Length)]).ToArray());
    }
  }
}