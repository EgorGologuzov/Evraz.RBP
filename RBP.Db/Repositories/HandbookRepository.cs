using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using RBP.Services.Contracts;
using RBP.Services.Dto;
using RBP.Services.Exceptions;
using RBP.Services.Interfaces;
using RBP.Services.Models;
using RBP.Services.Utils;

namespace RBP.Db.Repositories
{
    public class HandbookRepository : IHandbookRepository
    {
        private static readonly MemoryCash<IList<HandbookEntityReturnDto>> _memoryCash = new();

        protected readonly PostgresContext Context;
        protected readonly ILogger Logger;
        protected readonly IHandbookValidator Validator;

        public HandbookRepository(PostgresContext context, ILogger<HandbookRepository> logger, IHandbookValidator validator)
        {
            Context = context;
            Logger = logger;
            Validator = validator;
        }

        public Task<HandbookEntityReturnDto> Get(string handbook, object id)
        {
            switch (handbook)
            {
                case nameof(Defect):
                    return Get<Defect>(id);

                case nameof(RailProfile):
                    return Get<RailProfile>(id);

                case nameof(SteelGrade):
                    return Get<SteelGrade>(id);

                case nameof(WorkshopSegment):
                    return Get<WorkshopSegment>(id);

                default:
                    throw new EntityNotExistsException(handbook);
            }
        }

        private async Task<HandbookEntityReturnDto> Get<T>(object id) where T : class
        {
            return (await Context.Set<T>().FindAsync(id)).ToJson().FromJson<HandbookEntityReturnDto>();
        }

        private async Task<T> TypedGet<T>(object id) where T : class
        {
            return await Context.Set<T>().FindAsync(id);
        }

        public async Task<HandbookEntityReturnDto> Create(HandbookEntityCreateDto data)
        {
            HandbookEntityReturnDto result = data.HandbookName switch
            {
                nameof(Defect) => await Create<Defect>(data),
                nameof(RailProfile) => await Create<RailProfile>(data),
                nameof(SteelGrade) => await Create<SteelGrade>(data),
                nameof(WorkshopSegment) => await Create<WorkshopSegment>(data),
                _ => throw new EntityNotExistsException(data.HandbookName),
            };

            _memoryCash.Remove(data.HandbookName);

            return result;
        }

        private async Task<HandbookEntityReturnDto> Create<T>(HandbookEntityCreateDto data) where T : class
        {
            Validator?.Validate(data);
            DbSet<T> dbSet = Context.Set<T>();
            string dataJson = data.ToJson();
            T result = (await dbSet.AddAsync(dataJson.FromJson<T>())).Entity;
            await Context.SaveChangesAsync();

            Logger.LogInformation("Created {handbook} creation data {data}", typeof(T).Name, dataJson);

            return result.ToJson().FromJson<HandbookEntityReturnDto>();
        }

        public async Task<HandbookEntityReturnDto> Delete(string handbook, object id)
        {
            HandbookEntityReturnDto result = handbook switch
            {
                nameof(Defect) => await Delete<Defect>(id),
                nameof(RailProfile) => await Delete<RailProfile>(id),
                nameof(SteelGrade) => await Delete<SteelGrade>(id),
                nameof(WorkshopSegment) => await Delete<WorkshopSegment>(id),
                _ => throw new EntityNotExistsException(handbook),
            };

            _memoryCash.Remove(handbook);

            return result;
        }

        private async Task<HandbookEntityReturnDto> Delete<T>(object id) where T : class
        {
            T entity = await TypedGet<T>(id);
            entity.ThrowIfNull(new EntityNotExistsException(id));
            T result = Context.Set<T>().Remove(entity).Entity;
            await Context.SaveChangesAsync();

            Logger.LogInformation("Deleted {type} with data {data}", typeof(T).Name, result.ToJson());

            return result.ToJson().FromJson<HandbookEntityReturnDto>();
        }

        public async Task<HandbookEntityReturnDto> Update(HandbookEntityUpdateDto data)
        {
            HandbookEntityReturnDto result = data.HandbookName switch
            {
                nameof(Defect) => await Update<Defect>(data),
                nameof(RailProfile) => await Update<RailProfile>(data),
                nameof(SteelGrade) => await Update<SteelGrade>(data),
                nameof(WorkshopSegment) => await Update<WorkshopSegment>(data),
                _ => throw new EntityNotExistsException(data.HandbookName),
            };

            _memoryCash.Remove(data.HandbookName);

            return result;
        }

        private async Task<HandbookEntityReturnDto> Update<T>(HandbookEntityUpdateDto data) where T : class
        {
            Validator?.Validate(data);
            T entity = await TypedGet<T>(data.Id);
            entity.ThrowIfNull(new EntityNotExistsException(data.Id));
            string original = entity.ToJson();
            EntityEntry<T> entry = Context.Entry(entity);
            entry.CurrentValues.SetValues(data);
            T result = entry.Entity;

            await Context.SaveChangesAsync();

            Logger.LogInformation("Updated {type} old data {oldData}", typeof(T).Name, original);

            return result.ToJson().FromJson<HandbookEntityReturnDto>();
        }

        public async Task<IList<HandbookEntityReturnDto>> GetAll(string handbook)
        {
            IList<HandbookEntityReturnDto>? result = _memoryCash.Get(handbook);

            if (result is not null)
            {
                return result;
            }

            result = handbook switch
            {
                nameof(Defect) => await GetAll<Defect>(),
                nameof(RailProfile) => await GetAll<RailProfile>(),
                nameof(SteelGrade) => await GetAll<SteelGrade>(),
                nameof(WorkshopSegment) => await GetAll<WorkshopSegment>(),
                _ => throw new EntityNotExistsException(handbook),
            };

            _memoryCash.Set(handbook, result);

            return result;
        }

        private async Task<IList<HandbookEntityReturnDto>> GetAll<T>() where T : class
        {
            return (await Context.Set<T>().ToListAsync()).ConvertAll(e => e.ToJson().FromJson<HandbookEntityReturnDto>());
        }
    }
}