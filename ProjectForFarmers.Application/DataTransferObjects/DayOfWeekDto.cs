using AutoMapper;
using FarmersMarketplace.Application.Interfaces;
using FarmersMarketplace.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DayOfWeek = FarmersMarketplace.Domain.DayOfWeek;

namespace FarmersMarketplace.Application.DataTransferObjects
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
