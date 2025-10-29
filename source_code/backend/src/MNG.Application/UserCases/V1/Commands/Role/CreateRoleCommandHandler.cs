
using System.Security.Claims;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MNG.Contract.Abstractions.Message;
using MNG.Contract.Abstractions.Shared;
using MNG.Contract.Services.V1.Role;
using MNG.Domain.Abstractions;
using MNG.Domain.Entities.Identity;
using MNG.Domain.Exceptions;
using MNG.Persistence;

namespace MNG.Application.UserCases.V1.Commands.Role;
public sealed class CreateRoleCommandHandler : ICommandHandler<Command.CreateRoleCommand>
{
    private readonly IMapper _mapper;
    private readonly IPublisher _publisher;
    private readonly RoleManager<AppRole> _roleManager;
    private readonly IUnitOfWorkDbContext<ApplicationDbContext> _unitOfWork;


    public CreateRoleCommandHandler(IPublisher publisher, IMapper mapper, RoleManager<AppRole> roleManager, IUnitOfWorkDbContext<ApplicationDbContext> unitOfWork)
    {
        _mapper = mapper;
        _publisher = publisher;
        _roleManager = roleManager;
        _unitOfWork = unitOfWork;
    }
    public async Task<Result> Handle(Command.CreateRoleCommand request, CancellationToken cancellationToken)
    {
        var roleExists = await _roleManager.RoleExistsAsync(request.RoleName); // Không có cancellationToken ở đây nếu phương thức không hỗ trợ.
        if (roleExists)
            throw new RoleExceptions.RoleValidNameException(request.RoleName);

        var role = new AppRole() { Name = request.RoleName, Description = request.Description, RoleCode = request.RoleCode };
        var result = await _roleManager.CreateAsync(role); // Không có cancellationToken nếu phương thức không hỗ trợ.
        if (!result.Succeeded)
        {
            var errorMessage = string.Join(", ", result.Errors.Select(e => e.Description));
            return Result.Failure(new Error("CREATE_ROLE", $"Tạo vai trò thất bại: {errorMessage}. Vui lòng thử lại sau!"));
        }

        // Lấy danh sách các claims đã có.
        var existingClaims = await _roleManager.GetClaimsAsync(role); // Không có cancellationToken nếu phương thức không hỗ trợ.

        foreach (var item in request.Permissions)
        {
            var claim = new Claim(item.Key.ToString(), item.Value.ToString());
            if (!existingClaims.Any(c => c.Type == claim.Type && c.Value == claim.Value))
            {
                await _roleManager.AddClaimAsync(role, claim); // Không có cancellationToken nếu phương thức không hỗ trợ.
            }
        }

        return Result.Success();
    }

}
