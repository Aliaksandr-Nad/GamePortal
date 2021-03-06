﻿using AliaksNad.Battleship.Logic.Validators;
using AliaksNad.Battleship.Logic.Validators.Game;
using FluentValidation.Attributes;

namespace AliaksNad.Battleship.Logic.Models.Game
{
    [Validator(typeof(CoordinatesDtoValidator))]
    public class CoordinatesDto
    {
        public int CoordinateX { get; set; }

        public int CoordinateY { get; set; }
    }
}