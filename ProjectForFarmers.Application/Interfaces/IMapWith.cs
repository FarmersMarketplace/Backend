using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ProjectForFarmers.Application.Interfaces
{
    public interface IMapWith<T>
    {
        void Mapping(Profile profile);
    }

}
