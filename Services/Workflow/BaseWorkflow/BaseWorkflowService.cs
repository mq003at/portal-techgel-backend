using AutoMapper;
using Microsoft.EntityFrameworkCore;
using portal.Db;
using portal.DTOs;
using portal.Enums;
using portal.Models;

namespace portal.Services;



public abstract class BaseWorkflowService<TModel, TReadDTO, TCreateDTO, TUpdateDTO, TNodeModel>
    : BaseService<TModel, TReadDTO, TCreateDTO, TUpdateDTO>,
      IBaseWorkflowService<TModel, TReadDTO, TCreateDTO, TUpdateDTO, TNodeModel>
    where TModel : BaseWorkflow, new()
    where TReadDTO : BaseWorkflowDTO
    where TCreateDTO : BaseWorkflowCreateDTO
    where TUpdateDTO : BaseWorkflowUpdateDTO
    where TNodeModel : BaseWorkflowNode 
{
    protected new readonly ApplicationDbContext _context;
    protected new readonly IMapper _mapper;
    protected new readonly ILogger<BaseWorkflowService<TModel, TReadDTO, TCreateDTO, TUpdateDTO, TNodeModel>> _logger;

    public BaseWorkflowService(
        ApplicationDbContext context,
        IMapper mapper,
        ILogger<BaseWorkflowService<TModel, TReadDTO, TCreateDTO, TUpdateDTO, TNodeModel>> logger
    ) : base(context, mapper, logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }


}