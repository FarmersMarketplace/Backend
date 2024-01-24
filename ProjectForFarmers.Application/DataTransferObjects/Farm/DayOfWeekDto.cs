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
        public string StartHour { get; set; }
        public string StartMinute { get; set; }
        public string EndHour { get; set; }
        public string EndMinute { get; set; }

        
    }

}
