using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;
using api.Data;
using api.Dto;
using api.Models;
using Microsoft.EntityFrameworkCore;
using api.Service;

namespace api.Service
{
    public class TemperatureProgramService : ITemperatureProgramService
    {
        private readonly ApplicationDBContext _dbContext;

        public TemperatureProgramService(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<TemperatureProgram>> GetAllProgramsAsync()
        {
            return await _dbContext.TemperaturePrograms
                                   .Include(p => p.Intervals)
                                   .ToListAsync();
        }

        public async Task<TemperatureProgram> UpsertProgramAsync(TemperatureProgramDto dto)
        {
            if (dto.Intervals == null)
                throw new ArgumentException("Lista de intervale nu poate fi null.");

            if (dto.Intervals.Count > 6)
                throw new ArgumentException("Un program nu poate conține mai mult de 6 intervale.");

            var intervalsParsed = new List<TemperatureInterval>();
            foreach (var intDto in dto.Intervals)
            {
                if (!TimeSpan.TryParseExact(intDto.Start, @"hh\:mm", CultureInfo.InvariantCulture, out var start))
                    throw new ArgumentException($"Format timp invalid pentru Start: '{intDto.Start}' (trebuie 'HH:mm').");

                if (!TimeSpan.TryParseExact(intDto.End, @"hh\:mm", CultureInfo.InvariantCulture, out var end))
                    throw new ArgumentException($"Format timp invalid pentru End: '{intDto.End}' (trebuie 'HH:mm').");

                if (start >= end)
                    throw new ArgumentException($"În interval {intDto.Start} - {intDto.End}, ora de început trebuie să fie înaintea orei de sfârșit.");

                if (intDto.Temperature < 5 || intDto.Temperature > 35)
                    throw new ArgumentException($"Temperatura {intDto.Temperature} este în afara intervalului 5–35.");

                intervalsParsed.Add(new TemperatureInterval
                {
                    Start = start,
                    End = end,
                    Temperature = intDto.Temperature
                });
            }

            var existing = await _dbContext.TemperaturePrograms
                                           .Include(p => p.Intervals)
                                           .FirstOrDefaultAsync(p => p.Name.ToLower() == dto.Name.ToLower());

            if (existing == null)
            {
                var newProgram = new TemperatureProgram
                {
                    Name = dto.Name.ToLower(),
                    Intervals = intervalsParsed
                };
                _dbContext.TemperaturePrograms.Add(newProgram);
                await _dbContext.SaveChangesAsync();
                return newProgram;
            }
            else
            {
                _dbContext.TemperatureIntervals.RemoveRange(existing.Intervals);
                existing.Intervals = intervalsParsed;
                _dbContext.TemperaturePrograms.Update(existing);
                await _dbContext.SaveChangesAsync();
                return existing;
            }
        }

        public async Task<TemperatureProgram> SelectActiveProgramAsync(string programName)
        {
            // Resetăm toate programele: IsActive = false
            var toate = await _dbContext.TemperaturePrograms.ToListAsync();
            foreach (var p in toate)
            {
                // presupunem că ai adăugat proprietatea IsActive în model:
                p.IsActive = false;
            }

            var prog = await _dbContext.TemperaturePrograms
                                       .Include(p => p.Intervals)
                                       .FirstOrDefaultAsync(p => p.Name.ToLower() == programName.ToLower());

            if (prog == null)
                throw new KeyNotFoundException($"Programul cu numele '{programName}' nu există.");

            prog.IsActive = true;
            await _dbContext.SaveChangesAsync();

            // Construim mesajul întreg cu toată schema (opțional, dacă vrei și asta)
            var scheduleMsg = BuildScheduleMessage(prog);
            await WebSocketHandler.BroadcastMessageAsync(scheduleMsg);

            return prog;
        }

        public async Task<TemperatureProgram> GetActiveProgramAsync()
        {
            var active = await _dbContext.TemperaturePrograms
                                         .Include(p => p.Intervals)
                                         .FirstOrDefaultAsync(p => EF.Property<bool>(p, "IsActive") == true);
            return active;
        }

        public string BuildScheduleMessage(TemperatureProgram program)
        {
            var parts = new List<string>();
            foreach (var interval in program.Intervals.OrderBy(i => i.Start))
            {
                var start = interval.Start.ToString(@"hh\:mm");
                var end = interval.End.ToString(@"hh\:mm");
                var temp = interval.Temperature;
                parts.Add($"{start}-{end}-{temp}");
            }

            var joined = string.Join(",", parts);
            return $"Schedule:{program.Name.ToLower()}|{joined}";
        }

        // ──────────────────────────────────────────────────────────────────────────────────
        // Aici începe partea nouă: GetCurrentTemperatureAsync()
        // ──────────────────────────────────────────────────────────────────────────────────
        public async Task<int?> GetCurrentTemperatureAsync()
        {
            // 1. Găsim programul activ
            var activeProgram = await GetActiveProgramAsync();
            if (activeProgram == null) return null;

            // 2. Aflăm ora curentă (server-side). Dacă vrei să fie Bucharest-time, asigură‐te că
            //    server-ul folosește Europe/Bucharest ca zonă locală sau convertești în TimeZoneInfo.
            var now = DateTime.Now.TimeOfDay;

            // 3. Găsim intervalul în care ne încadrăm: Start <= now < End
            var matched = activeProgram.Intervals
                                       .FirstOrDefault(i => i.Start <= now && now < i.End);

            if (matched == null)
                return null;

            return matched.Temperature;
        }



        public async Task<TemperatureProgram?> UpdateProgramIntervalsAsync(string name, List<TemperatureIntervalDto> intervalDtos)
        {
            var program = await _dbContext.TemperaturePrograms
                                          .Include(p => p.Intervals)
                                          .FirstOrDefaultAsync(p => p.Name.ToLower() == name.ToLower());

            if (program == null)
                return null;

            if (intervalDtos.Count > 6)
                throw new ArgumentException("Un program nu poate avea mai mult de 6 intervale.");

            foreach (var intDto in intervalDtos)
            {
                if (!TimeSpan.TryParseExact(intDto.Start, @"hh\:mm\:ss", CultureInfo.InvariantCulture, out var start))
                    throw new ArgumentException($"Format timp invalid pentru Start: '{intDto.Start}'.");

                if (!TimeSpan.TryParseExact(intDto.End, @"hh\:mm\:ss", CultureInfo.InvariantCulture, out var end))
                    throw new ArgumentException($"Format timp invalid pentru End: '{intDto.End}'.");

                if (start >= end)
                    throw new ArgumentException($"Ora de început trebuie să fie înaintea orei de sfârșit: {intDto.Start} - {intDto.End}.");

                if (intDto.Temperature < 5 || intDto.Temperature > 35)
                    throw new ArgumentException($"Temperatura {intDto.Temperature} este în afara intervalului permis (5–35).");

                // Găsim intervalul existent după ID
                var intervalToUpdate = program.Intervals.FirstOrDefault(i => i.Id == intDto.Id);
                if (intervalToUpdate == null)
                    throw new ArgumentException($"Nu s-a găsit intervalul cu ID-ul {intDto.Id} pentru programul '{name}'.");

                // Modificăm valorile dorite
                intervalToUpdate.Start = start;
                intervalToUpdate.End = end;
                intervalToUpdate.Temperature = intDto.Temperature;
            }

            await _dbContext.SaveChangesAsync();

            return program;
        }






    }
}