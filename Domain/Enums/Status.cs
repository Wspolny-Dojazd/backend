using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enums;

/// <summary>
/// Status enum that defines the start state of the route.
/// </summary>
public enum Status
{
    /// <summary>
    /// Not started state.
    /// </summary>
    NotStarted,

    /// <summary>
    /// Started state.
    /// </summary>
    Started,
}
