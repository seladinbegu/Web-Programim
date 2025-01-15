using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iMovies.DTOs.TicketReservation;
using iMovies.Models;

namespace iMovies.Mappers
{
    public static class TicketReservationMapper
    {
        public static TicketReservationDto toTicketReservationDto(this TicketReservation ticketReservationModel)
        {
            return new TicketReservationDto
            {
                Id = ticketReservationModel.Id,
                UserId = ticketReservationModel.UserId,
                MovieId = ticketReservationModel.MovieId,
                ReservationDate = ticketReservationModel.ReservationDate,
                ShowTime = ticketReservationModel.ShowTime,
                Amount = ticketReservationModel.Amount,
                PaymentMethod = ticketReservationModel.PaymentMethod
            };
        }


















        public static TicketReservation toTicketReservationFromCreateDto(this TicketReservationCreateDto ticketReservationDto)
        {
            return new TicketReservation
            {
                UserId = ticketReservationDto.UserId,
                MovieId = ticketReservationDto.MovieId,
                ReservationDate = ticketReservationDto.ReservationDate,
                ShowTime = ticketReservationDto.ShowTime,
                Amount = ticketReservationDto.Amount,
                PaymentMethod = ticketReservationDto.PaymentMethod
            };
        }
    }
}