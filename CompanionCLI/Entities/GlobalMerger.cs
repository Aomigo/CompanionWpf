using CompanionCLI.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace CompanionCLI.Entities;

internal class GlobalMerger
{
    public List<Concert> Concerts { get; set; } = [];

    public DateTime FestivalStart { get; set; }

    public DateTime FestivalEnd { get; set; }
    public decimal Budget {  get; set; }
}
