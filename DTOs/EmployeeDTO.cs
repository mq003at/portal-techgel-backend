namespace portal.DTOs;

using portal.Models;

public class EmployeeDTO : BaseDTO<Employee>
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? MiddleName { get; set; }

    public Gender Gender { get; set; }
    public DateTime DateOfBirth { get; set; }

    public string? PersonalEmail { get; set; }
    public string? CompanyEmail { get; set; }

    public string? PhoneNumber { get; set; }
    public string? CompanyNumber { get; set; }

    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    public DateTime? ProbationStartDate { get; set; }
    public DateTime? ProbationEndDate { get; set; }

    public EmploymentStatus Status { get; set; }
    public string? Position { get; set; }
    public int? ManagerId { get; set; }
    public string? Address { get; set; }

    public decimal? Salary { get; set; } = 0.00m;

    public List<int> DivisionIds { get; set; } = new();
    public List<int> DepartmentIds { get; set; } = new();
    public List<int> SectionIds { get; set; } = new();
    public List<int> UnitIds { get; set; } = new();
    public List<int> TeamIds { get; set; } = new();

    public override void UpdateModel(Employee model) { }
}

public class CreateEmployeeDTO : BaseDTO<Employee>
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? MiddleName { get; set; }

    public Gender Gender { get; set; }
    public DateTime DateOfBirth { get; set; }

    public string PersonalEmail { get; set; } = null!;
    public string? CompanyEmail { get; set; }

    public string PhoneNumber { get; set; } = null!;
    public string? CompanyNumber { get; set; }

    public DateTime StartDate { get; set; }
    public DateTime? ProbationStartDate { get; set; }
    public DateTime? ProbationEndDate { get; set; }

    public string Position { get; set; } = null!;
    public EmploymentStatus Status { get; set; } = EmploymentStatus.Active;
    public int? ManagerId { get; set; }
    public decimal? Salary { get; set; } = 0.00m;
    public string? Address { get; set; }
    public List<int> DivisionIds { get; set; } = new();
    public List<int> DepartmentIds { get; set; } = new();
    public List<int> SectionIds { get; set; } = new();
    public List<int> UnitIds { get; set; } = new();
    public List<int> TeamIds { get; set; } = new();

    public override void UpdateModel(Employee model) { }
}

public class UpdateEmployeeDTO : BaseDTO<Employee>
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? MiddleName { get; set; }

    public Gender? Gender { get; set; }
    public DateTime? DateOfBirth { get; set; }

    public string? PersonalEmail { get; set; }
    public string? CompanyEmail { get; set; }

    public string? PhoneNumber { get; set; }
    public string? CompanyNumber { get; set; }

    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public DateTime? ProbationStartDate { get; set; }
    public DateTime? ProbationEndDate { get; set; }

    public EmploymentStatus? Status { get; set; }
    public string? Position { get; set; }
    public int? ManagerId { get; set; }
    public decimal? Salary { get; set; } = 0.00m;
    public string? Address { get; set; }
    public List<int> DivisionIds { get; set; } = new();
    public List<int> DepartmentIds { get; set; } = new();
    public List<int> SectionIds { get; set; } = new();
    public List<int> UnitIds { get; set; } = new();
    public List<int> TeamIds { get; set; } = new();

    public override void UpdateModel(Employee model)
    {
        if (FirstName != null)
            model.FirstName = FirstName;
        if (LastName != null)
            model.LastName = LastName;
        if (MiddleName != null)
            model.MiddleName = MiddleName;

        if (Gender.HasValue)
            model.Gender = Gender.Value;
        if (DateOfBirth.HasValue)
            model.DateOfBirth = DateOfBirth.Value;

        if (PersonalEmail != null)
            model.PersonalEmail = PersonalEmail;
        if (CompanyEmail != null)
            model.CompanyEmail = CompanyEmail;

        if (PhoneNumber != null)
            model.PhoneNumber = PhoneNumber;
        if (CompanyNumber != null)
            model.CompanyNumber = CompanyNumber;

        if (Address != null)
            model.Address = Address;

        if (MainID != null)
            model.MainID = MainID;
        if (StartDate.HasValue)
            model.StartDate = StartDate.Value;
        if (EndDate.HasValue)
            model.EndDate = EndDate.Value;
        if (ProbationStartDate.HasValue)
            model.ProbationStartDate = ProbationStartDate.Value;
        if (ProbationEndDate.HasValue)
            model.ProbationEndDate = ProbationEndDate.Value;

        if (Status.HasValue)
            model.Status = Status.Value;
        if (Position != null)
            model.Position = Position;
        if (ManagerId.HasValue)
            model.ManagerId = ManagerId.Value;
        if (Salary.HasValue)
            model.Salary = Salary.Value;
        if (DivisionIds != null)
        {
            model.EmployeeDivisions.Clear();
            foreach (var id in DivisionIds)
                model.EmployeeDivisions.Add(
                    new EmployeeDivision { EmployeeId = model.Id, DivisionId = id }
                );
        }

        if (DepartmentIds != null)
        {
            model.EmployeeDepartments.Clear();
            foreach (var id in DepartmentIds)
                model.EmployeeDepartments.Add(
                    new EmployeeDepartment { EmployeeId = model.Id, DepartmentId = id }
                );
        }

        if (SectionIds != null)
        {
            model.EmployeeSections.Clear();
            foreach (var id in SectionIds)
                model.EmployeeSections.Add(
                    new EmployeeSection { EmployeeId = model.Id, SectionId = id }
                );
        }

        if (UnitIds != null)
        {
            model.EmployeeUnits.Clear();
            foreach (var id in UnitIds)
                model.EmployeeUnits.Add(new EmployeeUnit { EmployeeId = model.Id, UnitId = id });
        }

        if (TeamIds != null)
        {
            model.EmployeeTeams.Clear();
            foreach (var teamId in TeamIds.Distinct())
            {
                model.EmployeeTeams.Add(
                    new EmployeeTeam { EmployeeId = model.Id, TeamId = teamId }
                );
            }
        }
    }
}
