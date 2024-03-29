﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameHub.Application.Tournament.Queries.GetTournamentByEncodedName
{
    public class GetTournamentByEncodedNameQuery : IRequest<TournamentDto>
    {
        public string EncodedName { get; set; }

        public GetTournamentByEncodedNameQuery(string encodedName)
        {
            EncodedName = encodedName;
        }
    }
}
