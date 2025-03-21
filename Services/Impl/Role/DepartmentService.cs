namespace portal.Services;

using AutoMapper;
using portal.Db;
using portal.DTOs;
using portal.Models;

public class DepartmentService
    : BaseService<Department, DepartmentDTO, UpdateDepartmentDTO>,
        IDepartmentService
{
    public DepartmentService(
        ApplicationDbContext context,
        IMapper mapper,
        ILogger<DepartmentService> logger
    )
        : base(context, mapper, logger) { }
}
