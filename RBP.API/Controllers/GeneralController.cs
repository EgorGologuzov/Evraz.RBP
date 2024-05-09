using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using RBP.Services.Contracts;
using RBP.Services.Exceptions;
using RBP.Services.Models;

namespace RBP.API.Controllers
{
    public class GeneralController : ControllerBase
    {
        protected readonly ILogger Logger;
        protected readonly IMapper Mapper;

        public GeneralController(ILogger logger, IMapper mapper)
        {
            Logger = logger;
            Mapper = mapper;
        }

        protected BadRequestObjectResult BadRequest<TException>(TException exception) where TException : Exception
        {
            return base.BadRequest(new BadRequestReturn
            {
                Exception = typeof(TException).Name,
                Message = exception.Message,
                Data = exception.Data
            });
        }

        [NonAction]
        protected async Task<IActionResult> TryResult(
            Func<Task<IActionResult>> action,
            Func<InvalidFieldValueException, Task<IActionResult>>? ifInvalidFieldValue = null,
            Func<EntityNotExistsException, Task<IActionResult>>? ifEntityNotExists = null,
            Func<PostgresException, Task<IActionResult>>? ifPkConflict = null,
            Func<PostgresException, Task<IActionResult>>? ifFkConflict = null,
            Func<PostgresException, Task<IActionResult>>? ifIxConflict = null,
            Func<Exception, Task<IActionResult>>? ifOther = null)
        {
            try
            {
                return await action.Invoke();
            }
            catch (InvalidFieldValueException ex)
            {
                return ifInvalidFieldValue is null ? BadRequest(ex) : await ifInvalidFieldValue.Invoke(ex);
            }
            catch (EntityNotExistsException ex)
            {
                return ifEntityNotExists is null ? BadRequest(ex) : await ifEntityNotExists.Invoke(ex);
            }
            catch (DbUpdateException ex) when (ex.InnerException is PostgresException pex)
            {
                string[] keys = pex.ConstraintName.Split("_");
                switch (keys[0])
                {
                    case "PK":
                        return ifPkConflict is null
                            ? BadRequest(new UniquenessViolationException("Нарушение уникальности первичного ключа", keys[1]))
                            : await ifPkConflict.Invoke(pex);

                    case "FK":
                        return ifFkConflict is null
                            ? BadRequest(new UniquenessViolationException("Нарушение ограничения внешнего ключа", keys[1], keys[3..]))
                            : await ifFkConflict.Invoke(pex);

                    case "IX":
                        return ifIxConflict is null
                            ? BadRequest(new UniquenessViolationException("Нарушение ограничения уникальности", keys[1], keys[2..]))
                            : await ifIxConflict.Invoke(pex);

                    default:
                        if (ifOther is null)
                        {
                            throw;
                        }

                        return await ifOther.Invoke(ex);
                }
            }
            catch (Exception ex) when (ifOther is not null)
            {
                return await ifOther.Invoke(ex);
            }
        }

        protected Guid ClientId => new(HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid).Value);
    }
}