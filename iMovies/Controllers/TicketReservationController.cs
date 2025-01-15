using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iMovies.Data;
using iMovies.DTOs.TicketReservation;
using iMovies.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMovies.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TicketReservationController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TicketReservationController(ApplicationDbContext context)
        {
            _context = context;
        }




        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            List<TicketReservationDto> ticketreservations = await _context.TicketReservations.Select(tr => tr.toTicketReservationDto()).ToListAsync();
            if (ticketreservations == null) return NotFound();

            return Ok(ticketreservations);
        }






        [HttpGet("{movieId}/{userId}")]
        public async Task<IActionResult> Get(int movieId, string userId)
        {
            var ticketreservations = await _context.TicketReservations
            .Where(tr => tr.MovieId == movieId && tr.UserId == userId).Select(tr => tr.toTicketReservationDto()).ToListAsync();

            if (ticketreservations == null) return NotFound();

            return Ok(ticketreservations);
        }











        [HttpGet("{userId}")]
        public async Task<IActionResult> GetByUserId(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                return BadRequest("User ID cannot be null or empty.");

            var ticketreservations = await _context.TicketReservations
                .Where(tr => tr.UserId == userId)
                .Select(tr => tr.toTicketReservationDto())
                .ToListAsync();

            if (!ticketreservations.Any())
                return NotFound();

            return Ok(ticketreservations);
        }










        [HttpDelete("{movieId}/{userId}")]
        public async Task<IActionResult> Delete(int movieId, string userId)
        {
            var ticketreservation = await _context.TicketReservations
                .FirstOrDefaultAsync(tr => tr.MovieId == movieId && tr.UserId == userId);  // Find the specific reservation

            if (ticketreservation == null)
                return NotFound();  // Return 404 if no matching reservation is found

            _context.TicketReservations.Remove(ticketreservation);  // Remove the reservation
            await _context.SaveChangesAsync();  // Save changes to the database

            return NoContent();  // Return 204 No Content to indicate successful deletion
        }








        [HttpPost]
        public async Task<IActionResult> Create(TicketReservationCreateDto createDto)
        {
            var ticketreservationModel = createDto.toTicketReservationFromCreateDto();
            _context.TicketReservations.Add(ticketreservationModel);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetByUserId), new { userId = ticketreservationModel.UserId }, ticketreservationModel);
        }










    }
}