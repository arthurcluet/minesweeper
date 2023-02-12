using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Rectangle = System.Windows.Shapes.Rectangle;

namespace Minesweeper
{
    class Case
    {
        bool hasMine;
        bool isFlagged;
        bool isRevealed;
        Rectangle display;

        public Case(Rectangle disp)
        {
            hasMine = false;
            isFlagged = false;
            display = disp;
        }

        public void setDisplay(Rectangle disp)
        {
            display = disp;
        }

        public Rectangle Display
        {
            get => display;
        }

        public void setMine(bool mine)
        {
            hasMine = mine;
        }

        public bool HasMine
        {
            get => hasMine;
            set
            {
                hasMine = value;
            }
        }

        public bool IsFlagged
        {
            get => isFlagged;
            set
            {
                isFlagged = value;
            }
        }

        public bool IsRevealed
        {
            get => isRevealed;
            set
            {
                isRevealed = value;
            }
        }
    }
}
