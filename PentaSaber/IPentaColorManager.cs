using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PentaSaber
{
    public interface IPentaColorManager
    {
        /// <summary>
        /// ID used to uniquely identify this type of <see cref="IPentaColorManager"/> (for user settings). 
        /// </summary>
        string Id { get; }
        /// <summary>
        /// User-friendly name of this <see cref="IPentaColorManager"/> type.
        /// </summary>
        string Name { get; }
        string? Description { get; }
        /// <summary>
        /// Set to true if score submission should be disabled.
        /// </summary>
        bool DisableScoreSubmission { get; }
        /// <summary>
        /// Gets the next <see cref="PentaNoteType"/> for the given note information.
        /// </summary>
        /// <param name="currentType"></param>
        /// <returns></returns>
        PentaNoteType GetNextNoteType(GameNoteController currentType);
    }
}
