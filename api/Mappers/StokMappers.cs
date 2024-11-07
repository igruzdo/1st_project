using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Stok;
using api.Models;

namespace api.Mappers
{
    public static class StokMappers
    {
        public static StokDto ToStokDto(this Stok stokModel)
        {
            return new StokDto
            {
                Id = stokModel.Id,
                Symbol = stokModel.Symbol,
                CompanyName = stokModel.CompanyName,
                Purchase = stokModel.Purchase,
                LastDiv = stokModel.LastDiv,
                Industry = stokModel.Industry,
                MarketCap = stokModel.MarketCap,
            };
        }
    }
}