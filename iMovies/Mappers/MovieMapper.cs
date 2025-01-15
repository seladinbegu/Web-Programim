using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iMovies.Models;
using iMovies.Models.DTOs.Movie;

namespace iMovies.Controllers.Mappers
{
    public static class MovieMapper
    {
        public static MovieDto toMovieDto(this Movie movieModel)
        {
            return new MovieDto
            {
                Id = movieModel.Id,
                Name = movieModel.Name,
                Duration = movieModel.Duration,
                ImgPath = movieModel.ImgPath,
                Genre = movieModel.Genre,
                ShowDate = movieModel.ShowDate
            };
        }





        public static Movie toMovieFromCreateDto(this MovieCreateDto movieDto)
        {
            return new Movie
            {
                Name = movieDto.Name,
                Duration = movieDto.Duration,
                ImgPath = movieDto.ImgPath,
                Genre = movieDto.Genre,
                ShowDate = movieDto.ShowDate

            };
        }







        public static Movie ToMovieFromUpdateDto(this MovieUpdateDto movieUpdateDto)
        {
            if (movieUpdateDto == null) throw new ArgumentNullException(nameof(movieUpdateDto));

            return new Movie
            {
                Name = movieUpdateDto.Name,
                Duration = movieUpdateDto.Duration,
                ImgPath = movieUpdateDto.ImgPath,
                Genre = movieUpdateDto.Genre,
                ShowDate = movieUpdateDto.ShowDate
            };
        }
    }
}