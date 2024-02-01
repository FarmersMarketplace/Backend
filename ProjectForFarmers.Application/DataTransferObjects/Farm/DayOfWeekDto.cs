using AutoMapper;
using ProjectForFarmers.Application.Interfaces;
using ProjectForFarmers.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DayOfWeek = ProjectForFarmers.Domain.DayOfWeek;

namespace ProjectForFarmers.Application.DataTransferObjects.Farm
{
    public class DayOfWeekDto
    {
        public bool IsOpened { get; set; }
        public byte? StartHour { get; set; }
        public byte? StartMinute { get; set; }
        public byte? EndHour { get; set; }
        public byte? EndMinute { get; set; }
    }

}
