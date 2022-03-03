using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PentaSaber.ColorManagers
{
    public class TestColorManager : IPentaColorManager
    {
        public string Id => "TestColorManager";

        public string Name => "Test Color Manager";

        public string? Description => "Color manager used for testing";

        private readonly IInputController _inputController;
        public TestColorManager(IInputController inputController)
        {
            _inputController = inputController;
        }
        int indexA = 0;
        int indexB = 0;
        public PentaNoteType GetNextNoteType(GameNoteController note)
        {
            ColorType currentType = note.noteData.colorType;
            PentaNoteType noteType = PentaNoteType.None;
            //if (_inputController is ITestInputController testInput)
            //{
            //    if (currentType == ColorType.ColorA)
            //    {
            //        noteType = testInput.NoteAToggled ? PentaNoteType.ColorA2 : PentaNoteType.ColorA1;
            //    }console 
            //    else if (currentType == ColorType.ColorB)
            //    {
            //        noteType = testInput.NoteBToggled ? PentaNoteType.ColorB2 : PentaNoteType.ColorB1;
            //    }
            //}
            //else
            {
                if (currentType == ColorType.ColorA)
                {
                    if (indexA > 10)
                    {
                        noteType = PentaNoteType.Neutral;
                    }
                    else if (indexA > 5)
                        noteType = PentaNoteType.ColorA2;
                    else
                        noteType = PentaNoteType.ColorA1;
                    indexA++;
                    if (indexA > 13)
                        indexA = 0;
                }
                else
                {
                    if (indexB > 10)
                    {
                        noteType = PentaNoteType.Neutral;
                    }
                    else if (indexB > 5)
                        noteType = PentaNoteType.ColorB2;
                    else
                        noteType = PentaNoteType.ColorB1;
                    indexB++;
                    if (indexB > 13)
                        indexB = 0;
                }
            }

            return noteType;
        }
    }
}
