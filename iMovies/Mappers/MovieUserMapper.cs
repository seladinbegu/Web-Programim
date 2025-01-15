using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iMovies.DTOs.MovieUser;
using iMovies.Models;

namespace iMovies.Controllers.Mappers;
public static class MovieUserMapper
{
    public static MovieUserDto toMovieUserDto(this MovieUser movieuserModel)
    {
        return new MovieUserDto
        {
            MovieId = movieuserModel.MovieId,
            UserId = movieuserModel.UserId
        };
    }









    public static MovieUser toMovieUserFromCreateDto(this MovieUserCreateDto movieuserDto)
    {
        return new MovieUser
        {
            MovieId = movieuserDto.MovieId,
            UserId = movieuserDto.UserId

        };
    }



}