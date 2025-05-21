using System.Text.Json;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using portal.Db;
using portal.DTOs;
using portal.Models;

namespace portal.Services;

public class EmployeeService
    : BaseService<Employee, EmployeeDTO, CreateEmployeeDTO, UpdateEmployeeDTO>,
        IEmployeeService
{
    public EmployeeService(
        ApplicationDbContext context,
        IMapper mapper,
        ILogger<EmployeeService> logger
    )
        : base(context, mapper, logger) { }
}
